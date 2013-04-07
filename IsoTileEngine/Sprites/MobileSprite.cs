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
        Queue<Vector2> path;
        Vector2 target;
        float speed = 1.0f;
        int collisionBufferX = 0;
        int collisionBufferY = 0;
        bool active = true;
        bool movingTowardsTarget = true;
        bool pathing = true;
        bool loopPath = true;
        bool collidable = true;
        bool visible = true;
        bool deactivateAtEndOfPath = false;
        bool hideAtEndOfPath = false;
        string endPathAnimation;

        #region Constructors
        public MobileSprite(Texture2D texture)
        {
            sprite = new SpriteAnimation(texture);
            path = new Queue<Vector2>();
            endPathAnimation = null;
        }
        #endregion

        #region Methods
        public void Update(GameTime gameTime)
        {
            if (active && movingTowardsTarget)
            {
                if (target != null)
                {
                    Vector2 delta = new Vector2(target.X - sprite.X, target.Y - sprite.Y);
                    if (delta.Length() > speed)
                    {
                        delta.Normalize();
                        delta *= speed;
                        Position += delta;
                    }
                    else
                    {
                        if (target == Position)
                        {
                            if (pathing)
                            {
                                if (path.Count > 0)
                                {
                                    target = path.Dequeue();
                                    if (loopPath)
                                        path.Enqueue(target);
                                }
                                else
                                {
                                    if (endPathAnimation != null)
                                    {
                                        if (sprite.CurrentAnimation != endPathAnimation)
                                            sprite.CurrentAnimation = endPathAnimation;
                                    }
                                    if (deactivateAtEndOfPath)
                                        active = false;
                                    if (hideAtEndOfPath)
                                        visible = false;
                                }
                            }
                        }
                        else
                            Position = target;
                    }
                }
            }
            if (active)
                sprite.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (visible)
                sprite.Draw(spriteBatch, 0, 0);
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

        public Vector2 Target
        {
            get { return target; }
            set { target = value; }
        }

        public int HorizontalCollisionBuffer
        {
            get { return collisionBufferX; }
            set { collisionBufferX = value; }
        }

        public int VerticalCollisionBuffer
        {
            get { return collisionBufferY; }
            set { collisionBufferY = value; }
        }

        public bool IsPathing
        {
            get { return pathing; }
            set { pathing = value; }
        }

        public bool DeactivateAfterPathing
        {
            get { return deactivateAtEndOfPath; }
            set { deactivateAtEndOfPath = value; }
        }

        public bool LoopPath
        {
            get { return loopPath; }
            set { loopPath = value; }
        }

        public string EndPathAnimation
        {
            get { return endPathAnimation; }
            set { endPathAnimation = value; }
        }

        public bool HideAtEndOfPath
        {
            get { return hideAtEndOfPath; }
            set { hideAtEndOfPath = value; }
        }

        public bool IsVisible
        {
            get { return visible; }
            set { visible = true; }
        }

        public float Speed
        {
            get { return speed; }
            set { speed = value; }
        }

        public bool IsActive
        {
            get { return active; }
            set { active = value; }
        }

        public bool IsMoving
        {
            get { return movingTowardsTarget; }
            set { movingTowardsTarget = value; }
        }

        public bool IsCollidable
        {
            get { return collidable; }
            set { collidable = value; }
        }

        public Rectangle BoundingBox
        {
            get { return sprite.BoundingBox; }
        }

        public Rectangle CollisionBox
        {
            get
            {
                return new Rectangle(sprite.BoundingBox.X + collisionBufferX, sprite.BoundingBox.Y + collisionBufferY, sprite.Width - 2 * collisionBufferX, sprite.Height - 2 * collisionBufferY);
            }
        }
        #endregion
    }
}
