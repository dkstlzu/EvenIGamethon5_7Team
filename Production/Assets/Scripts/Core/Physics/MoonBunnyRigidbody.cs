using System;
using System.Collections.Generic;
using System.Linq;
using dkstlzu.Utility;
using MoonBunny.Dev;
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
        public bool isGrounded;

        public Vector2 Velocity => _movement.Velocity;
        public bool isMovingToRight => _movement.Velocity.x > 0;
        public bool isMovingToLeft => _movement.Velocity.x < 0;
        public bool isFalling => _movement.Velocity.y < 0;
            
        private List<MoonBunnyCollider> _colliderList;
        [SerializeField] private MoonBunnyMovement _movement;
        private bool _canDestroyObstaclesByStepping;

        public bool CanDestroyObstaclesByStepping
        {
            get => _canDestroyObstaclesByStepping;
            set
            {
                _canDestroyObstaclesByStepping = value;
                _movement.CanDestroyObstacleByStepping = value;
            }
        }
        

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
            _movement = new MoonBunnyMovement(GridObject.GridTransform, Gravity);
            _movement.CanDestroyObstacleByStepping = CanDestroyObstaclesByStepping;
            _movement.StartMove(serializedInitialVelocity);
            
            _colliderList = new List<MoonBunnyCollider>();
            
            foreach (var colliderLayer in ColliderLayerList)
            {
                _colliderList.Add(new MoonBunnyCollider(this, _movement, colliderLayer));
            }
            
            S_RigidbodyList.Add(this);
        }

        public void FixedUpdate()
        {
            if (!enabled) return;
            
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

            isGrounded = false;
                
            foreach (Collision stayCollision in IntersectWith(_currentCollision, _previousCollision))
            {
                if (stayCollision is PlatformCollision)
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
            foreach (MoonBunnyCollider collider in _colliderList)
            {
                collider.SetIgnore(ignore);
            }
        }

        public void DontIgnoreCollision(LayerMask dontIgnore)
        {
            foreach (MoonBunnyCollider collider in _colliderList)
            {
                collider.DontIgnore(dontIgnore);
            }
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
        private LayerMask _ignoreLayer;
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
                other = Physics2D.OverlapBox(position + boxCollider2D.offset, boxCollider2D.size, 0, _targetLayer & ~_ignoreLayer);

            } else if (_collider is CircleCollider2D circleCollider2D)
            {
                other = Physics2D.OverlapCircle(position + circleCollider2D.offset, circleCollider2D.radius, _targetLayer & ~_ignoreLayer);
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

                if (fieldObject is Platform platform)
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


        public void SetIgnore(LayerMask ignoreLayerMask)
        {
            _ignoreLayer |= ignoreLayerMask;
        }

        public void DontIgnore(LayerMask dontIgnoreLayerMask)
        {
            _ignoreLayer &= ~dontIgnoreLayerMask;
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
        
        private MoonBunnyGravity _gravity;

        public bool CanDestroyObstacleByStepping { get; set; }

        public MoonBunnyMovement(GridTransform transform, float gravity)
        {
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
        
        public void Collide(Collision collision)
        {
            if (collision == null) return;
            
            if (collision is PlatformCollision platformCollision)
            {
                StopMove();
                return;
            }

            if (CanDestroyObstacleByStepping && collision.Other is Obstacle obstacle)
            {
                float relativeSpeedByHeight = GridTransform.GetVelocityByRelativeHeight(-Velocity.y, _gravity.GravityValue, Bounciness);
                Vector2Int fallingGridVelocity = GridTransform.GetGridByVelocity(0, relativeSpeedByHeight, _gravity.GravityValue);

                int targetGridVelocityX = Velocity.x >= 0 ? DefaultHorizontalSpeedOnCollide : -DefaultHorizontalSpeedOnCollide;
                int targetGridVelocityY = fallingGridVelocity.y;

                Vector2Int bouncyGridVelocity =  new Vector2Int(targetGridVelocityX, Mathf.Min(targetGridVelocityY, MaxGridVelocityY));
                    
                var gridVel = GridTransform.GetVelocityByGrid(bouncyGridVelocity, _gravity.GravityValue);
                
                StartMove(gridVel);
                return;
            }

            switch (collision.Other)
            {
                case BouncyPlatform bouncyPlatform:
                    float relativeSpeedByHeight = GridTransform.GetVelocityByRelativeHeight(-Velocity.y, _gravity.GravityValue, Bounciness);
                    Vector2Int fallingGridVelocity = GridTransform.GetGridByVelocity(0, relativeSpeedByHeight, _gravity.GravityValue);

                    int targetGridVelocityX = Velocity.x >= 0 ? DefaultHorizontalSpeedOnCollide : -DefaultHorizontalSpeedOnCollide;
                    int targetGridVelocityY = fallingGridVelocity.y + bouncyPlatform.JumpPower;

                    Vector2Int bouncyGridVelocity =  new Vector2Int(targetGridVelocityX, Mathf.Min(targetGridVelocityY, MaxGridVelocityY));
                    
                    var gridVel = GridTransform.GetVelocityByGrid(bouncyGridVelocity, _gravity.GravityValue);
                    
                    // MoonBunnyLog.print($"Velocity {Velocity}, relativeSpeed {relativeSpeedByHeight}, falling {fallingGridVelocity}, targetx {targetGridVelocityX},\n" +
                    //                    $" targety {targetGridVelocityY}, bouncyVel {bouncyGridVelocity}, gridVel {gridVel}");
                    
                    StartMove(gridVel);
                    break;
                case PinWheel pinWheel:
                case Bee bee:
                case ShootingStar shootingStar:
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
                    break;
                case SideWall: 
                    FlipX();
                    break;
            }
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