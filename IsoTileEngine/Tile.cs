using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace IsoTileEngine
{
    static class Tile
    {
        static public Texture2D TileSetTexture;

        static public readonly int TileWidth = 64;
        static public readonly int TileHeight = 64;

        static public readonly int TileStepX = 64;
        static public readonly int TileStepY = 16;

        static public readonly int OddRowXOffset = 32;
        static public readonly int HeightTileOffset = 32;

        static public Rectangle GetSourceRectangle(int tileIndex)
        {
            int tileY = tileIndex / (TileSetTexture.Width / TileWidth);
            int tileX = tileIndex % (TileSetTexture.Width / TileWidth);

            return new Rectangle(tileX * TileWidth, tileY * TileHeight, TileWidth, TileHeight);
        }
    }
}
