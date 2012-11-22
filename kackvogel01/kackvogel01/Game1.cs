using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace kackvogel01
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Rectangle clientBounds;
        SpriteFont perticles8;

        Point screenSize = new Point(20,15);  // screensize in Tiles
        Point tileSize = new Point(48, 48);   // tilesize in Pixel
        Player player;
        Enemy[] enemies = new Enemy[2];
        Tile[] baseTiles = new Tile[3];
        Map map;
        Physics physic;

        // Spielkram Reibung und Schwerkraft
        Vector2 gravity = new Vector2(0, 0);
        Vector2 friction = new Vector2(1, 0);

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
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
            graphics.PreferredBackBufferWidth = screenSize.X * tileSize.X;
            graphics.PreferredBackBufferHeight = screenSize.Y * tileSize.Y;
            graphics.IsFullScreen = false;
            graphics.ApplyChanges();
            Window.Title = "Kackvogel";
            physic = new Physics();

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

            // TODO: use this.Content to load your game content here
            clientBounds = new Rectangle(0, 0, Window.ClientBounds.Width, Window.ClientBounds.Height);
            
            //Test ausgabe Font
            perticles8 = Content.Load<SpriteFont>(@"Fonts\Pericles8");
            initializeTiles();

            map = new Map(baseTiles, screenSize);
            map.loadMap();

            initializePlayer();
            initializeEnemy();
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
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here

            // Spielkram Reibung, Schwerkraft, Kollision
            player.Speed += gravity;
            if (Keyboard.GetState().IsKeyUp(Keys.Left) && Keyboard.GetState().IsKeyUp(Keys.Right))
            {
                if (player.Speed.X < 0)
                    player.Speed += friction;

                if (player.Speed.X > 0)
                    player.Speed -= friction;
            }

            //if (player.Position.X > map.getTilePosition(23, 0).X)
            //    map.getTile(23, 0).IsAlive = false;
            //else
            //    map.getTile(23, 0).IsAlive = true;

            if (player.Position.X > enemies[1].Position.X)
                enemies[1].IsAlive = false;
            else
                enemies[1].IsAlive = true;
            // Ende Spielkram Reibung, Schwerkraft, Kollision

            player.update(gameTime, clientBounds);
            foreach(Enemy e in enemies)
                e.update(gameTime, clientBounds);
            map.update(gameTime, clientBounds);
            map.synchWithPlayer(player, gameTime, clientBounds);
            map.synchEnemies(enemies, gameTime, clientBounds);
            physic.applyPhysics(map, player, enemies,gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            if (gameTime.IsRunningSlowly)
                GraphicsDevice.Clear(Color.Red);
            else
                GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            map.draw(gameTime, spriteBatch);
            foreach (Enemy e in enemies)
                if(e.IsAlive)
                    e.draw(gameTime, spriteBatch);
            player.draw(gameTime, spriteBatch);

            //Test ausgabe
            //spriteBatch.DrawString(perticles8, player.collisionRect.ToString(), new Vector2(100, 100), Color.White);
            //spriteBatch.DrawString(perticles8, map.WidthInPixel.ToString(), new Vector2(100, 110), Color.White);

            spriteBatch.End();

            base.Draw(gameTime);
        }

        // ##################################################################################
        // Kram zum testen
        private void initializePlayer()
        {
            Texture2D playerTex;
            Vector2 playerPosition;
            Point playerFrameSize;
            Point playerCurrentFrame;
            Point playerSheetSize;
            Vector2 playerSpeed;

            playerTex = Content.Load<Texture2D>(@"Images\figur");
            playerPosition = new Vector2(100, 300);
            playerFrameSize = new Point(40, 80);
            playerCurrentFrame = new Point(0, 0);
            playerSheetSize = new Point(1, 1);
            playerSpeed = new Vector2(0, 0);
            player = new Player(playerTex, playerPosition, playerFrameSize, playerCurrentFrame, playerSheetSize, playerSpeed);
            player.MapWidth = map.WidthInPixel;
            player.ScreenWidth = screenSize.X * tileSize.X;
        }

        private void initializeEnemy()
        {
            Texture2D enemyTex;
            Vector2 enemyPosition;
            Point enemyFrameSize;
            Point enemyCurrentFrame;
            Point enemySheetSize;
            Vector2 enemySpeed;

            enemyTex = Content.Load<Texture2D>(@"Images\gegner");
            enemyPosition = new Vector2(Window.ClientBounds.Width / 2, Window.ClientBounds.Height / 4);
            enemyFrameSize = new Point(40, 48);
            enemyCurrentFrame = new Point(0, 0);
            enemySheetSize = new Point(2, 1);
            enemySpeed = new Vector2(1, 0);
            enemies[0] = new Enemy(enemyTex, enemyPosition, enemyFrameSize, 0, enemyCurrentFrame, enemySheetSize, enemySpeed, 250);

            enemyTex = Content.Load<Texture2D>(@"Images\gegner");
            enemyPosition = new Vector2(Window.ClientBounds.Width / 2, Window.ClientBounds.Height / 2);
            enemyFrameSize = new Point(40, 48);
            enemyCurrentFrame = new Point(0, 0);
            enemySheetSize = new Point(2, 1);
            enemySpeed = new Vector2(2, 0);
            enemies[1] = new Enemy(enemyTex, enemyPosition, enemyFrameSize, 0, enemyCurrentFrame, enemySheetSize, enemySpeed, 500);
        }

        private void initializeTiles()
        {
            // Tiles erstellen
            Texture2D tileTex;
            Vector2 tilePosition;
            Point tileFrameSize;
            Point tileCurrentFrame;
            Point tileSheetSize;
            Vector2 tileSpeed;

            baseTiles[0] = new Tile();

            tileTex = Content.Load<Texture2D>(@"Images\gnd");
            tilePosition = Vector2.Zero;
            tileFrameSize = new Point(tileSize.X, tileSize.Y);
            tileCurrentFrame = new Point(0, 0);
            tileSheetSize = new Point(1, 1);
            tileSpeed = new Vector2(0, 0);
            baseTiles[1] = new Tile(tileTex, tilePosition, tileFrameSize, tileCurrentFrame, tileSheetSize, tileSpeed, 1);

            tileTex = Content.Load<Texture2D>(@"Images\stone");
            baseTiles[2] = new Tile(tileTex, tilePosition, tileFrameSize, tileCurrentFrame, tileSheetSize, tileSpeed, 2);
        }
    }
}
