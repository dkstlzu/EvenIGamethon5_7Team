﻿using System;
using System.Collections.Generic;
using System.Linq;
using dkstlzu.Utility;
using UnityEngine;

namespace MoonBunny
{
    public class MoonBunnyRigidbody : MonoBehaviour
    {
        public static List<MoonBunnyRigidbody> S_RigidbodyList = new List<MoonBunnyRigidbody>();

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void ResetList()
        {
            S_RigidbodyList = new List<MoonBunnyRigidbody>();
        }

        public static void EnableAll()
        {
            foreach (MoonBunnyRigidbody rigidbody in S_RigidbodyList)
            {
                rigidbody.UnpauseMove();
                rigidbody.EnableCollision();
            }
        }

        public static void DisableAll()
        {
            foreach (MoonBunnyRigidbody rigidbody in S_RigidbodyList)
            {
                rigidbody.PauseMove();
                rigidbody.DisableCollision();
            }
        }
        
        public Vector2Int GridJumpVelocity;
        [SerializeField] private float _gravity;
        public float Gravity
        {
            get => _gravity;
            set
            {
                _gravity = value;
                _movement.ChangeGravity(_gravity);
            }
        }

        [Range(-10, 10)] public float MoveSacle = 1;

        public GridObject GridObject;
        public List<MoonBunnyCollider.ColliderLayerMask> ColliderLayerList;

        public Vector2 Velocity => _movement.Velocity;
        public bool isMovingToRight => _movement.Velocity.x > 0;
        public bool isMovingToLeft => _movement.Velocity.x < 0;
        public bool isFalling => _movement.Velocity.y < 0;
            
        private List<MoonBunnyCollider> _colliderList;
        private MoonBunnyMovement _movement;

        private HashSet<Collision> _currentCollision = new HashSet<Collision>();
        private HashSet<Collision> _previousCollision = new HashSet<Collision>();

        public event Action<Collision> OnEnterCollision;

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
                _colliderList.Add(new MoonBunnyCollider(this, _movement, colliderLayer));
            }
            
