public class RSI {
    public static double Calculate(double[] prices)
    {
        double sumGain = 0, sumLoss = 0;
        for (int i = 1; i < prices.Length; i++)
        {
            var difference = prices[i] - prices[i - 1];
            
            if (difference >= 0)
            {
                sumGain += difference;
            }
            else
            {
                sumLoss -= difference;
            }
        }

        var avgGain = sumGain/prices.Length;
        var avgLoss = sumLoss/prices.Length;
        var rs = avgGain/avgLoss;
        return 100- (100/(1+rs));
    }
}