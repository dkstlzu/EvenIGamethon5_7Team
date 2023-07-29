using System;
using System.Collections.Generic;
using System.Linq;
using dkstlzu.Utility;
using MoonBunny.Dev;
using UnityEngine;
using UnityEngine.Serialization;

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
        public LayerMask IgnoringLayerMask;
        public bool isGrounded;

        public Vector2 Velocity => _movement.Velocity;
        public bool isMovingToRight => _movement.Velocity.x > 0;
        public bool isMovingToLeft => _movement.Velocity.x < 0;
        public bool isFalling => _movement.Velocity.y < 0;
            
        private List<MoonBunnyCollider> _colliderList;
        [SerializeField] private MoonBunnyMovement _movement;

        public bool CanDestroyObstaclesByStepping { get; set; }

        private List<Collision> _currentCollision = new List<Collision>();
        private List<Collision> _previousCollision = new List<Collision>();
        private bool enabled = true;

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

            Vector2 serializedInitialVelocity = _movement.Velocity;
            _movement = new MoonBunnyMovement(this, GridObject.GridTransform, Gravity);
            _movement.StartMove(serializedInitialVelocity);
            
            _colliderList = new List<MoonBunnyCollider>();
            
            foreach (var colliderLayer in ColliderLayerList)
            {
                var collider = new MoonBunnyCollider(this, _movement, colliderLayer);
                collider.SetIgnore(IgnoringLayerMask);
                _colliderList.Add(collider);
            }
            
            S_RigidbodyList.Add(this);
        }

        private void OnDestroy()
        {
            S_RigidbodyList.Remove(this);
        }

        public void FixedUpdate()
        {
            if (!enabled) return;
            
            float deltaTime = Time.deltaTime;
            _movement.UpdateGravity(deltaTime * MoveSacle);
            
            // Collision Check
            _currentCollision.Clear();

            Collision[] collisions;

            for (int i = 0; i < _colliderList.Count; i++)
            {
                if (_colliderList[i].IsColliding(out collisions))
                {
                    for (int j = 0; j < collisions.Length; j++)
                    {
                        _currentCollision.Add(collisions[j]);
                    }
                }
            }

            foreach (Collision enterCollision in ExceptWith(_currentCollision, _previousCollision))
            {
                enterCollision.OnCollision();
                _movement.Collide(enterCollision);
                OnEnterCollision?.Invoke(enterCollision);
            }

            isGrounded = false;
                
            foreach (Collision stayCollision in IntersectWith(_currentCollision, _previousCollision))
            {
                if (stayCollision is BlockCollision)
                {
                    isGrounded = true;
                }
                _movement.Collide(stayCollision);
            }
            
            _previousCollision = new List<Collision>(_currentCollision);
            
            // Movement
            _movement.UpdateMove(deltaTime * MoveSacle);
        }

        private Collision[] ExceptWith(IEnumerable<Collision> from, IEnumerable<Collision> with)
        {
            HashSet<Collision> temp = new HashSet<Collision>(from);
            
            temp.ExceptWith(with);

            return temp.ToArray();
        }

        private Collision[] IntersectWith(IEnumerable<Collision> first, IEnumerable<Collision> second)
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
            float previousMoveScale = MoveSacle;
            
            if (time > 0)
            {
                CoroutineHelper.Delay(() =>
                {
                    MoveSacle = previousMoveScale;
                }, time);
            }
            
            MoveSacle = delta;
        }

        public void SetDefaultHorizontalSpeed(int gridSpeed)
        {
            _movement.DefaultHorizontalSpeedOnCollide = gridSpeed;
        }

        public void SetBounciness(float bouncy)
        {
            _movement.Bounciness = bouncy;
        }
        
        public void Move(Vector2 velocity)
        {
            _movement.StartMove(velocity);
        }

        public void Move(Vector2Int gridVelocity)
        {
            _movement.StartMove(GridTransform.GetVelocityByGrid(gridVelocity, Gravity));
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

        [ContextMenu("Enable")]
        public void Enable()
        {
            enabled = true;
        }

        [ContextMenu("Disable")]
        public void Disable()
        {
            ForcePosition();
            enabled = false;
        }

        [ContextMenu("Force Position")]
        public void ForcePosition()
        {
            _movement.ForcePosition(transform.position);
        }


        public void IgnoreCollision(LayerMask ignore)
        {
            IgnoringLayerMask |= ignore;
            
            foreach (MoonBunnyCollider collider in _colliderList)
            {
                collider.SetIgnore(IgnoringLayerMask);
            }
        }

        public void DontIgnoreCollision(LayerMask dontIgnore)
        {
            IgnoringLayerMask &= ~dontIgnore;
            
            foreach (MoonBunnyCollider collider in _colliderList)
            {
                collider.SetIgnore(IgnoringLayerMask);
            }
        }

        public bool IsIgnoring(LayerMask target)
        {
            return (target & IgnoringLayerMask) > 0;
        }
    }

    public class MoonBunnyCollider
    {
        [Flags]
        public enum Direction
        {
            None  = 0b0000,
            Up    = 0b0001,
            Down  = 0b0010,
            Left  = 0b0100,
            Right = 0b1000,
        }
        [Serializable]
        public class ColliderLayerMask          
        {
            public Collider2D Collider;
            public LayerMask LayerMask;
            [FormerlySerializedAs("Direciton")] public Direction Direction;
        }

        private MoonBunnyRigidbody _rigidbody;
        private MoonBunnyMovement _movement;
        private Collider2D _collider;
        private LayerMask _targetLayer;
        private LayerMask _ignoreLayer;
        private Direction _direction;

        private bool _enabled = true;
        public bool enabled => _enabled;
        
        public MoonBunnyCollider(MoonBunnyRigidbody rigidbody, MoonBunnyMovement movement, ColliderLayerMask colliderLayerMask) : this(colliderLayerMask.Collider, colliderLayerMask.LayerMask, colliderLayerMask.Direction)
        {
            _movement = movement;
            _rigidbody = rigidbody;
        }

        public MoonBunnyCollider(Collider2D collider, LayerMask layerMask, Direction direction)
        {
            _collider = collider;
            _targetLayer = layerMask;
            _direction = direction;
        }
        
        public bool IsColliding(out Collision[] collisions)
        {
            return CheckCollisionOn(_collider.transform.position, out collisions);
        }

        public bool WillCollide(Vector2 velocity, float time, out Collision[] collisions)
        {
            return CheckCollisionOn((Vector2)_collider.transform.position + velocity * time, out collisions);
        }

        private bool CheckCollisionOn(Vector2 position, out Collision[] collisions)
        {
            if (!enabled)
            {
                collisions = null;
                return false;
            }
            
            Vector2 targetPosition = position + _collider.offset;

            Collider2D[] others = null;
            collisions = null;

            if (_collider is BoxCollider2D boxCollider2D)
            {
                others = Physics2D.OverlapBoxAll(position + boxCollider2D.offset, boxCollider2D.size, 0, _targetLayer & ~_ignoreLayer);

            } else if (_collider is CircleCollider2D circleCollider2D)
            {
                others = Physics2D.OverlapCircleAll(position + circleCollider2D.offset, circleCollider2D.radius, _targetLayer & ~_ignoreLayer);
            }
   
            if (others == null)
            {
                return false;
            }
            
            List<Collision> collisionList = new List<Collision>();

            for (int i = 0; i < others.Length; i++)
            {
                bool directionCorrect = false;
                Direction collisionDirection = Direction.None;
                Vector3 otherPosition = others[i].transform.position + (Vector3)others[i].offset;
                
                if ((_direction & Direction.Down) > 0)
                {
                    if (otherPosition.y < targetPosition.y && _movement.Velocity.y < 0)
                    {
                        directionCorrect = true;
                        collisionDirection |= Direction.Down;
                    }
                }
                
                if ((_direction & Direction.Up) > 0)
                {
                    if (otherPosition.y > targetPosition.y && _movement.Velocity.y > 0)
                    {
                        directionCorrect = true;
                        collisionDirection |= Direction.Up;
                    }
                }
                
                if ((_direction & Direction.Left) > 0)
                {
                    if (otherPosition.x < targetPosition.x && _movement.Velocity.x < 0)
                    {
                        directionCorrect = true;
                        collisionDirection |= Direction.Left;
                    }
                }
                
                if ((_direction & Direction.Right) > 0)
                {
                    if (otherPosition.x > targetPosition.x && _movement.Velocity.x > 0)
                    {
                        directionCorrect = true;
                        collisionDirection |= Direction.Right;
                    }
                }

                if (!directionCorrect)
                {
                    return false;
                }

                ICollidable collidable;
                if (others[i].TryGetComponent<ICollidable>(out collidable))
                {
                    Collision[] doubleChecker = collidable.Collide(_rigidbody, collisionDirection);
                    if (doubleChecker == null)
                    {
                        doubleChecker = Array.Empty<Collision>();
                    }
                    collisionList.AddRange(doubleChecker);
                    // if (_rigidbody.CanDestroyObstaclesByStepping && fieldObject is Obstacle obstacle)
                    // {
                    //     if ((collisionDirection & Direciton.Down) > 0)
                    //     {
                    //         collisionList.Add(new DirectionCollision(new DirectionCollision.DirectionCollisionArgs(collisionDirection, _rigidbody.GridObject, obstacle)));
                    //     }
                    //     else
                    //     {
                    //         collisionList.Add(new GimmickCollision(new GimmickCollision.GimmickCollisionArgs(_rigidbody, _rigidbody.GridObject, obstacle)));
                    //     }
                    // } else if (fieldObject is Platform platform)
                    // {
                    //     collisionList.Add(new PlatformCollision(new PlatformCollision.PlatformCollisionArgs((Character)_rigidbody.GridObject, platform)));
                    //     collisionList.Add(new GimmickCollision(new GimmickCollision.GimmickCollisionArgs(_rigidbody, _rigidbody.GridObject, platform)));
                    // } else if (fieldObject is Gimmick gimmick)
                    // {
                    //     collisionList.Add(new GimmickCollision(new GimmickCollision.GimmickCollisionArgs(_rigidbody, _rigidbody.GridObject, gimmick)));
                    // } else
                    // {
                    //     collisionList.Add(new Collision(new Collision.CollisionArgs(_rigidbody.GridObject, fieldObject)));
                    // }
                }
            }

            collisions = collisionList.ToArray();

            if (collisions.Length > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Enable()
        {
            _enabled = true;
        }

        public void Disable()
        {
            _enabled = false;
        }


        public void SetIgnore(LayerMask ignoreLayerMask)
        {
            _ignoreLayer = ignoreLayerMask;
        }
    }

    [Serializable]
    public class MoonBunnyMovement
    {
        public Vector2 Velocity;

        [HideInInspector] public int DefaultHorizontalSpeedOnCollide = 1;
        [HideInInspector] public float Bounciness = 1f/3;

        private static int MaxGridVelocityY = 10;
        
        private GridTransform _transform;

        private Vector2 _lastPosition;

        private bool enabled = true;

        private MoonBunnyRigidbody _rigidbody;
        private MoonBunnyGravity _gravity;

        public bool CanDestroyObstacleByStepping => _rigidbody.CanDestroyObstaclesByStepping;

        public MoonBunnyMovement(MoonBunnyRigidbody rigidbody, GridTransform transform, float gravity)
        {
            _rigidbody = rigidbody;
            _transform = transform;
            _lastPosition = _transform.transform.position;
            _gravity = new MoonBunnyGravity(this, gravity);
        }
        
        public void UpdateGravity(float deltaTime)
        {
            if (!enabled) return;

            _gravity.Update(deltaTime);
        }

        public void UpdateMove(float deltaTime)
        {
            if (!enabled) return;

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

        public void StopMove()
        {
            Velocity = Vector2.zero;
        }

        public void PauseMove()
        {
            enabled = false;
            _gravity.Disable();
        }

        public void UnpauseMove()
        {
            enabled = true;
            _gravity.Enable();
        }
        
        public void FlipX()
        {
            StartMove(-Velocity.x, Velocity.y);
        }
        
        public void FlipY()
        {
            StartMove(Velocity.x, -Velocity.y);
        }
        
        public void Collide(Collision collision)
        {
            if (collision == null) return;
            
            if (collision is BlockCollision)
            {
                StopMove();
                return;
            } else if (collision is BounceCollision bounceCollision)
            {
                float xVelocity = Velocity.x;
                float yVelocity = Velocity.y;
                
                if ((collision.Other.transform.position.x - _lastPosition.x) * xVelocity > 0)
                {
                    xVelocity = -xVelocity;
                }
                if ((collision.Other.transform.position.y - _lastPosition.y) * yVelocity > 0)
                {
                    yVelocity = -yVelocity;
                }
                
                StartMove(new Vector2(xVelocity, yVelocity));
            } else if (collision is FlipCollision flipCollision)
            {
                if (flipCollision.isVertical)
                {
                    FlipY();
                }
                else
                {
                    FlipX();
                }
            } else if (collision is BouncyPlatformCollision bouncyPlatformCollision)
            {
                float relativeSpeedByHeight = GridTransform.GetVelocityByRelativeHeight(-Velocity.y, _gravity.GravityValue, Bounciness);
                Vector2Int fallingGridVelocity = GridTransform.GetGridByVelocity(0, relativeSpeedByHeight, _gravity.GravityValue);
                
                int targetGridVelocityX = Velocity.x >= 0 ? DefaultHorizontalSpeedOnCollide : -DefaultHorizontalSpeedOnCollide;
                int targetGridVelocityY = fallingGridVelocity.y + bouncyPlatformCollision.BouncyPlatform.JumpPower;
                
                int calculatedVelocityY = Mathf.Min(targetGridVelocityY, MaxGridVelocityY);
                int adjustedWithMunimumVelocityY = Mathf.Max(calculatedVelocityY, 1);
                
                Vector2Int bouncyGridVelocity =  new Vector2Int(targetGridVelocityX, adjustedWithMunimumVelocityY);
                
                var gridVel = GridTransform.GetVelocityByGrid(bouncyGridVelocity, _gravity.GravityValue);
                
                // MoonBunnyLog.print($"Velocity {Velocity}, relativeSpeed {relativeSpeedByHeight}, falling {fallingGridVelocity}, targetx {targetGridVelocityX},\n" +
                //                    $" targety {targetGridVelocityY}, bouncyVel {bouncyGridVelocity}, gridVel {gridVel}");
                
                StartMove(gridVel);
            }

            // if (collision is DirectionCollision directionCollision && CanDestroyObstacleByStepping && collision.Other is Obstacle)
            // {
            //     if ((directionCollision.Direciton & MoonBunnyCollider.Direciton.Down) > 0)
            //     {
            //         float relativeSpeedByHeight = GridTransform.GetVelocityByRelativeHeight(-Velocity.y, _gravity.GravityValue, Bounciness);
            //         Vector2Int fallingGridVelocity = GridTransform.GetGridByVelocity(0, relativeSpeedByHeight, _gravity.GravityValue);
            //
            //         int targetGridVelocityX = Velocity.x >= 0 ? DefaultHorizontalSpeedOnCollide : -DefaultHorizontalSpeedOnCollide;
            //         int targetGridVelocityY = fallingGridVelocity.y;
            //     
            //         int calculatedVelocityY = Mathf.Min(targetGridVelocityY, MaxGridVelocityY);
            //         int adjustedWithMunimumVelocityY = Mathf.Max(calculatedVelocityY, 1);
            //     
            //         Vector2Int bouncyGridVelocity =  new Vector2Int(targetGridVelocityX, adjustedWithMunimumVelocityY);
            //         
            //         var gridVel = GridTransform.GetVelocityByGrid(bouncyGridVelocity, _gravity.GravityValue);
            //     
            //         StartMove(gridVel);
            //         return;
            //     }
            // }
            //
            // switch (collision.Other)
            // {
            //     case BouncyPlatform bouncyPlatform:
            //         float relativeSpeedByHeight = GridTransform.GetVelocityByRelativeHeight(-Velocity.y, _gravity.GravityValue, Bounciness);
            //         Vector2Int fallingGridVelocity = GridTransform.GetGridByVelocity(0, relativeSpeedByHeight, _gravity.GravityValue);
            //
            //         int targetGridVelocityX = Velocity.x >= 0 ? DefaultHorizontalSpeedOnCollide : -DefaultHorizontalSpeedOnCollide;
            //         int targetGridVelocityY = fallingGridVelocity.y + bouncyPlatform.JumpPower;
            //
            //         int calculatedVelocityY = Mathf.Min(targetGridVelocityY, MaxGridVelocityY);
            //         int adjustedWithMunimumVelocityY = Mathf.Max(calculatedVelocityY, 1);
            //
            //         Vector2Int bouncyGridVelocity =  new Vector2Int(targetGridVelocityX, adjustedWithMunimumVelocityY);
            //         
            //         var gridVel = GridTransform.GetVelocityByGrid(bouncyGridVelocity, _gravity.GravityValue);
            //         
            //         // MoonBunnyLog.print($"Velocity {Velocity}, relativeSpeed {relativeSpeedByHeight}, falling {fallingGridVelocity}, targetx {targetGridVelocityX},\n" +
            //         //                    $" targety {targetGridVelocityY}, bouncyVel {bouncyGridVelocity}, gridVel {gridVel}");
            //         
            //         StartMove(gridVel);
            //         break;
            //     case PinWheel pinWheel:
            //     case Bee bee:
            //     case ShootingStar shootingStar:
            //         float xVelocity = Velocity.x;
            //         float yVelocity = Velocity.y;
            //         
            //         if ((collision.Other.transform.position.x - _lastPosition.x) * xVelocity > 0)
            //         {
            //             xVelocity = -xVelocity;
            //         }
            //         if ((collision.Other.transform.position.y - _lastPosition.y) * yVelocity > 0)
            //         {
            //             yVelocity = -yVelocity;
            //         }
            //
            //         StartMove(new Vector2(xVelocity, yVelocity));
            //         break;
            //     case SideWall: 
            //         FlipX();
            //         break;
            // }
        }

        public void ChangeGravity(float gravity)
        {
            _gravity.GravityValue = gravity;
        }

        public void ForcePosition(Vector3 position)
        {
            _lastPosition = position;
            _transform.position = position;
        }
    }

    public class MoonBunnyGravity
    {
        private MoonBunnyMovement _influencingMovement;
        
        public float GravityValue;

        private bool enabled = true;
        
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
            if (!enabled) return;
            
            _influencingMovement.StartMove(new Vector2(_influencingMovement.Velocity.x, _influencingMovement.Velocity.y - GravityValue * time));
        }

        public void Enable()
        {
            enabled = true;
        }

        public void Disable()
        {
            enabled = false;
        }
    }
}