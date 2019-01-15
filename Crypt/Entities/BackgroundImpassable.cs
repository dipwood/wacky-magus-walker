using Crypt.Contexts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Crypt.Entities
{
    class BackgroundImpassable : Entity
    {
        public BackgroundImpassable()
        {
            name = "background-impassable";
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            // Rectangle destinationRectangle = new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y);
            Rectangle destinationRectangle = new Rectangle((int)(entityPosition.X - size.X / 2), (int)(entityPosition.Y - size.Y / 2), (int)size.X, (int)size.Y);
            spriteBatch.Draw(image, destinationRectangle, currentFrame, Color.White);
        }

        public override void Update(GameTime gameTime, UpdateContext updateContext)
        {
            // location of wall on original image
            // currentFrame = currentFrame;
        }
    }
}