            S_RigidbodyList.Add(this);
        }

        public void Update()
        {
            float deltaTime = Time.deltaTime;
            _movement.UpdateGravity(deltaTime * MoveSacle);
            
            // Collision Check
            _currentCollision.Clear();

            Collision collision;

            foreach (MoonBunnyCollider collider in _colliderList)
            {
                // if (collider.WillCollide(_movement.Velocity, deltaTime, out collision))
                if (collider.IsColliding(out collision))
                {
                    _currentCollision.Add(collision);
                }
            }

            foreach (Collision enterCollision in ExceptWith(_currentCollision, _previousCollision))
            {
                enterCollision.OnCollision();
                _movement.Collide(enterCollision);
                OnEnterCollision?.Invoke(enterCollision);
            }

            foreach (Collision stayCollision in IntersectWith(_currentCollision, _previousCollision))
            {
                _movement.Collide(stayCollision);
            }
            
            _previousCollision = new HashSet<Collision>(_currentCollision);

            
            // Movement
            _movement.UpdateMove(deltaTime * MoveSacle);
        }

        private Collision[] ExceptWith(HashSet<Collision> from, HashSet<Collision> with)
        {
            HashSet<Collision> temp = new HashSet<Collision>(from);
            
            temp.ExceptWith(with);

            return temp.ToArray();
        }

        private Collision[] IntersectWith(HashSet<Collision> first, HashSet<Collision> second)
        {
            HashSet<Collision> temp = new HashSet<Collision>(first);
            
            temp.IntersectWith(second);

            return temp.ToArray();
        }

        public void EnableCollision()
        {
            foreach (MoonBunnyCollider collider in _colliderList)
            {
                collider.Enable();
            }
        }

        public void DisableCollision()
        {
            foreach (MoonBunnyCollider collider in _colliderList)
            {
                collider.Disable();
            }
        }

        public void ChangeDelta(float delta, float time = -1)
        {
            MoveSacle = delta;

            if (time > 0)
            {
                CoroutineHelper.Delay(() =>
                {
                    MoveSacle = 1;
                }, time);
            }
        }
        
        public void Move(Vector2 velocity)
        {
            _movement.StartMove(velocity);
        }

        public void Move(Vector2Int gridVelocity)
        {
            _movement.StartMove(GridTransform.GetVelocityByGrid(gridVelocity, Gravity));
        }

        public void Jump()
        {
            _movement.Jump();
        }
        
        public void StopMove()
        {
            _movement.StopMove();
        }

        public void PauseMove()
        {
            _movement.PauseMove();
        }

        public void UnpauseMove()
        {
            _movement.UnpauseMove();   
        }

        public void ForcePosition(Vector3 position)
        {
            _movement.ForcePosition(position);
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

        private MoonBunnyRigidbody _rigidbody;
        private MoonBunnyMovement _movement;
        private Collider2D _collider;
        private LayerMask _targetLayer;
        private Direciton _direciton;

        private bool _enabled = true;
        public bool enabled => _enabled;
        
        public MoonBunnyCollider(MoonBunnyRigidbody rigidbody, MoonBunnyMovement movement, ColliderLayerMask colliderLayerMask) : this(colliderLayerMask.Collider, colliderLayerMask.LayerMask, colliderLayerMask.Direciton)
        {
            _movement = movement;
            _rigidbody = rigidbody;
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
            if (!enabled)
            {
                collision = null;
                return false;
            }
            
            
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
                    directionCorrect = true;
                }
            }
            
            if ((_direciton & Direciton.Up) > 0)
            {
                if (other.transform.position.y > _collider.transform.position.y + _collider.offset.y && _movement.Velocity.y > 0)
                {
                    directionCorrect = true;
                }
            }
            
            if ((_direciton & Direciton.Left) > 0)
            {
                if (other.transform.position.x < _collider.transform.position.x + _collider.offset.x && _movement.Velocity.x < 0)
                {
                    directionCorrect = true;
                }
            }
            
            if ((_direciton & Direciton.Right) > 0)
            {
                if (other.transform.position.x > _collider.transform.position.x + _collider.offset.x && _movement.Velocity.x > 0)
                {
                    directionCorrect = true;
                }
            }

            if (!directionCorrect)
            {
                return false;
            }

            FieldObject fieldObject;
            if (other.TryGetComponent<FieldObject>(out fieldObject))
            {
                collision = null;
                
                if (fieldObject is Item item)
                {
                    collision = new ItemCollision(_rigidbody, item);
                } else if (fieldObject is Obstacle obstacle)
                {
                    collision = new ObstacleCollision(_rigidbody, obstacle);
                } else if (fieldObject is Platform platform)
                {
                    collision = new PlatformCollision(_rigidbody, platform);
                } else if (fieldObject is Gimmick gimmick)
                {
                    collision = new GimmickCollision(_rigidbody, gimmick);
                } else if (fieldObject is GridObject gridObject)
                {
                    collision = new GridObjectCollision(_rigidbody, gridObject);
                } else
                {
                    collision = new Collision(fieldObject);
                }

                return true;
            }

            return true;
        }

        public void Enable()
        {
            _enabled = true;
        }

        public void Disable()
        {
            _enabled = false;
        }
    }

    public class MoonBunnyMovement
    {
        public Vector2 Velocity;

        public Vector2Int GridVelocity;

        private static int MaxGridVelocityY = 10;
        
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
            StartMove(GridTransform.GetVelocityByGrid(GridVelocity.x, GridVelocity.y, _gravity.GravityValue));
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
                    Vector2Int fallingGridVelocity = GridTransform.GetGridByVelocity(Velocity.x, Velocity.y, _gravity.GravityValue);

                    if (fallingGridVelocity.y > 0) return;

                    int targetVelocityY = -fallingGridVelocity.y / 3 + bouncyPlatform.JumpPower + GridVelocity.y;

                    Vector2Int bouncyGridVelocity =  new Vector2Int(fallingGridVelocity.x, Mathf.Min(targetVelocityY, MaxGridVelocityY));
                    
                    var gridVel = GridTransform.GetVelocityByGrid(bouncyGridVelocity, _gravity.GravityValue);
                    
                    StartMove(Velocity.x, gridVel.y);
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

                    StartMove(new Vector2(xVelocity, yVelocity));
                } else if (obstacleCollision.Obstacle is Bee bee)
                {
                    float xVelocity = Velocity.x;
                    float yVelocity = Velocity.y;
                    
                    if ((bee.transform.position.x - _lastPosition.x) * xVelocity > 0)
                    {
                        xVelocity = -xVelocity;
                    }
                    if ((bee.transform.position.y - _lastPosition.y) * yVelocity > 0)
                    {
                        yVelocity = -yVelocity;
                    }

                    StartMove(new Vector2(xVelocity, yVelocity));
                }
            } else
            {
                if (collision.Other is SideWall sideWall)
                {
                    FlipX();
                }
            }
        }

        public void ChangeGravity(float gravity)
        {
            _gravity.GravityValue = gravity;
        }

        public void ForcePosition(Vector3 position)
        {
            _lastPosition = position;
        }
    }

    public class MoonBunnyGravity
    {
        private MoonBunnyMovement _influencingMovement;
        
        public float GravityValue;

        public MoonBunnyGravity(MoonBunnyMovement movement)
        {
            _influencingMovement = movement;
        }
        
        public MoonBunnyGravity(MoonBunnyMovement movement, float gravity) : this(movement)
        {
            GravityValue = gravity;
        }

        public void Update(float time)
        {
            _influencingMovement.Velocity = new Vector2(_influencingMovement.Velocity.x, _influencingMovement.Velocity.y - GravityValue * time);
        }
    }
}