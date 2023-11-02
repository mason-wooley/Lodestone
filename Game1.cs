using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Lodestone {
    public class Game1 : Game {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        private RenderTarget2D gameWindow;

        private Texture2D bush;
        private Player player;

        private KeyboardState currentKeyboardState;
        private KeyboardState previousKeyboardState;

        public Game1() {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        // Called before any game frames to initialize subsystems
        protected override void Initialize() {
            // Set the size of the screen
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
            graphics.ApplyChanges();

            // Content is drawn to the RenderTarget and then scaled up to the size of the window (x4)
            gameWindow = new RenderTarget2D(GraphicsDevice, 320, 180);

            base.Initialize();
        }

        protected override void LoadContent() {
            bush = Content.Load<Texture2D>("Graphics/World/Bush");
            player = new Player(new Vector2(100, 50), Content.Load<Texture2D>("Graphics/Player/Player"), new Rectangle(0, 0, 64, 64), 60, 0.15f);

            spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void Update(GameTime gameTime) {
            currentKeyboardState = Keyboard.GetState();

            player.Update(gameTime, currentKeyboardState, previousKeyboardState);

            previousKeyboardState = currentKeyboardState;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) {
            // First, draw to the RenderTarget
            GraphicsDevice.SetRenderTarget(gameWindow);
            GraphicsDevice.Clear(new Color(80, 80, 80));
            
            spriteBatch.Begin();
            spriteBatch.Draw(bush, new Vector2(20, 20), Color.White);
            player.Draw(gameTime, spriteBatch);
            spriteBatch.End();

            GraphicsDevice.SetRenderTarget(null);

            // Then draw the RenderTarget to the screen scaled up
            GraphicsDevice.Clear(Color.Black);
            
            // Setting PointClamp here keeps the pixel art from scaling blurry
            spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            spriteBatch.Draw(gameWindow, Vector2.Zero, null, Color.White, 0f, Vector2.Zero, 4, SpriteEffects.None, 0f);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
