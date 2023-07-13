using System;
using System.Collections.Generic;
using UnityEngine;

namespace MoonBunny
{
    public class MoonBunnyRigidbody : MonoBehaviour
    {
        public Vector2Int GridJumpVelocity;
        public float Gravity;
        public GridObject GridObject;
        public List<MoonBunnyCollider.ColliderLayerMask> ColliderLayerList;

        public bool isMovingToRight => _movement.Velocity.x > 0;
        public bool isMovingToLeft => _movement.Velocity.x < 0;
        public bool isFalling => _movement.Velocity.y < 0;
            
        private List<MoonBunnyCollider> _colliderList;
        private MoonBunnyMovement _movement;

        private void Reset()
        {
            TryGetComponent<GridObject>(out GridObject);
        }

        private void Awake()
        {
            if (GridObject == null)
            {
                TryGetComponent<GridObject>(out GridObject);
            }
                        
            _movement = new MoonBunnyMovement(GridObject.GridTransform, Gravity);
            _movement.GridVelocity = GridJumpVelocity;
            
            _colliderList = new List<MoonBunnyCollider>();
            
            foreach (var colliderLayer in ColliderLayerList)
            {
                _colliderList.Add(new MoonBunnyCollider(_movement, colliderLayer));
            }

        }

        public void Update()
        {
            float deltaTime = Time.deltaTime;
            _movement.UpdateGravity(deltaTime);

            Collision collision;

            foreach (MoonBunnyCollider collider in _colliderList)
            {
                if (collider.WillCollide(_movement.Velocity, deltaTime, out collision))
                {
                    print($"RigidBody Collision with {collision.Other}");
                    if (collision is GimmickCollision gimmickCollision)
                    {
                        gimmickCollision.OnCollision();
                    }
                    _movement.Collide(collision);
                }
            }
            
            _movement.UpdateMove(deltaTime);
        }

        public void Move(Vector2 velocity)
        {
            _movement.StartMove(velocity);
        }

        public void Jump()
        {
            _movement.Jump();
        }
        
        public void FlipXDirection()
        {
            _movement.FlipX();
        }
        
    }

    public class MoonBunnyCollider
    {
        [Flags]
        public enum Direciton
        {
            None  = 0,
            Up    = 0x10,
            Down  = 0x100,
            Left  = 0x1000,
            Right = 0x10000,
        }
        [Serializable]
        public class ColliderLayerMask          
        {
            public Collider2D Collider;
            public LayerMask LayerMask;
            public Direciton Direciton;
        }

        private MoonBunnyMovement _movement;
        private Collider2D _collider;
        private LayerMask _targetLayer;
        private Direciton _direciton;

        public MoonBunnyCollider(MoonBunnyMovement movement, ColliderLayerMask colliderLayerMask) : this(colliderLayerMask.Collider, colliderLayerMask.LayerMask, colliderLayerMask.Direciton)
        {
            _movement = movement;
        }

        public MoonBunnyCollider(Collider2D collider, LayerMask layerMask, Direciton direciton)
        {
            _collider = collider;
            _targetLayer = layerMask;
            _direciton = direciton;
        }
        
        public bool IsColliding(out Collision collision)
        {
            return CheckCollisionOn(_collider.transform.position, out collision);
        }

        public bool WillCollide(Vector2 velocity, float time, out Collision collision)
        {
            return CheckCollisionOn((Vector2)_collider.transform.position + velocity * time, out collision);
        }

