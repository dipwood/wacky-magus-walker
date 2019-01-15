using Microsoft.Xna.Framework;
using System;

namespace Crypt.Contexts
{
    class Message
    {
        public string Text { get; set; }
        public TimeSpan Appeared { get; set; }
        public Vector2 Position { get; set; }
        public Boolean IsActive { get; set; }
    }
}
