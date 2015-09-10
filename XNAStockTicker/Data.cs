using System.Collections.Generic;
using System.IO;

namespace XNAStockTicker
{
    public class Stock
    {
        public string Symbol { get; set; }
        public string Name { get; set; }
        public float Price { get; set; }
        public float Change { get; set; }

        /// <summary>
        /// Loads Stocks from text file. Format of each line is following:
        /// TICKER\tNAME
        /// where \t is a tab character seperating the ticker symbol and name
        /// </summary>
        /// <param name="stockFile"></param>
        /// <returns></returns>
        public static Stock[] LoadStocks(string stockFile)
        {
            List<Stock> stocks = new List<Stock>();
            string[] lines = File.ReadAllLines(stockFile);
            for (int i = 0; i < lines.Length; i++)
            {
                string[] data = lines[i].Split('\t');
                stocks.Add(new Stock() { Symbol = data[0], Name = data[1] });
            }
            return stocks.ToArray();
        }
    }

    
}
