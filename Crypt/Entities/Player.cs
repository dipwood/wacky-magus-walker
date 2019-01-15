using Crypt.Contexts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Crypt.Entities
{
    class Player : Entity
    {
        private readonly Rectangle[] downFrames;
        private readonly Rectangle[] upFrames;
        private readonly Rectangle[] leftFrames;
        private readonly Rectangle[] rightFrames;

        private bool verticalUp;
        private bool verticalDown;
        private bool horizontalLeft;
        private bool horizontalRight;

        // private Animation totalFrames2;

        public override void Draw(SpriteBatch spriteBatch)
        {
            // Rectangle destinationRectangle = new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y);
            Rectangle destinationRectangle = new Rectangle((int)(position.X - size.X / 2), (int)(position.Y - size.Y / 2), (int)size.X, (int)size.Y);
            spriteBatch.Draw(image, destinationRectangle, currentFrame, Color.White);

            // temp
            spriteBatch.DrawString(Program.Game.Font, "X POSITION: " + position.X, new Vector2(500, 0), Color.White);
            spriteBatch.DrawString(Program.Game.Font, "Y POSITION: " + position.Y, new Vector2(500, 20), Color.White);
        }

        public override void Update(GameTime gameTime, UpdateContext updateContext)
        {

            verticalUp = false;
            verticalDown = false;
            horizontalLeft = false;
            horizontalRight = false;

            currentFrame = new Rectangle(4, 6, 24, 33);
            var timer = (float)gameTime.ElapsedGameTime.TotalSeconds;
            remainingFrameTime = frameTime;

            if (updateContext.keyboardState.IsKeyDown(Keys.Up))
            {
                position.Y -= velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
                AnimateWalking(gameTime, timer, upFrames);
                verticalUp = true;
            }

            if (updateContext.keyboardState.IsKeyDown(Keys.Down))
            {
                position.Y += velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
                AnimateWalking(gameTime, timer, downFrames);
                verticalDown = true;
            }

            if (updateContext.keyboardState.IsKeyDown(Keys.Left))
            {
                position.X -= velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
                AnimateWalking(gameTime, timer, leftFrames);
                horizontalLeft = true;
            }

            if (updateContext.keyboardState.IsKeyDown(Keys.Right))
            {
                position.X += velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
                AnimateWalking(gameTime, timer, rightFrames);
                horizontalRight = true;
            }

            position.X = Math.Min(Math.Max(0, this.position.X), updateContext.screenWidth - this.size.X);
            position.Y = Math.Min(Math.Max(0, this.position.Y), updateContext.screenHeight - this.size.Y);

            foreach (Entity entity in updateContext.entityList.ToArray())
            {
                if ("background-impassable".Equals(entity.name) || "interactable".Equals(entity.name))
                {
                    List<Vector2> playerPointList = new List<Vector2>
                    {
                        new Vector2(position.X - size.X / 2, position.Y - size.Y / 2),
                        new Vector2(position.X - size.X / 2, position.Y + size.Y / 2),
                        new Vector2(position.X + size.X / 2, position.Y + size.Y / 2),
                        new Vector2(position.X + size.X / 2, position.Y - size.Y / 2)
                    };
                    Polygon playerPolygon = new Polygon(playerPointList);

                    List<Vector2> entityPointList = new List<Vector2>
                    {
                        new Vector2(entity.entityPosition.X - entity.size.X / 2, entity.entityPosition.Y - entity.size.Y / 2),
                        new Vector2(entity.entityPosition.X - entity.size.X / 2, entity.entityPosition.Y + entity.size.Y / 2),
                        new Vector2(entity.entityPosition.X + entity.size.X / 2, entity.entityPosition.Y + entity.size.Y / 2),
                        new Vector2(entity.entityPosition.X + entity.size.X / 2, entity.entityPosition.Y - entity.size.Y / 2)
                    };
                    Polygon entityPolygon = new Polygon(entityPointList);

                    var collisionResult = Intersector.Intersect(playerPolygon, entityPolygon);

                    if (collisionResult.Intersect)
                    {
                        position += collisionResult.MinimumTranslationVector;
                        entity.interactable = true;
                    }
                }
            }
        }

        private void AnimateWalking(GameTime gameTime, float timer, Rectangle[] totalFrames)
        {
            remainingFrameTime -= timer;
            currentFrame = totalFrames[currentFrameCount];

            _remainingDelay -= timer;

            if (_remainingDelay <= 0)
            {
                currentFrameCount++;
                _remainingDelay += _delay;
            }


            if (currentFrameCount > (totalFrames.Length - 1))
            {
                currentFrameCount = 0;
            }
        }

        public Player()
        {
            name = "player";
            velocity = 100f;
            size = new Vector2(48, 66);
            layer = 1;

            downFrames = new Rectangle[]
            {
                new Rectangle(4, 51, 24, 33),
                new Rectangle(135, 52, 24, 33),
                new Rectangle(56, 51, 24, 33),
                new Rectangle(83, 52, 24, 33),
                new Rectangle(30, 51, 24, 33),
                new Rectangle(109, 52, 24, 33),
            };

            upFrames = new Rectangle[]
            {
                new Rectangle(4, 131, 24, 33),
                new Rectangle(135, 131, 24, 33),
                new Rectangle(56, 131, 24, 33),
                new Rectangle(83, 131, 24, 33),
                new Rectangle(30, 131, 24, 33),
                new Rectangle(109, 131, 24, 33),
            };

            leftFrames = new Rectangle[]
            {
                new Rectangle(5, 91, 24, 33),
                new Rectangle(26, 91, 24, 33),
                new Rectangle(51, 91, 24, 33),
                new Rectangle(75, 91, 24, 33),
                new Rectangle(99, 91, 24, 33),
                new Rectangle(121, 91, 24, 33),
            };

            rightFrames = new Rectangle[]
            {
                new Rectangle(267, 220, 24, 33),
                new Rectangle(246, 220, 24, 33),
                new Rectangle(220, 220, 24, 33),
                new Rectangle(196, 220, 24, 33),
                new Rectangle(173, 220, 24, 33),
                new Rectangle(150, 220, 24, 33),
            };
        }
    }
}
