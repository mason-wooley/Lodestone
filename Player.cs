using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;

namespace Lodestone {
    class Player {
        public Vector2 Position;
        public Texture2D Texture;
        public Rectangle FramePosition;
        public int FrameCount;
        public float FrameTime;
        public float MaxSpeed; // pixels per second
        public float Acceleration; // pixels per second

        // These can also be instantiated in the constructor
        private int activeFrame = 18;
        private float timer = 0.0f;
        private Vector2 velocity = Vector2.Zero;
        private float friction = 80f;

        public Player(Vector2 position, Texture2D texture, Rectangle frame, int frameCount, float frameTime, float maxSpeed, float acceleration) {
            Position = position;
            Texture = texture;
            FramePosition = frame;
            FrameCount = frameCount;
            FrameTime = frameTime;
            MaxSpeed = maxSpeed;
            Acceleration = acceleration;
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
                // activeFrame++;

                // Then reset the timer
                timer = 0.0f;
            }

            FramePosition.X = FramePosition.Width * activeFrame;
            #endregion

            #region Input
            var frameSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Gets the input direction from arrow keys; 8 movement directions
            Vector2 inputDirection = GetInputDirection(currKeys);

            // If the player is requesting to move, apply acceleration
            if (inputDirection.Length() > 0) {
                float dotProduct = Vector2.Dot(Vector2.Normalize(inputDirection), Vector2.Normalize(velocity));

                // If trying to move in the opposite of current travel direction, let friction slow down player
                if (dotProduct < 0) {
                    velocity += inputDirection * friction * frameSeconds;
                }
                // Normal acceleration
                else {
                    velocity += inputDirection * Acceleration * frameSeconds;
                }

                // If the speed is over max, normalize it to max speed
                if (velocity.Length() >= (MaxSpeed * frameSeconds)) {
                    velocity = Vector2.Normalize(velocity) * MaxSpeed * frameSeconds;
                }
            }
            // If player is attempting to stop and has more than friction amount left to slow down
            else  if (velocity.Length() > (friction * frameSeconds)) {
                velocity += Vector2.Negate(Vector2.Normalize(velocity)) * friction * frameSeconds;
            }
            // Set to zero to not overshoot
            else {
                velocity = Vector2.Zero;
            }

            Position += velocity;
            #endregion
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
            spriteBatch.Draw(Texture, Position, FramePosition, Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, 0f);
        }

        // Returns the normalized Vector2 of the player's movement direction
        private Vector2 GetInputDirection(KeyboardState currentKeyState) {
            var inputDirection = Vector2.Zero;

            if (currentKeyState.IsKeyDown(Keys.Right)) {
                inputDirection.X = 1;
            } else if (currentKeyState.IsKeyDown(Keys.Left)) {
                inputDirection.X = -1;
            }

            if (currentKeyState.IsKeyDown(Keys.Down)) {
                inputDirection.Y = 1;
            } else if (currentKeyState.IsKeyDown(Keys.Up)) {
                inputDirection.Y = -1;
            }

            // Only normalize the vector if it has magnitude, otherwise it divides by zero
            if (inputDirection.Length() > 0) {
                inputDirection.Normalize();
            }

            return inputDirection;
        }

        // Calculated in radians
        private float VectorToAngle(Vector2 v) {
            return (float)Math.Atan2(v.Y, v.X);
        }
    }
}
