using Crypt.Contexts;
using Crypt.Entities;
using Crypt.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;

namespace Crypt
{
    public class CryptGame : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        // content
        List<Entity> entityList;
        Dictionary<string, Texture2D> textureMap;
        Dictionary<string, SoundEffect> soundEffects;
        Dictionary<string, Message> messages;
        // SpriteFont font;
        public SpriteFont Font { get; private set; }

        DialogueBox dialogueBox;

        // everything an entity would need for updating itself
        UpdateContext updateContext;

        // border
        float screenWidth;
        float screenHeight;

        bool firstUpdate = true;

        public CryptGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            Window.Title = "Prototype Game 1";

            screenWidth = (float)graphics.PreferredBackBufferWidth;
            screenHeight = (float)graphics.PreferredBackBufferHeight;

            graphics.GraphicsDevice.BlendState = BlendState.AlphaBlend;

            // most of the content in these objects (ideally)
            entityList = new List<Entity>();
            updateContext = new UpdateContext();
            messages = new Dictionary<string, Message>();
            soundEffects = new Dictionary<string, SoundEffect>();
            textureMap = new Dictionary<string, Texture2D>();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // textures
            textureMap = ContentFactory.TextureFactory(graphics);

            // sound effects
            soundEffects.Add("chest-opened", ContentFactory.LoadSound(@"..\..\..\..\Content\soundeffects\65 Treasure Chest Opening.wav", graphics));

            // songs
            // make a song map

            // fonts
            Font = Content.Load<SpriteFont>("font");

            // dialogue box
            dialogueBox = new DialogueBox(graphics)
            {
                Text = "Hello World! Press Enter or Button A to proceed.\n" +
                       "I will be on the next pane! " +
                       "And wordwrap will occur, especially if there are some longer words!\n" +
                       "Monospace fonts work best but you might not want Courier New.\n" +
                       "In this code sample, after this dialog box finishes, you can press the O key to open a new one."
            };
            dialogueBox.Initialize();
            updateContext.textureMap = textureMap;
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (firstUpdate)
            // put this into its own method later if it becomes a lot
            {
                entityList.Add(new Player()
                {
                    image = textureMap["magus_1_transparent_walk.png"],
                    position = new Vector2(450, 200),
                    layer = 2
                });

                entityList.Add(new Background()
                {
                    image = textureMap["cell_1.png"],
                    position = new Vector2(300, 0),
                    size = new Vector2(147, 159),
                    layer = 1
                });

                // top wall wall
                entityList.Add(new BackgroundImpassable()
                {
                    image = textureMap["cell_1.png"],
                    position = new Vector2(300, 0),
                    entityPosition = new Vector2(447, 44),
                    size = new Vector2(294, 88),
                    currentFrame = new Rectangle(0, 0, 147, 45),
                    layer = 0
                });

                // bottom wall
                entityList.Add(new BackgroundImpassable()
                {
                    image = textureMap["cell_1.png"],
                    position = new Vector2(300, 292),
                    entityPosition = new Vector2(447, 303),
                    size = new Vector2(294, 22),
                    currentFrame = new Rectangle(0, 146, 147, 13),
                    layer = 0
                });

                // left wall
                entityList.Add(new BackgroundImpassable()
                {
                    image = textureMap["cell_1.png"],
                    position = new Vector2(300, 0),
                    entityPosition = new Vector2(309, 159),
                    size = new Vector2(18, 318),
                    currentFrame = new Rectangle(0, 0, 9, 159),
                    layer = 0
                });

                // right wall
                entityList.Add(new BackgroundImpassable()
                {
                    image = textureMap["cell_1.png"],
                    position = new Vector2(576, 0),
                    entityPosition = new Vector2(585, 159),
                    entitySize = new Vector2(9, 159),
                    size = new Vector2(18, 318),
                    currentFrame = new Rectangle(138, 0, 9, 159),
                    layer = 0
                });

                entityList.Add(new Interactable()
                {
                    image = textureMap["chest_closed.png"],
                    position = new Vector2(560, 146),
                    entityPosition = new Vector2(560, 146),
                    size = new Vector2(32, 32),
                    currentFrame = new Rectangle(0, 0, 16, 16),
                    layer = 1,
                    interactable = false
                });

                firstUpdate = false;
            }

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            this.GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;
            this.GraphicsDevice.BlendState = BlendState.AlphaBlend;

            updateContext.keyboardState = Keyboard.GetState();
            updateContext.entityList = entityList;
            updateContext.soundEffects = soundEffects;
            updateContext.screenWidth = screenWidth;
            updateContext.screenHeight = screenHeight;

            foreach (Entity entity in entityList.ToArray())
            {
                entity.Update(gameTime, updateContext);
            }

            // Update the dialog box (essentially, process key/button input)
            dialogueBox.Update(gameTime, updateContext);

            updateContext.previousKeyboardState = updateContext.keyboardState;
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // spriteBatch.Begin();
            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp);

            // consider: Animations["walk_right"]=new Rectangle[4]{new Rectangle(0,0,10,10), ... } // pseudo code

            //for (int layerNumber = 0; layerNumber < 2; layerNumber++)
            //{
            //    foreach (Entity entity in entityList)
            //    {
            //        // for (int layerNumber = 0; layerNumber < 2; layerNumber++)
            //        // {
            //        if (entity.layer.Equals(layerNumber))
            //        {
            //            entity.Draw(spriteBatch);
            //        }

            //    }
            //}

            foreach (var entity in entityList.OrderBy(v => v.layer))
            {
                entity.Draw(spriteBatch);
            }

            dialogueBox.Draw(spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
