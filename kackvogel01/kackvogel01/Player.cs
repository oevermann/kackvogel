using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace kackvogel01
{
    class Player : Sprite
    {
        #region Members
        const int MAX_SPEED_RUN = 2;
        const int MAX_SPEED_JUMP = 10;
        const int MAX_SPEED_FALL = 50;
        const int SPEEDSTEP_RUN = 1;

        bool isOnGround = true;
        int score = 0;

        int mapWidth;
        int screenWidth;
        #endregion

        #region Constructors
        public Player(Texture2D textureImage, Vector2 position, Point frameSize,
            Point currentFrame, Point sheetSize, Vector2 speed)
            : base(textureImage, position, frameSize, currentFrame, sheetSize, speed)
        {
        }

        public Player(Texture2D textureImage, Vector2 position, Point frameSize,
            int collisionOffset, Point currentFrame, Point sheetSize, Vector2 speed)
            : base(textureImage, position, frameSize, collisionOffset, currentFrame, sheetSize, speed)
        {
        }

        public Player(Texture2D textureImage, Vector2 position, Point frameSize,
            int collisionOffset, Point currentFrame, Point sheetSize, Vector2 speed, int millisecondsPerFrame)
            : base(textureImage, position, frameSize, collisionOffset, currentFrame, sheetSize, speed, millisecondsPerFrame)
        {
        }
        #endregion

        public override void update(GameTime gameTime, Rectangle clientBounds)
        {
            
            KeyboardState keybState = Keyboard.GetState();

            // Tastaturabfrage
            if (keybState.IsKeyDown(Keys.Left))
                this.Speed += new Vector2(-SPEEDSTEP_RUN, 0);
            else if (keybState.IsKeyDown(Keys.Right))
                this.Speed += new Vector2(SPEEDSTEP_RUN, 0);
            if (keybState.IsKeyDown(Keys.Up) && isOnGround)
                this.Speed += new Vector2(0, -MAX_SPEED_JUMP);

            if (this.speed.X > MAX_SPEED_RUN)
                this.speed.X = MAX_SPEED_RUN;
            if (this.speed.X < -MAX_SPEED_RUN)
                this.speed.X = -MAX_SPEED_RUN;
            if (this.speed.Y < -MAX_SPEED_JUMP)
                this.speed.Y = -MAX_SPEED_RUN;
            if (this.speed.Y > MAX_SPEED_FALL)
                this.speed.Y = MAX_SPEED_FALL;

            positionOffsetX = (int)position.X - clientBounds.Width / 2;
            if (positionOffsetX < 0)
                positionOffsetX = 0;
            if (positionOffsetX > mapWidth - screenWidth)
                positionOffsetX = mapWidth - screenWidth;

            //base.update(gameTime, clientBounds);
        }

        public override void draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(textureImage,
                position - new Vector2(positionOffsetX, 0),
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

        #region Properties
        public bool IsOnGround
        {
            get
            {
                return isOnGround;
            }
            set
            {
                isOnGround = value;
            }
        }

        public int Score
        {
            get
            {
                return score;
            }
            set
            {
                score = value;
            }
        }

        public int MapWidth
        {
            set
            {
                mapWidth = value;
            }
        }

        public int ScreenWidth
        {
            set
            {
                screenWidth = value;
            }
        }
        #endregion
    }
}
