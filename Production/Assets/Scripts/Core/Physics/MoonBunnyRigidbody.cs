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
            
            _colliderList = new List<MoonBunnyCollider>();
            
            foreach (var colliderLayer in ColliderLayerList)
            {
                _colliderList.Add(new MoonBunnyCollider(colliderLayer));
            }
            
            _movement = new MoonBunnyMovement(GridObject.GridTransform, Gravity);
            _movement.GridVelocity = GridJumpVelocity;
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
                    collision.OnCollision();
                    _movement.Collide(collision);
                }
            }
            
            _movement.UpdateMove(deltaTime);
        }

        public void Move(Vector2 velocity)
        {
            _movement.StartMove(velocity);
        }
    }

    public class MoonBunnyCollider
    {
        [Serializable]
        public class ColliderLayerMask
        {
            public Collider2D Collider;
            public LayerMask LayerMask;
        }

        private Collider2D _collider;
        private LayerMask _targetLayer;

        public MoonBunnyCollider(ColliderLayerMask colliderLayerMask) : this(colliderLayerMask.Collider, colliderLayerMask.LayerMask)
        {
        }

        public MoonBunnyCollider(Collider2D collider, LayerMask layerMask)
        {
            _collider = collider;
            _targetLayer = layerMask;
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

            if (_collider is BoxCollider2D boxCollider2D)
            {
                other = Physics2D.OverlapBox(  position + boxCollider2D.offset, boxCollider2D.size, 0, _targetLayer);

            } else if (_collider is CircleCollider2D circleCollider2D)
            {
                other = Physics2D.OverlapCircle(position + circleCollider2D.offset, circleCollider2D.radius, _targetLayer);
            }
   
            if (other == null)
            {
                collision = null;
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
                } else if (gridObject is BouncyPlatform bouncyPlatform)
                {
                    collision = new BouncyPlatformCollision(bouncyPlatform);
                } else if (gridObject is Ricecake ricecake)
                {
                    collision = new RicecakeCollision(ricecake);
                }

                return true;
            }

            collision = null;
            return false;
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
            _lastPosition = _transform.position;
            _gravity = new MoonBunnyGravity(this, gravity);
        }
        
        public void UpdateGravity(float deltaTime)
        {
            _gravity.Update(deltaTime);
        }

        public void UpdateMove(float deltaTime)
        {
            if (isPaused) return;

            _transform.position = new Vector2(_lastPosition.x + deltaTime * Velocity.x, _lastPosition.y + deltaTime * Velocity.y);
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
        
        public void Collide(Collision collision)
        {
            if (collision == null) return;
            
            if (collision is BouncyPlatformCollision bouncyPlatformCollision)
            {
                Velocity = GridTransform.GetVelocityByGrid(GridVelocity.x, GridVelocity.y + bouncyPlatformCollision.Platform.JumpPower, _gravity.GravityScale);
            } else if (collision is ObstacleCollision obstacleCollision)
            {
                
            }
        }

        private void PlayerCollide(Collision collision)
        {
            
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