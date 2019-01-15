using Crypt.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Crypt.Contexts
{
    class DialogueBox : Entity
    {
        /// <summary>
        /// Process input for dialog box
        /// </summary>
        public override void Update(GameTime gameTime, UpdateContext updateContext)
        {
            if (Active)
            {

                // Button press will proceed to the next page of the dialog box
                if ((updateContext.keyboardState.IsKeyDown(Keys.Enter) && updateContext.previousKeyboardState.IsKeyUp(Keys.Enter)))
                {
                    if (!(_currentPage >= _pages.Count - 1))
                    {
                        _currentPage++;
                    }

                    else
                    {
                        Hide();
                    }
                }

                // Shortcut button to skip entire dialog box
                if ((updateContext.keyboardState.IsKeyDown(Keys.X) && updateContext.previousKeyboardState.IsKeyUp(Keys.X)))
                {
                    Hide();
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (Active)
            {
                foreach (var side in BorderRectangles)
                {
                    // spriteBatch.Draw(_borderTexture, null, side);
                    spriteBatch.Draw(_borderTexture, side, BorderColor);

                    // Draw background fill texture (in this example, it's 50% transparent white)
                    // spriteBatch.Draw(_fillTexture, null, TextRectangle);
                    spriteBatch.Draw(_fillTexture, TextRectangle, FillColor);

                    // Draw the current page onto the dialog box
                    // spriteBatch.DrawString(Program.Game.DialogFont, _pages[_currentPage], TextPosition, DialogColor);
                    // spriteBatch.DrawString(CryptGame.Font, "orzo... fuk u", TextPosition, DialogColor);
                    spriteBatch.DrawString(Program.Game.Font, _pages[_currentPage], TextPosition, DialogColor);
                }
            }
        }

        public DialogueBox(GraphicsDeviceManager graphics)
        {
            BorderWidth = 2;

            // color inside the border
            FillColor = Color.DimGray;

            // color of text
            DialogColor = Color.LightGray;

            // color of border
            BorderColor = Color.DarkGray;

            _fillTexture = new Texture2D(graphics.GraphicsDevice, 1, 1);
            _fillTexture.SetData(new[] { FillColor });

            _borderTexture = new Texture2D(graphics.GraphicsDevice, 1, 1);
            _borderTexture.SetData(new[] { BorderColor });

            _pages = new List<string>();
            _currentPage = 0;

            var sizeX = (int)(graphics.GraphicsDevice.Viewport.Width * 0.8);
            var sizeY = (int)(graphics.GraphicsDevice.Viewport.Height * 0.2);

            Size = new Vector2(sizeX, sizeY);

            // var center = (float)new Vector2((float)graphics.PreferredBackBufferWidth / 2, (float)graphics.PreferredBackBufferHeight / 2));
            var posX = CenterScreen(graphics).X - (Size.X / 2f);
            var posY = graphics.GraphicsDevice.Viewport.Height - Size.Y - 30;

            Position = new Vector2(posX, posY);
        }

        /// <summary>
        /// Initialize a dialog box
        /// - can be used to reset the current dialog box in case of "I didn't quite get that..."
        /// </summary>
        /// <param name="text"></param>
        public void Initialize(string text = null)
        {
            Text = text ?? Text;

            _currentPage = 0;

            _pages = WordWrap(Text);

            Show();
        }

        /// <summary>
        /// Show the dialog box on screen
        /// - invoke this method manually if Text changes
        /// </summary>
        public void Show()
        {
            Active = true;

            // use stopwatch to manage blinking indicator
            // _stopwatch = new Stopwatch();

            // _stopwatch.Start();

            // _pages = WordWrap(Text);
        }

        /// <summary>
        /// Manually hide the dialog box
        /// </summary>
        public void Hide()
        {
            Active = false;

            // _stopwatch.Stop();

            // _stopwatch = null;
        }

        /// <summary>
        /// The position and size of the bordering sides on the edges of the dialog box
        /// </summary>
        private List<Rectangle> BorderRectangles => new List<Rectangle>
        {
            // Top (contains top-left & top-right corners)
            new Rectangle(TextRectangle.X - BorderWidth, TextRectangle.Y - BorderWidth,
                TextRectangle.Width + BorderWidth*2, BorderWidth),

            // Right
            new Rectangle(TextRectangle.X + TextRectangle.Size.X, TextRectangle.Y, BorderWidth, TextRectangle.Height),

            // Bottom (contains bottom-left & bottom-right corners)
            new Rectangle(TextRectangle.X - BorderWidth, TextRectangle.Y + TextRectangle.Size.Y,
                TextRectangle.Width + BorderWidth*2, BorderWidth),

            // Left
            new Rectangle(TextRectangle.X - BorderWidth, TextRectangle.Y, BorderWidth, TextRectangle.Height)
        };

        /// <summary>
        /// Wrap words to the next line where applicable
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private List<string> WordWrap(string text)
        {
            var pages = new List<string>();

            var capacity = MaxCharsPerLine * MaxLines > text.Length ? text.Length : MaxCharsPerLine * MaxLines;

            var result = new StringBuilder(capacity);
            var resultLines = 0;

            var currentWord = new StringBuilder();
            var currentLine = new StringBuilder();

            for (var i = 0; i < text.Length; i++)
            {
                var currentChar = text[i];
                var isNewLine = text[i] == '\n';
                var isLastChar = i == text.Length - 1;

                currentWord.Append(currentChar);

                if (char.IsWhiteSpace(currentChar) || isLastChar)
                {
                    var potentialLength = currentLine.Length + currentWord.Length;

                    if (potentialLength > MaxCharsPerLine)
                    {
                        result.AppendLine(currentLine.ToString());

                        currentLine.Clear();

                        resultLines++;
                    }

                    currentLine.Append(currentWord);

                    currentWord.Clear();

                    if (isLastChar || isNewLine)
                    {
                        result.AppendLine(currentLine.ToString());
                    }

                    if (resultLines > MaxLines || isLastChar || isNewLine)
                    {
                        pages.Add(result.ToString());

                        result.Clear();

                        resultLines = 0;

                        if (isNewLine)
                        {
                            currentLine.Clear();
                        }
                    }
                }
            }

            return pages;
        }

        /// <summary>
        /// The position and size of the dialog box fill rectangle
        /// </summary>
        private Rectangle TextRectangle => new Rectangle(Position.ToPoint(), Size.ToPoint());

        /// <summary>
        /// Thickness of border
        /// </summary>
        public int BorderWidth { get; set; }

        /// <summary>
        /// Color used for border around dialog box
        /// </summary>
        public Color BorderColor { get; set; }

        /// <summary>
        /// X,Y coordinates of this dialog box
        /// </summary>
        public Vector2 Position { get; set; }

        /// <summary>
        /// Width and Height of this dialog box
        /// </summary>
        public Vector2 Size { get; set; }

        /// <summary>
        /// Border fill texture (built from BorderColor)
        /// </summary>
        private readonly Texture2D _borderTexture;

        // Shortcut for finding center point of screen
        public Vector2 CenterScreen(GraphicsDeviceManager graphics)
            => new Vector2(graphics.GraphicsDevice.Viewport.Width / 2f, graphics.GraphicsDevice.Viewport.Height / 2f);

        /// <summary>
        /// Background fill texture (built from FillColor)
        /// </summary>
        private readonly Texture2D _fillTexture;

        /// <summary>
        /// Color used to fill dialog box background
        /// </summary>
        public Color FillColor { get; set; }

        /// <summary>
        /// The starting position of the text inside the dialog box
        /// </summary>
        private Vector2 TextPosition => new Vector2(Position.X + DialogBoxMargin / 2, Position.Y + DialogBoxMargin / 2);

        /// <summary>
        /// Color used for text in dialog box
        /// </summary>
        public Color DialogColor { get; set; }

        /// <summary>
        /// Margin surrounding the text inside the dialog box
        /// </summary>
        private const float DialogBoxMargin = 24f;

        /// <summary>
        /// Collection of pages contained in this dialog box
        /// </summary>
        private List<string> _pages;

        /// <summary>
        /// The index of the current page
        /// </summary>
        private int _currentPage;

        /// <summary>
        /// All text contained in this dialog box
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// The amount of characters allowed on a given line
        /// NOTE: If you want to use a font that is not monospaced, this will need to be reevaluated
        /// </summary>
        private int MaxCharsPerLine => (int)Math.Floor((Size.X - DialogBoxMargin) / (_characterSize.X / 2));

        /// <summary>
        /// Determine the maximum amount of lines allowed per page
        /// NOTE: This will change automatically with font size
        /// </summary>
        private int MaxLines => (int)Math.Floor((Size.Y - DialogBoxMargin) / (_characterSize.Y / 2)) - 1;

        /// <summary>
        /// Size (in pixels) of a wide alphabet letter (W is the widest letter in almost every font) 
        /// </summary>
        private Vector2 _characterSize = Program.Game.Font.MeasureString(new StringBuilder("W", 1));

        /// <summary>
        /// Bool that determines active state of this dialog box
        /// </summary>
        public bool Active { get; private set; }
    }
}
