using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IsoTileEngine
{
    class MapCell
    {
        public List<int> BaseTiles = new List<int>();
        public List<int> HeightTiles = new List<int>();
        public List<int> TopperTiles = new List<int>();

        #region Constructors
        public MapCell(int tileID)
        {
            TileID = tileID;
            Walkable = true;
            IsVisible = false;
            Explored = false;
            SlopeMap = -1;
        }
        #endregion

        #region Methods
        public void AddBaseTile(int tileID)
        {
            BaseTiles.Add(tileID);
        }

        public void AddHeightTile(int tileID)
        {
            HeightTiles.Add(tileID);
        }

        public void AddToperTile(int tileID)
        {
            TopperTiles.Add(tileID);
        }
        #endregion

        #region Properties
        public int SlopeMap { get; set; }

        public bool Walkable { get; set; }

        public bool Explored { get; set; }

        public bool IsVisible { get; set; }

        public int TileID
        {
            get { return BaseTiles.Count > 0 ? BaseTiles[0] : 0; }
            set
            {
                if (BaseTiles.Count > 0)
                    BaseTiles[0] = value;
                else
                    AddBaseTile(value);
            }
        }
        #endregion
    }
}
