using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace kackvogel01
{
    abstract class Sprite
    {
        #region Members
        protected Texture2D textureImage;
        protected Point frameSize;
        protected Point currentFrame;
        protected Point sheetSize;
        int collisionOffset;
        const int defaultCollisionOffset = 0;
        int timeSinceLastFrame = 0;
        int millisecondsPerFrame;
        const int defaultMillisecondsPerFrame = 16;
        public Vector2 speed;
        protected Vector2 position;
        protected int positionOffsetX;
        bool isAlive;
        bool isOnScreen;
        int remainingLives;
        #endregion

        #region Constructors
        // Constructor for Background Tiles, just initializing values
        public Sprite()
        {
            this.textureImage = null;
            this.position = Vector2.Zero;
            this.positionOffsetX = 0;
            this.frameSize = Point.Zero;
            this.collisionOffset = 0;
            this.currentFrame = Point.Zero;
            this.sheetSize = Point.Zero;
            this.speed = Vector2.Zero;
            this.millisecondsPerFrame = defaultMillisecondsPerFrame;
            this.isAlive = true;
            this.isOnScreen = false;
            this.remainingLives = 1;
        }

        public Sprite(Texture2D textureImage, Vector2 position, Point frameSize,
            Point currentFrame, Point sheetSize, Vector2 speed)
            : this(textureImage, position, frameSize, defaultCollisionOffset, currentFrame, sheetSize, speed, defaultMillisecondsPerFrame)
        {
        }

        public Sprite(Texture2D textureImage, Vector2 position, Point frameSize,
            int collisionOffset, Point currentFrame, Point sheetSize, Vector2 speed)
            : this(textureImage, position, frameSize, collisionOffset, currentFrame, sheetSize, speed, defaultMillisecondsPerFrame)
        {
        }

        public Sprite(Texture2D textureImage, Vector2 position, Point frameSize,
            int collisionOffset, Point currentFrame, Point sheetSize, Vector2 speed, int millisecondsPerFrame)
        {
            this.textureImage = textureImage;
            this.position = position;
            this.positionOffsetX = 0;
            this.frameSize = frameSize;
            this.collisionOffset = collisionOffset;
            this.currentFrame = currentFrame;
            this.sheetSize = sheetSize;
            this.speed = speed;
            this.millisecondsPerFrame = millisecondsPerFrame;
            this.isAlive = true;
            this.isOnScreen = false;
            this.remainingLives = 1;
        }
        #endregion

        #region Methods
        public virtual void update(GameTime gameTime, Rectangle clientBounds)
        {
            // animating sprite
            timeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;
            if (timeSinceLastFrame >= millisecondsPerFrame)
            {
                timeSinceLastFrame = 0;
                ++currentFrame.X;
                if (currentFrame.X >= sheetSize.X)
                {
                    currentFrame.X = 0;
                    ++currentFrame.Y;
                    if (currentFrame.Y >= sheetSize.Y)
                        currentFrame.Y = 0;
                }
            }

            // set new position
            position += speed;

            // if Sprite is not on screen, set isOnScreen to false, else true
            if (position.X + frameSize.X < 0)
                isOnScreen = false;
            else if (position.X > clientBounds.Width)
                isOnScreen = false;
            else if (position.Y + frameSize.Y < 0)
                isOnScreen = false;
            else if (position.Y > clientBounds.Height)
                isOnScreen = false;
            else
                isOnScreen = true;
        }

        public virtual void draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(textureImage,
                position,
                new Rectangle(currentFrame.X * frameSize.X,
                    currentFrame.Y * frameSize.Y,
                    frameSize.X, frameSize.Y),
                Color.White,
                0,
                Vector2.Zero,
                1f,
                SpriteEffects.None,
                0);
        }
        #endregion

        #region public Properties
        public Rectangle collisionRect
        {
            get
            {
                return new Rectangle(
                    (int)position.X + collisionOffset,
                    (int)position.Y + collisionOffset,
                    frameSize.X - (collisionOffset * 2),
                    frameSize.Y - (collisionOffset * 2));
            }
        }

        public Vector2 Position
        {
            get
            {
                return position;
            }
            set
            {
                position = value;
            }
        }

        public Vector2 Speed
        {
            get
            {
                return speed;
            }
            set
            {
                speed = value;
            }
        }

        public bool IsAlive
        {
            get
            {
                return isAlive;
            }
            set
            {
                isAlive = value;
            }
        }

        public bool IsOnScreen
        {
            get
            {
                return isOnScreen;
            }
        }

        public int RemainingLives
        {
            get
            {
                return remainingLives;
            }
            set
            {
                remainingLives = value;
            }
        }

        public int PositionOffsetX
        {
            get
            {
                return positionOffsetX;
            }
            set
            {
                positionOffsetX = value;
            }
        }
        #endregion

        #region protected Properties
        protected Texture2D TextureImage
        {
            get
            {
                return textureImage;
            }
        }
        protected Point FrameSize
        {
            get
            {
                return frameSize;
            }
        }
        protected Point SheetSize
        {
            get
            {
                return sheetSize;
            }
        }
        protected Point CurrentFrame
        {
            get
            {
                return currentFrame;
            }
        }
        protected int CollisionOffset
        {
            get
            {
                return collisionOffset;
            }
        }
        protected int MillisecondsPerFrame
        {
            get
            {
                return millisecondsPerFrame;
            }
        }
        #endregion
    }
}
