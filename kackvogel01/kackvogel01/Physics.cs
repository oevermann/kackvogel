using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace kackvogel01
{
    class Physics
    {

        #region applyPhysic
        public void applyPhysics(Map map, Player player, Enemy[] enemies, GameTime gametime)
        {
            float x = 1.5f;
            player.Speed += new Vector2(0, 0.15f * x);

            

            mapCollisionPlayer(map, player);

            foreach (Enemy e in enemies)
            {
                e.Speed += new Vector2(0, 0.15f * x);
                if (e.IsAlive && e.IsOnScreen)
                {
                    mapCollisionEnemy(map, e);
                }
            }
        }
#endregion

        #region MapCollision
        private void mapCollisionPlayer(Map map, Player player)
        {

            if (player.Speed.Y != 0)
                player.IsOnGround = false;

            Vector2 moveAmount = player.Speed;
            
            //foreach (Tile tile in tileList)
            //{
            //    moveAmount = horizontalCollisionTest(moveAmount, player, tile);
            //    moveAmount = verticalCollisionTest(moveAmount, player, tile);
            //}

            moveAmount = horizontalCollisionTest(moveAmount, player, map);
            moveAmount = verticalCollisionTest(moveAmount, player, map);

            Vector2 newPosition = player.Position + moveAmount;
            
            newPosition = new Vector2(
                MathHelper.Clamp(newPosition.X, 0,
                  map.WidthInPixel - player.collisionRect.Width),
                MathHelper.Clamp(newPosition.Y, 2 * (map.TileHeight),
                  map.HeightInPixel - player.collisionRect.Height));
            player.Position = newPosition;
        }

        private void mapCollisionEnemy(Map map, Enemy enemy)
        {

            Vector2 moveAmount = enemy.Speed;

            //foreach (Tile tile in tileList)
            //{
            //    moveAmount = horizontalCollisionTest(moveAmount, player, tile);
            //    moveAmount = verticalCollisionTest(moveAmount, player, tile);
            //}

            moveAmount = horizontalCollisionTest(moveAmount, enemy, map);
            moveAmount = verticalCollisionTestEnemy(moveAmount, enemy, map);

            Vector2 newPosition = enemy.Position + moveAmount;

            newPosition = new Vector2(
                MathHelper.Clamp(newPosition.X, 0,
                  map.WidthInPixel - enemy.collisionRect.Width),
                MathHelper.Clamp(newPosition.Y, 2 * (map.TileHeight),
                  map.HeightInPixel - enemy.collisionRect.Height));
            enemy.Position = newPosition;
        }
        #endregion

        #region Hilfsfunktionen MapCollision
        private Vector2 horizontalCollisionTest(Vector2 moveAmount, Sprite player, Map map)
        {
            if (moveAmount.X == 0)
                return moveAmount;

            Rectangle afterMoveRect = player.collisionRect;
            afterMoveRect.Offset((int)moveAmount.X, 0);
            Vector2 corner1, corner2;

            if (moveAmount.X < 0)
            {
                corner1 = new Vector2(afterMoveRect.Left,
                                      afterMoveRect.Top + 1);
                corner2 = new Vector2(afterMoveRect.Left,
                                      afterMoveRect.Bottom - 1);
            }
            else
            {
                corner1 = new Vector2(afterMoveRect.Right,
                                      afterMoveRect.Top + 1);
                corner2 = new Vector2(afterMoveRect.Right,
                                      afterMoveRect.Bottom - 1);
            }

            Tile mapCell1 = map.getTile((int)(corner1.X / map.TileHeight), (int)(corner1.Y / map.TileWidth));
            Tile mapCell2 = map.getTile((int)(corner2.X / map.TileHeight), (int)(corner2.Y / map.TileWidth));

            if ((!mapCell1.IsBackground && !mapCell1.IsDestructible) ||
                (!mapCell2.IsBackground && !mapCell2.IsDestructible))
            {
                moveAmount.X = 0;
                player.Speed = new Vector2(0, player.Speed.Y);
            }

            
            return moveAmount;
        }

        private Vector2 verticalCollisionTest(Vector2 moveAmount, Player player, Map map)
        {
            if (moveAmount.Y == 0)
                return moveAmount;

            Rectangle afterMoveRect = player.collisionRect;
            afterMoveRect.Offset((int)moveAmount.X, (int)moveAmount.Y);
            Vector2 corner1, corner2;

            if (moveAmount.Y < 0)
            {
                corner1 = new Vector2(afterMoveRect.Left + 1,
                                      afterMoveRect.Top);
                corner2 = new Vector2(afterMoveRect.Right - 1,
                                      afterMoveRect.Top);
            }
            else
            {
                corner1 = new Vector2(afterMoveRect.Left + 1,
                                      afterMoveRect.Bottom);
                corner2 = new Vector2(afterMoveRect.Right - 1,
                                      afterMoveRect.Bottom);
            }

            Tile mapCell1 = map.getTile((int)(corner1.X / map.TileHeight), (int)(corner1.Y / map.TileWidth));
            Tile mapCell2 = map.getTile((int)(corner2.X / map.TileHeight), (int)(corner2.Y / map.TileWidth));

            if ((!mapCell1.IsBackground && !mapCell1.IsDestructible) ||
                (!mapCell2.IsBackground && !mapCell2.IsDestructible))
            {
                if (moveAmount.Y > 0)
                    player.IsOnGround = true;
                moveAmount.Y = 0;
                player.Speed = new Vector2(player.Speed.X, 0);
            }


            return moveAmount;
        }

        private Vector2 verticalCollisionTestEnemy(Vector2 moveAmount, Enemy player, Map map)
        {
            if (moveAmount.Y == 0)
                return moveAmount;

            Rectangle afterMoveRect = player.collisionRect;
            afterMoveRect.Offset((int)moveAmount.X, (int)moveAmount.Y);
            Vector2 corner1, corner2;

            if (moveAmount.Y < 0)
            {
                corner1 = new Vector2(afterMoveRect.Left + 1,
                                      afterMoveRect.Top);
                corner2 = new Vector2(afterMoveRect.Right - 1,
                                      afterMoveRect.Top);
            }
            else
            {
                corner1 = new Vector2(afterMoveRect.Left + 1,
                                      afterMoveRect.Bottom);
                corner2 = new Vector2(afterMoveRect.Right - 1,
                                      afterMoveRect.Bottom);
            }

            Tile mapCell1 = map.getTile((int)(corner1.X / map.TileHeight), (int)(corner1.Y / map.TileWidth));
            Tile mapCell2 = map.getTile((int)(corner2.X / map.TileHeight), (int)(corner2.Y / map.TileWidth));

            if ((!mapCell1.IsBackground && !mapCell1.IsDestructible) ||
                (!mapCell2.IsBackground && !mapCell2.IsDestructible))
            {
                //if (moveAmount.Y > 0)
                //    player.IsOnGround = true;
                moveAmount.Y = 0;
                player.Speed = new Vector2(player.Speed.X, 0);
            }


            return moveAmount;
        }
        #endregion
    }
}
