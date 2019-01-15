using Crypt.Entities;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Crypt.Contexts
{
    class UpdateContext
    {
        public KeyboardState keyboardState;
        public KeyboardState previousKeyboardState;
        public List<Entity> entityList;
        public float screenWidth;
        public float screenHeight;
        public Dictionary<string, SoundEffect> soundEffects;
        internal Dictionary<string, Texture2D> textureMap;
    }
}