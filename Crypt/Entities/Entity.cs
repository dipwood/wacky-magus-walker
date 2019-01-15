using Crypt.Contexts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Crypt.Entities
{
    abstract class Entity
    {
        public Vector2 position;
        public Rectangle positionRectangle;
        public Vector2 size;
        public Texture2D image;
        public Rectangle currentFrame;
        public int currentFrameCount = 0;
        public Rectangle[] totalFrames;
        public float frameTime = 0.5f;
        public float remainingFrameTime;
        public Animation currentAnimation;
        public bool isVisible;
        public Vector2 entityPosition;
        public Vector2 entitySize;
        public bool interactable;

        public const double _delay = 0.175;
        public double _remainingDelay = _delay;

        public String name;
        public float velocity;
        public int layer;

        public abstract void Update(GameTime gameTime, UpdateContext updateContext);

        public abstract void Draw(SpriteBatch spriteBatch);
    }
}
