using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crypt.Contexts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Crypt.Entities
{
    class Interactable : Entity
    {
        public Interactable()
        {
            name = "interactable";
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Rectangle destinationRectangle = new Rectangle((int)(entityPosition.X - size.X / 2), (int)(entityPosition.Y - size.Y / 2), (int)size.X, (int)size.Y);
            spriteBatch.Draw(image, destinationRectangle, currentFrame, Color.White);
        }

        public override void Update(GameTime gameTime, UpdateContext updateContext)
        {
            if ((interactable && updateContext.keyboardState.IsKeyDown(Keys.A) && updateContext.previousKeyboardState.IsKeyUp(Keys.A)))
            {
                image = updateContext.textureMap["chest_opened.png"];
                interactable = false;
                var chestOpened = updateContext.soundEffects["chest-opened"].CreateInstance();
                chestOpened.IsLooped = false;
                chestOpened.Pitch = 0.5f;
                chestOpened.Play();
            }
        }
    }
}
