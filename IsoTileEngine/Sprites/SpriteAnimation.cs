using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace IsoTileEngine.Sprites
{
    class SpriteAnimation
    {
        Texture2D texture;
        bool isAnimating = true;
        Color tint;
        Vector2 position;
        Vector2 lastPosition;
        Dictionary<string, FrameAnimation> animations;
        string currentAnimation;
        bool rotateByPosition = false;
        float rotation = 0.0f;
        Vector2 center;
        int width;
        int height;

        #region Constructors
        public SpriteAnimation(Texture2D texture)
        {
            this.texture = texture;
            tint = Color.White;
            position = Vector2.Zero;
            lastPosition = Vector2.Zero;
            animations = new Dictionary<string, FrameAnimation>();
            currentAnimation = null;
            DrawOffset = Vector2.Zero;
            DrawDepth = 0.0f;
        }
        #endregion

        #region
        public void Update(GameTime gameTime)
        {
            if (isAnimating)
            {
                if (CurrentFrameAnimation == null)
                {
                    if (animations.Count > 0)
                    {
                        string[] keys = new string[animations.Count];
                        animations.Keys.CopyTo(keys, 0);
                        currentAnimation = keys[0];
                    }
                    else
                        return;
                }

                CurrentFrameAnimation.Update(gameTime);

                if (!string.IsNullOrEmpty(CurrentFrameAnimation.NextAnimation))
                {
                    if (CurrentFrameAnimation.PlayCount > 0)
                        currentAnimation = CurrentFrameAnimation.NextAnimation;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, int xOffset, int yOffset)
        {
            if (isAnimating)
                spriteBatch.Draw(texture, 
                    Camera.WorldToScreen(position) + center + DrawOffset + new Vector2(xOffset, yOffset), 
                    CurrentFrameAnimation.FrameRectangle, 
                    tint, rotation, center, 1f, SpriteEffects.None, DrawDepth);
        }

        public void AddAnimation(string name, int x, int y, int width, int height, int frames, float frameLength)
        {
            animations.Add(name, new FrameAnimation(x, y, width, height, frames, frameLength));
            this.width = width;
            this.height = height;
            center = new Vector2(width / 2, height / 2);
        }

        public void AddAnimation(string name, int x, int y, int width, int height, int frames, float frameLength, string nextAnimation)
        {
            animations.Add(name, new FrameAnimation(x, y, width, height, frames, frameLength, nextAnimation));
            this.width = width;
            this.height = height;
            center = new Vector2(width / 2, height / 2);
        }

        public FrameAnimation GetAnimationByKey(string name)
        {
            if (animations.ContainsKey(name))
                return animations[name];
            else
                return null;
        }

        public void Move(int x, int y)
        {
            lastPosition = position;
            position.X += x;
            position.Y += y;
            UpdateRotation();
        }

        private void UpdateRotation()
        {
            if (rotateByPosition)
                rotation = (float)Math.Atan2(position.Y - lastPosition.Y, position.X - lastPosition.X);
        }
        #endregion

        #region Properties
        public Vector2 Position
        {
            get { return position; }
            set
            {
                lastPosition = position;
                position = value;
                if (position.X < 0)
                    position.X = 0;
                if (position.Y < 0)
                    position.Y = 0;
                UpdateRotation();
            }
        }

        public int X
        {
            get { return (int)position.X; }
            set
            {
                lastPosition.X = position.X;
                position.X = value;
                UpdateRotation();
            }
        }

        public int Y
        {
            get { return (int)position.Y; }
            set
            {
                lastPosition.Y = position.Y;
                position.Y = value;
                UpdateRotation();
            }
        }

        public int Width
        {
            get { return width; }
        }

        public int Height
        {
            get { return height; }
        }

        public bool AutoRotate
        {
            get { return rotateByPosition; }
            set { rotateByPosition = value; }
        }

        public float Rotation
        {
            get { return rotation; }
            set { rotation = value; }
        }

        public Rectangle BoundingBox
        {
            get { return new Rectangle(X, Y, Width, Height); }
        }

        public Texture2D Texture
        {
            get { return texture; }
        }

        public Color Tint
        {
            get { return tint; }
            set { tint = value; }
        }

        public bool IsAnimating
        {
            get { return isAnimating; }
            set { isAnimating = value; }
        }

        public FrameAnimation CurrentFrameAnimation
        {
            get
            {
                if (!string.IsNullOrEmpty(currentAnimation))
                    return animations[currentAnimation];
                else
                    return null;
            }
        }

        public string CurrentAnimation
        {
            get { return currentAnimation; }
            set
            {
                if (animations.ContainsKey(value))
                {
                    currentAnimation = value;
                    animations[currentAnimation].CurrentFrame = 0;
                    animations[currentAnimation].PlayCount = 0;
                }
            }
        }

        public Vector2 DrawOffset { get; set; }

        public float DrawDepth { get; set; }
        #endregion
    }
}