        private bool CheckCollisionOn(Vector2 position, out Collision collision)
        {
            Collider2D other = null;
            collision = null;

            if (_collider is BoxCollider2D boxCollider2D)
            {
                other = Physics2D.OverlapBox(position + boxCollider2D.offset, boxCollider2D.size, 0, _targetLayer);

            } else if (_collider is CircleCollider2D circleCollider2D)
            {
                other = Physics2D.OverlapCircle(position + circleCollider2D.offset, circleCollider2D.radius, _targetLayer);
            }
   
            if (other == null)
            {
                return false;
            }

            bool directionCorrect = false;
           
            if ((_direciton & Direciton.Down) > 0)
            {
                if (other.transform.position.y < _collider.transform.position.y + _collider.offset.y && _movement.Velocity.y < 0)
                {
                    Debug.Log("Rigidbody DirectionCheck Down");
                    directionCorrect = true;
                }
            }
            
            if ((_direciton & Direciton.Up) > 0)
            {
                if (other.transform.position.y > _collider.transform.position.y + _collider.offset.y && _movement.Velocity.y > 0)
                {
                    Debug.Log("Rigidbody DirectionCheck Up");
                    directionCorrect = true;
                }
            }
            
            if ((_direciton & Direciton.Left) > 0)
            {
                if (other.transform.position.x < _collider.transform.position.x + _collider.offset.x && _movement.Velocity.x < 0)
                {
                    Debug.Log("Rigidbody DirectionCheck Left");
                    directionCorrect = true;
                }
            }
            
            if ((_direciton & Direciton.Right) > 0)
            {
                if (other.transform.position.x > _collider.transform.position.x + _collider.offset.x && _movement.Velocity.x > 0)
                {
                    Debug.Log("Rigidbody DirectionCheck Right");
                    directionCorrect = true;
                }
            }

            if (!directionCorrect)
            {
                return false;
            }

            GridObject gridObject;
            if (other.TryGetComponent<GridObject>(out gridObject))
            {
                collision = null;
                
                if (gridObject is Item item)
                {
                    collision = new ItemCollision(item);
                } else if (gridObject is Obstacle obstacle)
                {
                    collision = new ObstacleCollision(obstacle);
                } else if (gridObject is Platform platform)
                {
                    collision = new PlatformCollision(platform);
                } else if (gridObject is Gimmick gimmick)
                {
                    collision = new GimmickCollision(gimmick);
                }

                return true;
            }

            return true;
        }
    }

    public class MoonBunnyMovement
    {
        public Vector2 Velocity;

        public Vector2Int GridVelocity;

        private GridTransform _transform;

        private Vector2 _lastPosition;

        private bool isPaused;
        
        private MoonBunnyGravity _gravity;

        public MoonBunnyMovement(GridTransform transform, float gravity)
        {
            _transform = transform;
            _lastPosition = _transform.transform.position;
            _gravity = new MoonBunnyGravity(this, gravity);
        }
        
        public void UpdateGravity(float deltaTime)
        {
            _gravity.Update(deltaTime);
        }

        public void UpdateMove(float deltaTime)
        {
            if (isPaused) return;

            _transform.position = new Vector3(_lastPosition.x + deltaTime * Velocity.x, _lastPosition.y + deltaTime * Velocity.y, _transform.transform.position.z);
            _lastPosition = _transform.position;
        }
        
        public void StartMove(float x, float y)
        {
            Velocity = new Vector2(x, y);
        }

        public void StartMove(Vector2 velocity)
        {
            StartMove(velocity.x, velocity.y);
        }

        public void Jump()
        {
            StartMove(GridTransform.GetVelocityByGrid(GridVelocity.x, GridVelocity.y, _gravity.GravityScale));
        }

        public void StopMove()
        {
            Velocity = Vector2.zero;
        }

        public void PauseMove()
        {
            isPaused = true;
        }

        public void UnpauseMove()
        {
            isPaused = false;
        }
        
        public void FlipX()
        {
            StartMove(-Velocity.x, Velocity.y);
        }
        
        public void Collide(Collision collision)
        {
            if (collision == null) return;
            
            if (collision is PlatformCollision platformCollision)
            {
                if (platformCollision.Platform is BouncyPlatform bouncyPlatform)
                {
                    var gridVel = GridTransform.GetVelocityByGrid(GridVelocity.x, GridVelocity.y + bouncyPlatform.JumpPower, _gravity.GravityScale);
                    StartMove(Velocity.x, gridVel.y);
                } else if (platformCollision.Platform is SideWall sideWall)
                {
                    FlipX();
                } else if (platformCollision.Platform is Platform platform)
                {
                    StopMove();
                } 
            } else if (collision is ObstacleCollision obstacleCollision)
            {
                if (obstacleCollision.Obstacle is PinWheel pinWheel)
                {
                    float xVelocity = Velocity.x;
                    float yVelocity = Velocity.y;

                    if ((pinWheel.transform.position.x - _lastPosition.x) * xVelocity > 0)
                    {
                        xVelocity = -xVelocity;
                    }
                    if ((pinWheel.transform.position.y - _lastPosition.y) * yVelocity > 0)
                    {
                        yVelocity = -yVelocity;
                    }

                    Velocity = new Vector2(xVelocity, yVelocity);
                }
            }
        }
    }

    public class MoonBunnyGravity
    {
        private MoonBunnyMovement _influencingMovement;
        
        public float GravityScale;

        public MoonBunnyGravity(MoonBunnyMovement movement)
        {
            _influencingMovement = movement;
        }
        
        public MoonBunnyGravity(MoonBunnyMovement movement, float gravity) : this(movement)
        {
            GravityScale = gravity;
        }

        public void Update(float time)
        {
            _influencingMovement.Velocity = new Vector2(_influencingMovement.Velocity.x, _influencingMovement.Velocity.y - GravityScale * time);
        }
    }
}