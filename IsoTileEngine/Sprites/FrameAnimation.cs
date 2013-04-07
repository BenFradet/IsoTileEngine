using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace IsoTileEngine.Sprites
{
    class FrameAnimation : ICloneable
    {
        private Rectangle initialFrame;
        private int frameCount = 1;
        private int currentFrame = 0;
        private float frameLength = 0.2f;
        private float frameTimer = 0.0f;
        private int playCount = 0;
        private string nextAnimation = null;

        #region Constructors
        public FrameAnimation(Rectangle firstFrame, int frames)
        {
            initialFrame = firstFrame;
            frameCount = frames;
        }

        public FrameAnimation(int x, int y, int width, int height, int frames) :
            this(new Rectangle(x, y, width, height), frames) { }

        public FrameAnimation(int x, int y, int width, int height, int frames, float frameLength) :
            this(x, y, width, height, frames)
        {
            this.frameLength = frameLength;
        }

        public FrameAnimation(int x, int y, int width, int height, int frames, float frameLength, string nextAnimation) :
            this(x, y, width, height, frames, frameLength)
        {
            this.nextAnimation = nextAnimation;
        }
        #endregion

        #region Methods
        public void Update(GameTime gameTime)
        {
            frameTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (frameTimer > frameLength)
            {
                frameTimer = 0;
                currentFrame = (currentFrame + 1) % frameCount;
                if (currentFrame == 0)
                    playCount = (int)MathHelper.Min(playCount + 1, int.MaxValue);
            }
        }

        public object Clone()
        {
            return new FrameAnimation(initialFrame.X, initialFrame.Y, initialFrame.Width, initialFrame.Height, frameCount, frameLength, nextAnimation);
        }
        #endregion

        #region Properties
        public int FrameCount
        {
            get { return frameCount; }
            set { frameCount = value; }
        }

        public float FrameLength
        {
            get { return frameLength; }
            set { frameLength = value; }
        }

        public int CurrentFrame
        {
            get { return currentFrame; }
            set { currentFrame = (int)MathHelper.Clamp(value, 0, frameCount - 1); }
        }

        public int FrameWidth
        {
            get { return initialFrame.Width; }
        }

        public int FrameHeight
        {
            get { return initialFrame.Height; }
        }

        public Rectangle FrameRectangle
        {
            get
            {
                return new Rectangle(initialFrame.X + initialFrame.Width * currentFrame, initialFrame.Y, initialFrame.Width, initialFrame.Height);
            }
        }

        public int PlayCount
        {
            get { return playCount; }
            set { playCount = value; }
        }

        public string NextAnimation
        {
            get { return nextAnimation; }
            set { nextAnimation = value; }
        }
        #endregion
    }
}
