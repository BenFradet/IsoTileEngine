using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace IsoTileEngine
{
    class MapRow
    {
        public List<MapCell> Columns = new List<MapCell>();
    }

    class TileMap
    {
        private Texture2D mouseMap;
        private Texture2D slopeMap;
        public List<MapRow> Rows = new List<MapRow>();
        public int MapWidth = 50;
        public int MapHeight = 50;

        #region Constructors
        public TileMap(Texture2D mouseMap, Texture2D slopeMap)
        {
            this.mouseMap = mouseMap;
            this.slopeMap = slopeMap;

            for (int y = 0; y < MapHeight; y++)
            {
                MapRow thisRow = new MapRow();
                for (int x = 0; x < MapWidth; x++)
                {
                    thisRow.Columns.Add(new MapCell(0));
                }
                Rows.Add(thisRow);
            }

            Rows[0].Columns[3].TileID = 3;
            Rows[0].Columns[4].TileID = 3;
            Rows[0].Columns[5].TileID = 1;
            Rows[0].Columns[6].TileID = 1;
            Rows[0].Columns[7].TileID = 1;

            Rows[1].Columns[3].TileID = 3;
            Rows[1].Columns[4].TileID = 1;
            Rows[1].Columns[5].TileID = 1;
            Rows[1].Columns[6].TileID = 1;
            Rows[1].Columns[7].TileID = 1;

            Rows[2].Columns[2].TileID = 3;
            Rows[2].Columns[3].TileID = 1;
            Rows[2].Columns[4].TileID = 1;
            Rows[2].Columns[5].TileID = 1;
            Rows[2].Columns[6].TileID = 1;
            Rows[2].Columns[7].TileID = 1;

            Rows[3].Columns[2].TileID = 3;
            Rows[3].Columns[3].TileID = 1;
            Rows[3].Columns[4].TileID = 1;
            Rows[3].Columns[5].TileID = 2;
            Rows[3].Columns[6].TileID = 2;
            Rows[3].Columns[7].TileID = 2;

            Rows[4].Columns[2].TileID = 3;
            Rows[4].Columns[3].TileID = 1;
            Rows[4].Columns[4].TileID = 1;
            Rows[4].Columns[5].TileID = 2;
            Rows[4].Columns[6].TileID = 2;
            Rows[4].Columns[7].TileID = 2;

            Rows[5].Columns[2].TileID = 3;
            Rows[5].Columns[3].TileID = 1;
            Rows[5].Columns[4].TileID = 1;
            Rows[5].Columns[5].TileID = 2;
            Rows[5].Columns[6].TileID = 2;
            Rows[5].Columns[7].TileID = 2;

            Rows[16].Columns[4].AddHeightTile(54);

            Rows[17].Columns[3].AddHeightTile(54);

            Rows[15].Columns[3].AddHeightTile(54);
            Rows[16].Columns[3].AddHeightTile(53);

            Rows[15].Columns[4].AddHeightTile(54);
            Rows[15].Columns[4].AddHeightTile(54);
            Rows[15].Columns[4].AddHeightTile(51);

            Rows[18].Columns[3].AddHeightTile(51);
            Rows[19].Columns[3].AddHeightTile(50);
            Rows[18].Columns[4].AddHeightTile(55);

            Rows[14].Columns[4].AddHeightTile(54);

            Rows[14].Columns[5].AddHeightTile(62);
            Rows[14].Columns[5].AddHeightTile(61);
            Rows[14].Columns[5].AddHeightTile(63);

            Rows[17].Columns[4].AddToperTile(114);
            Rows[16].Columns[5].AddToperTile(115);
            Rows[14].Columns[4].AddToperTile(125);
            Rows[15].Columns[5].AddToperTile(91);
            Rows[16].Columns[6].AddToperTile(94);

            Rows[15].Columns[5].Walkable = false;
            Rows[15].Columns[6].Walkable = false;
            Rows[16].Columns[5].Walkable = false;
            Rows[16].Columns[6].Walkable = false;
            Rows[17].Columns[5].Walkable = false;
            Rows[17].Columns[6].Walkable = false;

            Rows[12].Columns[9].AddHeightTile(34);
            Rows[11].Columns[9].AddHeightTile(34);
            Rows[11].Columns[8].AddHeightTile(34);
            Rows[10].Columns[9].AddHeightTile(34);

            Rows[12].Columns[8].AddToperTile(31);
            Rows[12].Columns[8].SlopeMap = 0;
            Rows[13].Columns[8].AddToperTile(31);
            Rows[13].Columns[8].SlopeMap = 0;

            Rows[12].Columns[10].AddToperTile(32);
            Rows[12].Columns[10].SlopeMap = 1;
            Rows[13].Columns[9].AddToperTile(32);
            Rows[13].Columns[9].SlopeMap = 1;

            Rows[14].Columns[9].AddToperTile(30);
            Rows[14].Columns[9].SlopeMap = 4;
        }
        #endregion

        #region Methods
        public Point WorldToMapCell(Point worldPoint, out Point localPoint)
        {
            Point mapCell = new Point((int)(worldPoint.X / mouseMap.Width), (int)(worldPoint.Y / mouseMap.Height) * 2);

            int localPointX = worldPoint.X % mouseMap.Width;
            int localPointY = worldPoint.Y % mouseMap.Height;

            int dx = 0;
            int dy = 0;

            uint[] uints = new uint[1];

            if (new Rectangle(0, 0, mouseMap.Width, mouseMap.Height).Contains(localPointX, localPointY))
            {
                mouseMap.GetData(0, new Rectangle(localPointX, localPointY, 1, 1), uints, 0, 1);

                if (uints[0] == 0xFF0000FF)
                {
                    dx = -1;
                    dy = -1;
                    localPointX += mouseMap.Width / 2;
                    localPointY += mouseMap.Height / 2;
                }
                else if (uints[0] == 0xFF00FF00)
                {
                    dx = -1;
                    localPointX += mouseMap.Width / 2;
                    dy = 1;
                    localPointY -= mouseMap.Height / 2;
                }
                else if (uints[0] == 0xFF00FFFF)
                {
                    dy = -1;
                    localPointX -= mouseMap.Width / 2;
                    localPointY += mouseMap.Height / 2;
                }
                else if (uints[0] == 0xFFFF0000)
                {
                    dy = 1;
                    localPointX -= mouseMap.Width / 2;
                    localPointY -= mouseMap.Width / 2;
                }
            }
            mapCell.X += dx;
            mapCell.Y += dy - 2;

            localPoint = new Point(localPointX, localPointY);
            if (mapCell.Y < 0)
                mapCell.Y = 0;
            if (mapCell.X < 0)
                mapCell.X = 0;
            if (mapCell.Y > MapHeight)
                mapCell.Y = MapHeight;
            if (mapCell.X > MapWidth)
                mapCell.X = MapWidth;
            return mapCell;
        }

        public Point WorldToMapCell(Point worldPoint)
        {
            Point tmp;
            return WorldToMapCell(worldPoint, out tmp);
        }

        public Point WorldToMapCell(Vector2 worldPoint)
        {
            return WorldToMapCell(new Point((int)worldPoint.X, (int)worldPoint.Y));
        }

        public MapCell GetCellAtWorldPoint(Point worldPoint)
        {
            Point mapPoint = WorldToMapCell(worldPoint);
            return Rows[mapPoint.Y].Columns[mapPoint.X];
        }

        public MapCell GetCellAtWorldPoint(Vector2 worldPoint)
        {
            return GetCellAtWorldPoint(new Point((int)worldPoint.X, (int)worldPoint.Y));
        }

        public int GetSlopeMapHeight(Point localPixel, int slope)
        {
            Point texturePoint = new Point(slope * mouseMap.Width + localPixel.X, localPixel.Y);
            Color[] slopeColor = new Color[1];

            if (new Rectangle(0, 0, slopeMap.Width, slopeMap.Height).Contains(texturePoint.X, texturePoint.Y))
            {
                slopeMap.GetData(0, new Rectangle(texturePoint.X, texturePoint.Y, 1, 1), slopeColor, 0, 1);
                return (int)(((float)(255 - slopeColor[0].R) / 255f) * Tile.HeightTileOffset);
            }
            return 0;
        }

        public int GetSlopeHeightAtWorldPoint(Point worldPoint)
        {
            Point localPoint;
            Point mapPoint = WorldToMapCell(worldPoint, out localPoint);
            int slope = -1;
            if(mapPoint.X >= 0 && mapPoint.X <= MapWidth && mapPoint.Y >= 0 && mapPoint.Y <= MapHeight)
                slope = Rows[mapPoint.Y].Columns[mapPoint.X].SlopeMap;

            return GetSlopeMapHeight(localPoint, slope);
        }

        public int GetSlopeHeightAtWorldPoint(Vector2 worldPoint)
        {
            return GetSlopeHeightAtWorldPoint(new Point((int)worldPoint.X, (int)worldPoint.Y));
        }

        public int GetOverallHeight(Point worldPoint)
        {
            Point mapCellPoint = WorldToMapCell(worldPoint);
            int height = 0;
            if(mapCellPoint.X >= 0 && mapCellPoint.X <= MapWidth && mapCellPoint.Y >= 0 && mapCellPoint.Y <= MapHeight)
                height = Rows[mapCellPoint.Y].Columns[mapCellPoint.X].HeightTiles.Count * Tile.HeightTileOffset;
            height += GetSlopeHeightAtWorldPoint(worldPoint);
            return height;
        }

        public int GetOverallHeight(Vector2 worldPoint)
        {
            return GetOverallHeight(new Point((int)worldPoint.X, (int)worldPoint.Y));
        }
        #endregion
    }
}
