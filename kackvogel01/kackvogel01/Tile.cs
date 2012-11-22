using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace kackvogel01
{
    class Tile : Sprite
    {
        #region Members
        bool isBackground;
        bool isDestructible;
        bool isEatable;
        int type;
        #endregion

        #region Constructors
        public Tile()
        {
            isBackground = true;
            isDestructible = false;
            isEatable = false;
            type = 0;
        }

        public Tile(Tile tile)
            : base(tile.TextureImage, tile.Position, tile.FrameSize, tile.CollisionOffset,
            tile.CurrentFrame, tile.SheetSize, tile.Speed, tile.MillisecondsPerFrame)
        {
            isBackground = tile.isBackground;
            isDestructible = tile.isDestructible;
            isEatable = tile.isEatable;
            IsAlive = tile.IsAlive;
            type = 0;
        }

        public Tile(Texture2D textureImage, Vector2 position, Point frameSize,
            Point currentFrame, Point sheetSize, Vector2 speed, int type)
            : base(textureImage, position, frameSize, currentFrame, sheetSize, speed)
        {
            this.type = type;
        }

        public Tile(Texture2D textureImage, Vector2 position, Point frameSize,
            int collisionOffset, Point currentFrame, Point sheetSize, Vector2 speed, int type)
            : base(textureImage, position, frameSize, collisionOffset, currentFrame, sheetSize, speed)
        {
            this.type = type;
        }

        public Tile(Texture2D textureImage, Vector2 position, Point frameSize,
            int collisionOffset, Point currentFrame, Point sheetSize, Vector2 speed, int millisecondsPerFrame, int type)
            : base(textureImage, position, frameSize, collisionOffset, currentFrame, sheetSize, speed, millisecondsPerFrame)
        {
            this.type = type;
        }
        #endregion

        #region Properties
        public bool IsBackground
        {
            get
            {
                return isBackground;
            }
            set
            {
                isBackground = value;
            }
        }

        public bool IsDestructible
        {
            get
            {
                return isDestructible;
            }
            set
            {
                isDestructible = value;
            }
        }

        public bool IsEatable
        {
            get
            {
                return isEatable;
            }
            set
            {
                isEatable = value;
            }
        }

        public int TileWidth
        {
            get
            {
                return base.frameSize.X;
            }
        }

        public int TileHeight
        {
            get
            {
                return base.frameSize.Y;
            }
        }

        public int Type
        {
            get
            {
                return type;
            }
            set
            {
                type = value;
            }
        }
        #endregion
    }
}
