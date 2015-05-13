using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XNAStockTicker
{
    public class StockDisplay
    {
        public const int SPEED = 2;

        private int stockIndex;
        private int delta = 700;

        public event EventHandler<EventArgs> StockGone;

        public Point Resolution { get; set; }
        public SpriteFont SymFont { get; set; }
        public SpriteFont NumFont { get; set; }
        public Stock[] Stocks { get; set; }

        public void Draw(SpriteBatch batch)
        {
            batch.Begin();
            delta -= SPEED;
            int x = delta;
            int currentStock = stockIndex;

            do
            {
                Stock stock = Stocks[currentStock];

                Vector2 symBound = SymFont.MeasureString(stock.Symbol);
                Vector2 priceBound = NumFont.MeasureString(stock.Price.ToString());
                Vector2 changeBound = NumFont.MeasureString(stock.Change.ToString());

                int ticklength = (int)(symBound.X + priceBound.X + changeBound.X + 15 + 10);

                if (currentStock == stockIndex && ticklength <= -delta)
                {
                    delta += ticklength;
                    stockIndex++;
                    if (stockIndex == Stocks.Length)
                        stockIndex = 0;
                    if (StockGone != null) StockGone(this, new EventArgs());
                    continue;
                }
                if (stock.Price == -1)
                {
                    batch.DrawString(SymFont, stock.Symbol, new Vector2(x, 0), Color.Gray);
                    batch.DrawString(NumFont, stock.Price.ToString(), new Vector2(x + symBound.X + 5, 0), Color.Gray);
                    batch.DrawString(NumFont, Math.Abs(stock.Change).ToString(), new Vector2(x + symBound.X + priceBound.X + 10, 0), Color.Gray);
                }
                else
                {
                    batch.DrawString(SymFont, stock.Symbol, new Vector2(x, 0), Color.Black);
                    batch.DrawString(NumFont, stock.Price.ToString(), new Vector2(x + symBound.X + 5, 0), Color.Black);
                    batch.DrawString(NumFont, Math.Abs(stock.Change).ToString(), new Vector2(x + symBound.X + priceBound.X + 10, 0), stock.Change == 0 ? Color.Gray : (stock.Change > 0 ? Color.Green : Color.Red));
                }
                currentStock++;
                if (currentStock == Stocks.Length) currentStock = 0;
                x += ticklength;
            } while (x < Resolution.X);

            batch.End();
        }
    }
}
