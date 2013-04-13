using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace IsoTileEngine.Sprites
{
    class MobileSprite
    {
        SpriteAnimation sprite;
        TileMap map;
        Queue<Vector2> path;
        Vector2 currentTarget;
        Vector2 nullTarget;

        #region Constructors
        public MobileSprite(Texture2D texture, TileMap map)
        {
            sprite = new SpriteAnimation(texture);
            this.map = map;
            path = new Queue<Vector2>();
            currentTarget = new Vector2(-1, -1);
            nullTarget = new Vector2(-1, -1);
        }
        #endregion

        #region Methods
        public void Update(GameTime gameTime)
        {
            if (path.Count > 0 && map.GetCellAtWorldPoint(currentTarget) != map.GetCellAtWorldPoint(Position))
            {
                currentTarget = path.Dequeue();
            }
            else if (currentTarget != Position && currentTarget != nullTarget)
            {
                sprite.Move((int)FindBestMove(GetPossibleMoves(Position)).X, (int)FindBestMove(GetPossibleMoves(Position)).Y);
            }

            sprite.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch, int xOffset, int yOffset)
        {
            sprite.Draw(spriteBatch, xOffset, yOffset);
        }

        public void AddPathNode(Vector2 node)
        {
            path.Enqueue(node);
        }

        public void AddPathNode(int x, int y)
        {
            path.Enqueue(new Vector2(x, y));
        }

        public void ClearPathNodes()
        {
            path.Clear();
        }

        private List<Vector2> GetPossibleMoves(Vector2 position)
        {
            List<Vector2> possibleMoves = new List<Vector2>();
            Point currentCell = map.WorldToMapCell(position);

            if (currentCell.Y % 2 == 0)
            {
                if (currentCell.X >= 1 && currentCell.Y >= 1 && map.Rows[currentCell.X - 1].Columns[currentCell.Y - 1].Walkable)
                    possibleMoves.Add(new Vector2(-Tile.TileStepX / 2f, -Tile.TileStepY / 2f));
                if (currentCell.X >= 1 && map.Rows[currentCell.X - 1].Columns[currentCell.Y].Walkable)
                    possibleMoves.Add(new Vector2(-Tile.TileStepX, 0));
                if (currentCell.X >= 1 && currentCell.Y < map.MapHeight && map.Rows[currentCell.X - 1].Columns[currentCell.Y + 1].Walkable)
                    possibleMoves.Add(new Vector2(-Tile.TileStepX / 2f, Tile.TileStepY / 2f));
                if (currentCell.Y > 1 && map.Rows[currentCell.X].Columns[currentCell.Y - 2].Walkable)
                    possibleMoves.Add(new Vector2(0, -Tile.TileStepY));
                if (currentCell.Y >= 1 && map.Rows[currentCell.X].Columns[currentCell.Y - 1].Walkable)
                    possibleMoves.Add(new Vector2(Tile.TileStepX / 2f, -Tile.TileStepY / 2f));
                if (currentCell.Y < map.MapHeight && map.Rows[currentCell.X].Columns[currentCell.Y + 1].Walkable)
                    possibleMoves.Add(new Vector2(Tile.TileStepX / 2f, Tile.TileStepY / 2f));
                if (currentCell.Y < map.MapHeight - 1 && map.Rows[currentCell.X].Columns[currentCell.Y + 2].Walkable)
                    possibleMoves.Add(new Vector2(0, Tile.TileStepY));
                if (currentCell.X < map.MapWidth && map.Rows[currentCell.X + 1].Columns[currentCell.Y].Walkable)
                    possibleMoves.Add(new Vector2(Tile.TileStepX, 0));
            }
            else
            {
                if (currentCell.X >= 1 && map.Rows[currentCell.X - 1].Columns[currentCell.Y].Walkable)
                    possibleMoves.Add(new Vector2(-Tile.TileStepX, 0));
                if (currentCell.Y > 1 && map.Rows[currentCell.X].Columns[currentCell.Y - 2].Walkable)
                    possibleMoves.Add(new Vector2(0, -Tile.TileStepY));
                if (currentCell.Y >= 1 && map.Rows[currentCell.X].Columns[currentCell.Y - 1].Walkable)
                    possibleMoves.Add(new Vector2(-Tile.TileStepX / 2f, -Tile.TileStepY / 2f));
                if (currentCell.Y < map.MapHeight && map.Rows[currentCell.X].Columns[currentCell.Y + 1].Walkable)
                    possibleMoves.Add(new Vector2(-Tile.TileStepX / 2f, Tile.TileStepY / 2f));
                if (currentCell.Y < map.MapHeight - 1 && map.Rows[currentCell.X].Columns[currentCell.Y + 2].Walkable)
                    possibleMoves.Add(new Vector2(0, Tile.TileStepY));
                if (currentCell.X < map.MapWidth && currentCell.Y >= 1 && map.Rows[currentCell.X + 1].Columns[currentCell.Y - 1].Walkable)
                    possibleMoves.Add(new Vector2(Tile.TileStepX / 2f, -Tile.TileStepY / 2f));
                if (currentCell.X < map.MapWidth && map.Rows[currentCell.X + 1].Columns[currentCell.Y].Walkable)
                    possibleMoves.Add(new Vector2(Tile.TileStepX, 0));
                if (currentCell.X < map.MapWidth && currentCell.Y < map.MapHeight && map.Rows[currentCell.X + 1].Columns[currentCell.Y + 1].Walkable)
                    possibleMoves.Add(new Vector2(Tile.TileStepX / 2f, Tile.TileStepY / 2f));
            }

            return possibleMoves;
        }

        private Vector2 FindBestMove(List<Vector2> moves)
        {
            List<float> deltas = new List<float>();
            foreach(Vector2 vec in moves)
            {
                float dx = currentTarget.X - (Position.X + vec.X);
                float dy = currentTarget.Y - (Position.Y + vec.Y);
                deltas.Add((float)Math.Sqrt(dx * dx + dy * dy));
            }

            return moves[deltas.IndexOf(deltas.Min())];
        }
        #endregion

        #region Properties
        public SpriteAnimation Sprite
        {
            get { return sprite; }
        }

        public Vector2 Position
        {
            get { return sprite.Position; }
            set { sprite.Position = value; }
        }

        /*public int HorizontalCollisionBuffer
        {
            get { return collisionBufferX; }
            set { collisionBufferX = value; }
        }

        public int VerticalCollisionBuffer
        {
            get { return collisionBufferY; }
            set { collisionBufferY = value; }
        }*/

        public bool IsMoving
        {
            get { return path.Count > 0; }
        }

        public Rectangle BoundingBox
        {
            get { return sprite.BoundingBox; }
        }

        /*public Rectangle CollisionBox
        {
            get
            {
                return new Rectangle(sprite.BoundingBox.X + collisionBufferX, sprite.BoundingBox.Y + collisionBufferY, sprite.Width - 2 * collisionBufferX, sprite.Height - 2 * collisionBufferY);
            }
        }*/
        #endregion
    }
}
