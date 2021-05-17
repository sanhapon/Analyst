using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Npgsql;

    public class DbHelper {
        public static T ExecuteReader<T>(string sql, Func<IDataReader, T> callback) {
            var constr = "Server=127.0.0.1;Port=5432;Database=hugo;User Id=postgres;Password=password";
            using var conn = new NpgsqlConnection(constr);
            using var cmd = new NpgsqlCommand(sql, conn);
            conn.Open();
            
            using var reader = cmd.ExecuteReader();
            T ret = callback(reader);
            reader.Close();
            conn.Close();
            return ret;
        }

        public static void ExecuteInsert(string sql) {
            var constr = "Server=127.0.0.1;Port=5432;Database=hugo;User Id=postgres;Password=password";
            using var conn = new NpgsqlConnection(constr);
            using var cmd = new NpgsqlCommand(sql, conn);
            conn.Open();
            cmd.Prepare();
            cmd.ExecuteNonQuery();
            conn.Close();
        }
    }