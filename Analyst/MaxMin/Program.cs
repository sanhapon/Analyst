using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace MaxMin
{
    class Point {
        public DateTime DT {get;set;}
        public double Close {get;set;}    
    }

    class Program
    {
        static void Main(string[] args)
        {
            string sql = "select closetime, close from klines;";
            List<Point> points = DbHelper.ExecuteReader<List<Point>>(sql, Read);
            // FindSupport(points);
            
            CalCulateRSI(points.ToArray());
          

        }

        static List<Point> Read(IDataReader reader) {
            var points = new List<Point>();

            while(reader.Read()) {
                var p = new Point() {
                    DT = reader.GetDateTime(0).ToUniversalTime(),
                    Close = reader.GetDouble(1)
                };

                points.Add(p);
            }
            return points;
        }

        static void CalCulateRSI(Point[] points) {
            for(int i=14; i<points.Length; i++) {
                var list = points[(i-14) .. i];
                var prices = list.Select(p => p.Close);
                var rsi = RSI.Calculate(prices.ToArray());

                Console.WriteLine(list.Length + ":" +  list[list.Length-1].DT + ":\t" + rsi);

                 var sql = string.Format(@"INSERT INTO Indicators (symbol, time, rsi) VALUES ('{0}','{1}','{2}');",
                        "DOTUSDT",
                        list[list.Length-1].DT,
                        rsi
                    );
                 DbHelper.ExecuteInsert(sql);
            
            }
        }

        static void FindSupport(List<Point> points) {
            double min = double.MaxValue;
            int count = 0;
            DateTime dateTime = DateTime.Today;

            var minima = new List<Point>();

            foreach(var p in points) {
                if (p.Close < min) {
                    min = p.Close;
                    dateTime = p.DT;
                    // Console.WriteLine(min);
                } 

                count++;

                if (count == 11) {
                    // Console.WriteLine("-----------------");
                    Console.WriteLine(dateTime + ":  " + min);
                    minima.Add(new Point() {DT = dateTime, Close = min});
                    var sql = string.Format(@"INSERT INTO Minimal (symbol, time, price) VALUES ('{0}','{1}','{2}');",
                        "DOTUSDT",
                        dateTime,
                        min
                    );
                    DbHelper.ExecuteInsert(sql);
                    count = 0;
                    min = double.MaxValue;
                }
            }
        }
    }
}
