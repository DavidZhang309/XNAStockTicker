using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;


namespace XNAStockTicker
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        const string CFGFILE = "init.cfg";

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        StockDisplay display;
        StockUpdater updater;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this)
                {
                    PreferredBackBufferWidth = 1366,
                    PreferredBackBufferHeight = 100
                };
            Content.RootDirectory = "Content";

            if (!File.Exists(CFGFILE))
            {
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.ShowDialog();
                File.WriteAllText(CFGFILE, dialog.FileName);
            }

            display = new StockDisplay() { Resolution = new Point(1366, 100) };
            display.Stocks = Stock.LoadStocks(File.ReadAllText(CFGFILE));
            display.StockGone += new EventHandler<EventArgs>(display_StockGone);

            updater = new StockUpdater();
            updater.Stocks = display.Stocks;
            updater.Initialize();
        }

        void display_StockGone(object sender, EventArgs e)
        {
            updater.NextRequest();
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            display.SymFont = Content.Load<SpriteFont>("TickerSymbol");
            display.NumFont = Content.Load<SpriteFont>("TickerNum");
            Console.WriteLine(display.SymFont.MeasureString("W").Y.ToString());
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            
            // TODO: Add your update logic here
            updater.Update();
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            // TODO: Add your drawing code here
            display.Draw(spriteBatch);
            base.Draw(gameTime);
        }
    }
}
