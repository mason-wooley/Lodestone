using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

namespace Lodestone {
    class Player {
        public Vector2 Position;
        public Texture2D Texture;
        public Rectangle FramePosition;
        public int FrameCount;
        public float FrameTime;

        // These can also be instantiated in the constructor
        private int activeFrame = 0;
        private float timer = 0.0f;
        private float speed = 2.0f;
        private Vector2 velocity = Vector2.Zero;

        public Player(Vector2 position, Texture2D texture, Rectangle frame, int frameCount, float frameTime) {
            Position = position;
            Texture = texture;
            FramePosition = frame;
            FrameCount = frameCount;
            FrameTime = frameTime;
        }

        public void Update(GameTime gameTime, KeyboardState currKeys, KeyboardState prevKeys) {
            #region Animation
            // Count the number of elapsed seconds between each call
            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Loop the animation from start
            if (activeFrame >= FrameCount) {
                activeFrame = 0;
            }

            // Update the activeFrame after the elapsed time has passed the frame animation time
            if (timer >= FrameTime) {
                activeFrame++;

                // Then reset the timer
                timer = 0.0f;
            }

            FramePosition.X = FramePosition.Width * activeFrame;
            #endregion

            #region Input
            if (currKeys.IsKeyDown(Keys.Right)) {
                velocity.X = 1;
            }
            else if (currKeys.IsKeyDown(Keys.Left)) {
                velocity.X = -1;
            }
            else {
                velocity.X = 0;
            }

            if (currKeys.IsKeyDown(Keys.Down)) {
                velocity.Y = 1;
            } else if (currKeys.IsKeyDown(Keys.Up)) {
                velocity.Y = -1;
            } else {
                velocity.Y = 0;
            }

            // Only normalize the vector if it has magnitude, otherwise it divides by zero
            if (velocity.Length() > 0) {
                velocity.Normalize();
            }

            Position += velocity * speed;
            #endregion
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
            spriteBatch.Draw(Texture, Position, FramePosition, Color.White);
        }
    }
}
