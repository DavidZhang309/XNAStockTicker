using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace XNAStockTicker
{
    public class StockRequestState
    {
        public StreamReader DataStream { get; set; }
        public string Data { get; set; }
        public WebRequest HttpRequest { get; private set; }
        public Stock Stock { get; private set; }

        public int DeltaTime { get; set; }

        public StockRequestState(Stock stock, WebRequest request)
        {
            Stock = stock;
            HttpRequest = request;
        }
    }

    public class StockUpdater
    {
        private int arrIndex = 20;
        private List<StockRequestState> requests;
        private Queue<StockRequestState> finish;

        public StockUpdater()
        {
            requests = new List<StockRequestState>();
            finish = new Queue<StockRequestState>();
        }

        public Stock[] Stocks { get; set; }

        public void Request(int index)
        {
            WebRequest request = WebRequest.Create("http://download.finance.yahoo.com/d/quotes.csv?s=" + Stocks[index].Symbol.Replace('.', '-') + ".TO&f=l1c1&e=.csv/");
            StockRequestState state = new StockRequestState(Stocks[index], request);
            requests.Add(state);

            request.BeginGetResponse(new AsyncCallback(GotResponse), state);
        }
        public void NextRequest()
        {
            Request(arrIndex++);
        }

        public void Initialize()
        {
            for (int i = 0; i < 20; i++)
            {
                Request(i);
            }
        }

        public void GotResponse(IAsyncResult result)
        {
            StockRequestState state = result.AsyncState as StockRequestState;
            WebResponse response = state.HttpRequest.EndGetResponse(result);
            state.DataStream = new StreamReader(response.GetResponseStream());
            state.Data = state.DataStream.ReadToEnd();
            lock (finish)
                finish.Enqueue(state);
        }

        public void Update()
        {
            while (finish.Count > 0)
            {
                StockRequestState state = finish.Dequeue();
                string[] data = state.Data.Split(',');
                Stock stock = state.Stock;
                if (data[0] == "N/A")
                {
                    Console.WriteLine("Stock: {0}({1}) Has no price", stock.Name, stock.Symbol);
                    stock.Price = -1;
                }
                else
                {
                    stock.Price = (float)Convert.ToDouble(data[0]);
                    stock.Change = (float)Convert.ToDouble(data[1]);
                    Console.WriteLine("Data for {0}: {1} {2}", stock.Symbol, stock.Price, stock.Change);
                }
            }
        }
    }
}
