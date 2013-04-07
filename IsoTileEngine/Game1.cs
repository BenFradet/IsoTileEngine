using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using IsoTileEngine.Sprites;

namespace IsoTileEngine
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        SpriteFont pericles6;
        SpriteFont pericles15;

        TileMap map;
        int squareAcross = 17;
        int squareDown = 37;

        int baseOffsetX = -32;
        int baseOffsetY = -64;

        int a = 0, b = 0;

        float heightRowDepthMod = 0.00001f;

        bool coordinates = false;

        SpriteAnimation player;

        Texture2D fogOfWar;
        Texture2D hilight;

        KeyboardState currentKeyboardState;
        KeyboardState previousKeyboardState;

        MouseState currentMouseState;
        MouseState previousMouseState;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            this.IsMouseVisible = true;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            map = new TileMap(Content.Load<Texture2D>("Textures/Tilesets/mousemap"), Content.Load<Texture2D>("Textures/Tilesets/slopemap"));

            Tile.TileSetTexture = Content.Load<Texture2D>("Textures/Tilesets/tileset");
            fogOfWar = Content.Load<Texture2D>("Textures/fogofwar");
            hilight = Content.Load<Texture2D>("Textures/hilight");

            player = new SpriteAnimation(Content.Load<Texture2D>("Textures/Characters/player"));

            player.AddAnimation("walkEast", 0, 0, 48, 48, 8, 0.1f);
            player.AddAnimation("walkNorth", 0, 48, 48, 48, 8, 0.1f);
            player.AddAnimation("walkNorthEast", 0, 48 * 2, 48, 48, 8, 0.1f);
            player.AddAnimation("walkNorthWest", 0, 48 * 3, 48, 48, 8, 0.1f);
            player.AddAnimation("walkSouth", 0, 48 * 4, 48, 48, 8, 0.1f);
            player.AddAnimation("walkSouthEast", 0, 48 * 5, 48, 48, 8, 0.1f);
            player.AddAnimation("walkSouthWest", 0, 48 * 6, 48, 48, 8, 0.1f);
            player.AddAnimation("walkWest", 0, 48 * 7, 48, 48, 8, 0.1f);

            player.AddAnimation("idleEast", 0, 0, 48, 48, 1, 0.2f);
            player.AddAnimation("idleNorth", 0, 48, 48, 48, 1, 0.2f);
            player.AddAnimation("idleNorthEast", 0, 48 * 2, 48, 48, 1, 0.2f);
            player.AddAnimation("idleNorthWest", 0, 48 * 3, 48, 48, 1, 0.2f);
            player.AddAnimation("idleSouth", 0, 48 * 4, 48, 48, 1, 0.2f);
            player.AddAnimation("idleSouthEast", 0, 48 * 5, 48, 48, 1, 0.2f);
            player.AddAnimation("idleSouthWest", 0, 48 * 6, 48, 48, 1, 0.2f);
            player.AddAnimation("idleWest", 0, 48 * 7, 48, 48, 1, 0.2f);

            player.Position = new Vector2(100, 100);
            player.DrawOffset = new Vector2(-24, -38);
            player.CurrentAnimation = "walkEast";
            player.IsAnimating = true;

            pericles15 = Content.Load<SpriteFont>("Fonts/pericles15");
            pericles6 = Content.Load<SpriteFont>("Fonts/pericles6");

            currentKeyboardState = new KeyboardState();
            previousKeyboardState = new KeyboardState();

            currentMouseState = new MouseState();

            Camera.ViewWidth = this.graphics.PreferredBackBufferWidth;
            Camera.ViewHeight = this.graphics.PreferredBackBufferHeight;
            Camera.WorldWidth = (map.MapWidth - 2) * Tile.TileStepX;
            Camera.WorldHeight = (map.MapHeight - 2) * Tile.TileStepY;
            Camera.DisplayOffset = new Vector2(baseOffsetX, baseOffsetY);
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            currentKeyboardState = Keyboard.GetState();
            currentMouseState = Mouse.GetState();

            Vector2 moveVector = Vector2.Zero;
            Vector2 moveDir = Vector2.Zero;
            string animation = "";

            if (currentKeyboardState.IsKeyDown(Keys.A))
            {
                moveDir = new Vector2(-2, -1);
                animation = "walkNorthWest";
                moveVector += new Vector2(-2, -1);
            }
            else if (currentKeyboardState.IsKeyDown(Keys.Z))
            {
                moveDir = new Vector2(0, -1);
                animation = "walkNorth";
                moveVector += new Vector2(0, -1);
            }
            else if (currentKeyboardState.IsKeyDown(Keys.E))
            {
                moveDir = new Vector2(2, -1);
                animation = "walkNorthEast";
                moveVector += new Vector2(2, -1);
            }
            else if (currentKeyboardState.IsKeyDown(Keys.Q))
            {
                moveDir = new Vector2(-2, 0);
                animation = "walkWest";
                moveVector += new Vector2(-2, 0);
            }
            else if (currentKeyboardState.IsKeyDown(Keys.D))
            {
                moveDir = new Vector2(2, 0);
                animation = "walkEast";
                moveVector += new Vector2(2, 0);
            }
            else if (currentKeyboardState.IsKeyDown(Keys.W))
            {
                moveDir = new Vector2(-2, 1);
                animation = "walkSouthWest";
                moveVector += new Vector2(-2, 1);
            }
            else if (currentKeyboardState.IsKeyDown(Keys.S))
            {
                moveDir = new Vector2(0, 1);
                animation = "walkSouth";
                moveVector += new Vector2(0, 1);
            }
            else if (currentKeyboardState.IsKeyDown(Keys.X))
            {
                moveDir = new Vector2(2, 1);
                animation = "walkSouthEast";
                moveVector += new Vector2(2, 1);
            }

            if (!map.GetCellAtWorldPoint(player.Position + moveDir).Walkable)
                moveDir = Vector2.Zero;

            if (Math.Abs(map.GetOverallHeight(player.Position) - map.GetOverallHeight(player.Position + moveDir)) > 10)
                moveDir = Vector2.Zero;

            if (moveDir.Length() != 0)
            {
                player.Move((int)moveDir.X, (int)moveDir.Y);
                if (player.CurrentAnimation != animation)
                    player.CurrentAnimation = animation;
            }
            else
                player.CurrentAnimation = "idle" + player.CurrentAnimation.Substring(4);

            float playerX = MathHelper.Clamp(player.Position.X, player.DrawOffset.X, Camera.WorldWidth);
            float playerY = MathHelper.Clamp(player.Position.Y, player.DrawOffset.Y, Camera.WorldHeight);
            player.Position = new Vector2(playerX, playerY);

            Vector2 testPosition = Camera.WorldToScreen(player.Position);
            if (testPosition.X < 100)
                Camera.Move(new Vector2(testPosition.X - 100, 0));
            else if (testPosition.X > Camera.ViewWidth - 100)
                Camera.Move(new Vector2(testPosition.X - (Camera.ViewWidth - 100), 0));
            if (testPosition.Y < 100)
                Camera.Move(new Vector2(0, testPosition.Y - 100));
            else if (testPosition.Y > Camera.ViewHeight - 100)
                Camera.Move(new Vector2(0, testPosition.Y - (Camera.ViewHeight - 100)));

            player.Update(gameTime);

            if (currentKeyboardState.IsKeyDown(Keys.C) && !previousKeyboardState.IsKeyDown(Keys.C))
                coordinates = !coordinates;

            UpdateFogOfWar();

            previousKeyboardState = currentKeyboardState;

            base.Update(gameTime);
        }

        private void UpdateFogOfWar()
        {
            Point playerMap = map.WorldToMapCell(new Point((int)player.Position.X, (int)player.Position.Y));

            a = playerMap.X;
            b = playerMap.Y;

            for(int y = 0; y < map.MapHeight; y++)
                for (int x = 0; x < map.MapWidth; x++)
                {
                    map.Rows[y].Columns[x].IsVisible = false;
                }

            for (int y = playerMap.Y - 3; y <= playerMap.Y + 3; y++)
            {
                for (int x = playerMap.X - 3; x <= playerMap.X + 3; x++)
                {
                    if (x >= 0 && x < map.MapWidth && y >= 0 && y < map.MapHeight)
                    {
                        if (y % 2 != 0)
                        {
                            if ((x == playerMap.X + 1 && y == playerMap.Y) || (x == playerMap.X && y <= playerMap.Y + 2 && y >= playerMap.Y - 2) || (x == playerMap.X - 1 && y <= playerMap.Y + 1 && y >= playerMap.Y - 1))
                            {
                                map.Rows[y].Columns[x].Explored = true;
                                map.Rows[y].Columns[x].IsVisible = true;
                            }
                            else
                                map.Rows[y].Columns[x].IsVisible = false;
                        }
                        else
                        {
                            if ((x == playerMap.X - 1 && y == playerMap.Y) || (x == playerMap.X && y <= playerMap.Y + 2 && y >= playerMap.Y - 2) || (x == playerMap.X + 1 && y <= playerMap.Y + 1 && y >= playerMap.Y - 1))
                            {
                                map.Rows[y].Columns[x].Explored = true;
                                map.Rows[y].Columns[x].IsVisible = true;
                            }
                            else
                                map.Rows[y].Columns[x].IsVisible = false;
                        }
                    }
                }
            }

            Vector2 hilightLocNow = Camera.ScreenToWorld(new Vector2(currentMouseState.X, currentMouseState.Y));
            Point hilightPointNow = map.WorldToMapCell(new Point((int)hilightLocNow.X, (int)hilightLocNow.Y));
            if (hilightPointNow.X >= 0 && hilightPointNow.X <= map.MapWidth && hilightPointNow.Y >= 0 && hilightPointNow.Y <= map.MapHeight)
            {
                map.Rows[hilightPointNow.Y].Columns[hilightPointNow.X].IsVisible = true;
            }

        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);

            Vector2 firstSquare = new Vector2(Camera.Location.X / Tile.TileStepX, Camera.Location.Y / Tile.TileStepY);
            int firstX = (int)firstSquare.X;
            int firstY = (int)firstSquare.Y;

            Vector2 squareOffset = new Vector2(Camera.Location.X % Tile.TileStepX, Camera.Location.Y % Tile.TileStepY);
            int offsetX = (int)squareOffset.X;
            int offsetY = (int)squareOffset.Y;

            float maxDepth = ((map.MapWidth + 1) * (map.MapHeight + 1) * Tile.TileWidth) / 10;
            float depthOffset;
            Point playerMapPoint = map.WorldToMapCell(new Point((int)player.Position.X, (int)player.Position.Y));
            for (int y = 0; y < squareDown; y++)
            {
                int rowOffset = 0;
                if ((firstY + y) % 2 == 1)
                    rowOffset = Tile.OddRowXOffset;
                for (int x = 0; x < squareAcross; x++)
                {
                    int mapX = firstX + x;
                    int mapY = firstY + y;
                    depthOffset = 0.7f - ((mapX + mapY * Tile.TileWidth) / maxDepth);

                    if (mapX >= map.MapWidth || mapY >= map.MapHeight)
                        continue;

                    foreach (int tileID in map.Rows[mapY].Columns[mapX].BaseTiles)
                    {
                        spriteBatch.Draw(Tile.TileSetTexture,
                            Camera.WorldToScreen(new Vector2(mapX * Tile.TileStepX + rowOffset, mapY * Tile.TileStepY)),
                            Tile.GetSourceRectangle(tileID),
                            Color.White, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 1.0f);
                    }

                    int heightRow = 0;
                    foreach (int tileID in map.Rows[mapY].Columns[mapX].HeightTiles)
                    {
                        spriteBatch.Draw(Tile.TileSetTexture, 
                            Camera.WorldToScreen(new Vector2(mapX * Tile.TileStepX + rowOffset, mapY * Tile.TileStepY - heightRow * Tile.HeightTileOffset)), 
                                Tile.GetSourceRectangle(tileID), 
                                Color.White, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 
                                depthOffset - (float)heightRow * heightRowDepthMod);
                        heightRow++;
                    }

                    foreach (int tileID in map.Rows[mapY].Columns[mapX].TopperTiles)
                    {
                        spriteBatch.Draw(Tile.TileSetTexture, 
                            Camera.WorldToScreen(new Vector2(mapX * Tile.TileStepX + rowOffset, mapY * Tile.TileStepY)), 
                                Tile.GetSourceRectangle(tileID), 
                                Color.White, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 
                                depthOffset - (float)heightRow * heightRowDepthMod);
                    }

                    if (map.Rows[y + firstY].Columns[x + firstX].Explored && !map.Rows[y + firstY].Columns[x + firstX].IsVisible)
                        spriteBatch.Draw(fogOfWar,
                            new Rectangle(x * Tile.TileStepX - offsetX + rowOffset + baseOffsetX,
                                y * Tile.TileStepY - offsetY + baseOffsetY, Tile.TileWidth, Tile.TileHeight),
                                new Color(255, 255, 255, 100));
                    else if (!map.Rows[y + firstY].Columns[x + firstX].Explored && !map.Rows[y + firstY].Columns[x + firstX].IsVisible)
                        spriteBatch.Draw(fogOfWar,
                            new Rectangle(x * Tile.TileStepX - offsetX + rowOffset + baseOffsetX,
                                y * Tile.TileStepY - offsetY + baseOffsetY, Tile.TileWidth, Tile.TileHeight),
                                new Color(255, 255, 255, 200));

                    if (coordinates)
                        spriteBatch.DrawString(pericles6, (x + firstX).ToString() + ", " + (y + firstY).ToString(),
                            new Vector2((x * Tile.TileStepX) - offsetX + rowOffset + baseOffsetX + 24,
                                (y * Tile.TileStepY) - offsetY + baseOffsetY + 48),
                                Color.White, 0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);

                    if (mapX == playerMapPoint.X && mapY == playerMapPoint.Y)
                        player.DrawDepth = depthOffset - (float)(heightRow + 2) * heightRowDepthMod;
                }
            }

            player.Draw(spriteBatch, 0, -map.GetOverallHeight(player.Position));

            Vector2 hilightLoc = Camera.ScreenToWorld(new Vector2(Mouse.GetState().X, Mouse.GetState().Y));
            Point hilightPoint = map.WorldToMapCell(new Point((int)hilightLoc.X, (int)hilightLoc.Y));
            int hilightRowOffset = 0;
            if (hilightPoint.Y % 2 == 1)
                hilightRowOffset = Tile.OddRowXOffset;
            spriteBatch.Draw(hilight,
                Camera.WorldToScreen(new Vector2(hilightPoint.X * Tile.TileStepX + hilightRowOffset, (hilightPoint.Y + 2) * Tile.TileStepY)),
                new Rectangle(0, 0, Tile.TileStepX, Tile.HeightTileOffset), 
                Color.White, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);

            spriteBatch.DrawString(pericles15, "x = " + a + "; y = " + b, Vector2.Zero, Color.White);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
