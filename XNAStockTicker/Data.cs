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

        public static bool VerifyUsefulness(string stockSymbol)
        {
            string[] data = stockSymbol.Split('.');
            return !(data.Length >= 3);
        }

        public static Stock[] LoadStocks()
        {
            List<Stock> stocks = new List<Stock>();
            string[] lines = File.ReadAllLines("TSX.txt");
            for (int i = 0; i < lines.Length; i++)
            {
                string[] data = lines[i].Split('\t');
                if (VerifyUsefulness(data[0]))
                    stocks.Add(new Stock() { Symbol = data[0], Name = data[1] });
            }
            return stocks.ToArray();
        }
    }

    
}
