using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crypt.Contexts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Crypt.Entities
{
    class Background : Entity
    {
        public override void Draw(SpriteBatch spriteBatch)
        {
            Rectangle destinationRectangle = new Rectangle((int)position.X, (int)position.Y, (int)size.X*2, (int)size.Y*2);
            spriteBatch.Draw(image, destinationRectangle, currentFrame, Color.White);
        }

        public override void Update(GameTime gameTime, UpdateContext updateContext)
        {
            currentFrame = new Rectangle(0, 0, (int)size.X, (int)size.Y);
        }
    }
}
