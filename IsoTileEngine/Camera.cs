using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace IsoTileEngine
{
    static class Camera
    {
        static private Vector2 location = Vector2.Zero;

        #region Methods
        public static Vector2 WorldToScreen(Vector2 worldPosition)
        {
            return worldPosition - Location + DisplayOffset;
        }

        public static Vector2 ScreenToWorld(Vector2 screenPosition)
        {
            return screenPosition + Location - DisplayOffset;
        }

        public static void Move(Vector2 offset)
        {
            Location += offset;
        }
        #endregion

        #region Properties
        public static int ViewWidth { get; set; }
        public static int ViewHeight { get; set; }
        public static int WorldWidth { get; set; }
        public static int WorldHeight { get; set; }

        public static Vector2 DisplayOffset { get; set; }
        public static Vector2 Location
        {
            get
            {
                return location;
            }
            set
            {
                location = new Vector2(MathHelper.Clamp(value.X, 0, WorldWidth - ViewWidth), MathHelper.Clamp(value.Y, 0, WorldHeight - ViewHeight));
            }
        }
        #endregion

    }
}
