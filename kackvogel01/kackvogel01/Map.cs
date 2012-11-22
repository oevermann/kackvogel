using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace kackvogel01
{
    class Map
    {
        #region Members
        Tile[,] tileArray;
        Tile[] baseTiles;
        int[,] mapArray;
        Vector2 mapOrigin;
        Vector2 mapOffset;

        Point sizeInTiles;
        Point sizeInPixel;
        Point sizeOfTile;
        Point tilesOnScreen;
        #endregion

        #region Constructors
        public Map(Tile[] baseTiles, Point tilesOnScreen)
        {
            this.baseTiles = baseTiles;
            this.tilesOnScreen = tilesOnScreen;

            sizeOfTile = new Point(baseTiles[1].TileWidth, baseTiles[1].TileHeight);
            mapOrigin = Vector2.Zero;
            mapOffset = Vector2.Zero;
        }
        #endregion

        #region Methods
        public Tile getTile(int x, int y)
        {
            return tileArray[x, y];
        }

        public Vector2 getTilePosition(int x, int y)
        {
            return tileArray[x, y].Position - mapOffset;
        }

        public Rectangle getTileCollisionRect(int x, int y)
        {
            return new Rectangle(tileArray[x, y].collisionRect.X - (int)mapOffset.X,
                                 tileArray[x, y].collisionRect.Y - (int)mapOffset.X,
                                 tileArray[x, y].collisionRect.Width,
                                 tileArray[x, y].collisionRect.Height);
        }

        public void update(GameTime gameTime, Rectangle clientBounds)
        {
            if (mapOffset.X > 0)
                mapOffset.X = 0;

            if (mapOffset.X < -(sizeInPixel.X - clientBounds.Width))
                mapOffset.X = -(sizeInPixel.X - clientBounds.Width);

            // update tiles
            for (int x = 0; x < sizeInTiles.X; ++x)
                for (int y = 0; y < sizeInTiles.Y; ++y)
                    if (!tileArray[x, y].IsBackground)
                    {
                        tileArray[x, y].update(gameTime, clientBounds);
                        tileArray[x, y].Position = new Vector2((int)(x * sizeOfTile.X + mapOffset.X),
                                                               (int)(y * sizeOfTile.Y + mapOffset.Y));
                    }
        }

        public void synchWithPlayer(Player player, GameTime gameTime, Rectangle clientBounds)
        {
            mapOffset.X = -player.PositionOffsetX;
        }

        public void synchEnemies(Enemy[] enemies, GameTime gameTime, Rectangle clientBounds)
        {
            foreach (Enemy e in enemies)
                e.PositionOffsetX = -(int)mapOffset.X;
        }

        public virtual void draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            for (int x = 0; x < sizeInTiles.X; ++x)
                for (int y = 0; y < sizeInTiles.Y; ++y)
                    if (tileArray[x, y].IsOnScreen && tileArray[x, y].IsAlive && !tileArray[x, y].IsBackground)
                        tileArray[x, y].draw(gameTime, spriteBatch);
        }

        public Point getStartIndex(Vector2 position)
        {
            return new Point(0, 0);
        }

        public Point getEndIndex(Vector2 position)
        {
            return new Point(sizeInTiles.X, sizeInTiles.Y);
        }


        // ##################### temporäre Karte #################################
        public void loadMap()
        {
            mapArray = generateMap();

            sizeInTiles = new Point(mapArray.GetLength(0), mapArray.GetLength(1));
            sizeInPixel = new Point(sizeInTiles.X * sizeOfTile.X, sizeInTiles.Y * sizeOfTile.Y);

            tileArray = new Tile[sizeInTiles.X, sizeInTiles.Y];
            for (int x = 0; x < sizeInTiles.X; ++x)
                for (int y = 0; y < sizeInTiles.Y; ++y)
                    tileArray[x, y] = new Tile();

            for (int x = 0; x < sizeInTiles.X; ++x)
                for (int y = 0; y < sizeInTiles.Y; ++y)
                    if(mapArray[x,y] != 0)
                        tileArray[x, y] = new Tile(baseTiles[mapArray[x,y]]);
        }

        public int[,] generateMap()
        {
            int[,] tempmap = new int[40, 15];
            for (int x = 0; x < 40; ++x)
                for (int y = 0; y < 15; ++y)
                    tempmap[x, y] = 0;

            for (int x = 0; x < 40; ++x)
                tempmap[x, 14] = 1;

            for (int x = 0; x < 40; ++x)
                tempmap[x, 0] = 1;

            for (int y = 0; y < 15; ++y)
                tempmap[0, y] = 1;

            for (int y = 0; y < 15; ++y)
                tempmap[39, y] = 1;

            for (int x = 1; x < 39; ++x)
                if (x % 2 == 0 || x % 3 == 0)
                    tempmap[x, 10] = 2;
            return tempmap;
        }
        #endregion

        #region Properties
        public int WidthInTiles
        {
            get
            {
                return sizeInTiles.X;
            }
        }
        public int HeightInTiles
        {
            get
            {
                return sizeInTiles.Y;
            }
        }
        public int WidthInPixel
        {
            get
            {
                return sizeInPixel.X;
            }
        }
        public int HeightInPixel
        {
            get
            {
                return sizeInPixel.Y;
            }
        }

        public int TileWidth
        {
            get
            {
                return sizeOfTile.X;
            }
        }

        public int TileHeight
        {
            get
            {
                return sizeOfTile.Y;
            }
        }
        #endregion
    }
}
