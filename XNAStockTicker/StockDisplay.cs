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

                Color symColor = stock.Price == -1 ? Color.Gray : Color.Black;
                Color priceColor = stock.Price == -1 ? Color.Gray : Color.Black;
                Color changeColor = Color.Gray;
                if (stock.Price != -1)
                    changeColor = stock.Change == 0 ? Color.Gray : (stock.Change > 0 ? Color.Green : Color.Red);

                Vector2 position = new Vector2(x, 0);
                batch.DrawString(SymFont, stock.Symbol, position, symColor);
                position.X += symBound.X + 5;
                batch.DrawString(NumFont, stock.Price.ToString(), position, priceColor);
                position.X += priceBound.X + 5;
                batch.DrawString(NumFont, Math.Abs(stock.Change).ToString(), position, changeColor);

                currentStock++;
                if (currentStock == Stocks.Length) currentStock = 0;
                x += ticklength;
            } while (x < Resolution.X);

            batch.End();
        }
    }
}
