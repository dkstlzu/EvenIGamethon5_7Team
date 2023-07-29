using System;
using System.Collections;
using System.Collections.Generic;
using dkstlzu.Utility;
using MoonBunny.Dev;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MoonBunny.Effects
{
    public interface IEffect
    {
        void Effect();
    }

    public class MagnetEffect : IEffect
    {
        private CircleCollider2D _target;
        private SpriteRenderer _spriteRenderer;
        private float _magneticPower;
        private float _duration = -1;

        public MagnetEffect(Character target, float magneticPower)
        {
            _target = target.MagneticField;
            _spriteRenderer = target.MagneticField.GetComponentInChildren<SpriteRenderer>();
            _magneticPower = magneticPower;
        }
        
        public MagnetEffect(Character target, float magneticPower, float duration) : this(target, magneticPower)
        {
            _duration = duration;
        }

        public void Effect()
        {
            float previousPower = _target.radius;
            
            _target.radius += _magneticPower;
            _spriteRenderer.transform.localScale = new Vector3(_magneticPower * 2, _magneticPower * 2, 1);

            if (_duration > 0)
            {
                UpdateManager.instance.Delay(() =>
                {
                    _target.radius = previousPower;
                    _spriteRenderer.transform.localScale = new Vector3(previousPower * 2, previousPower * 2, 1);
                }, _duration);
            }
        }
    }

    public class HeartEffect : IEffect
    {
        private Character _target;

        public HeartEffect(Character target)
        {
            _target = target;
        }

        public void Effect()
        {
            _target.GetHeart();
        }
    }

    public class RocketEffect : IEffect
    {
        private static bool S_isEffectOn = false;
        private MoonBunnyRigidbody _rigidbody;
        private float _upSpeed;
        private float _duration;
        
        public RocketEffect(MoonBunnyRigidbody rigidbody, float upSpeed, float duration)
        {
            _rigidbody = rigidbody;
            _upSpeed = upSpeed;
            _duration = duration;
        }
        
        public void Effect()
        {
            if (S_isEffectOn) return;
            
            Vector2 previousVelocity = _rigidbody.Velocity;
            float previousGravity = _rigidbody.Gravity;
            
            _rigidbody.IgnoreCollision(LayerMask.GetMask("Obstacle"));
            _rigidbody.Move(new Vector2(0, _upSpeed));
            _rigidbody.Gravity = 0;
            S_isEffectOn = true;
            
            UpdateManager.instance.Delay(() =>
            {
                _rigidbody.DontIgnoreCollision(LayerMask.GetMask("Obstacle"));
                _rigidbody.Move(previousVelocity);
                _rigidbody.Gravity = previousGravity;
                S_isEffectOn = false;
            }, _duration);
        }
    }
    
    public class StarCandyEffect : IEffect
    {
        public static GameObject S_StarCandyExplosionEffect;
        private static int S_MaxDestroyNumber;
        
        private LayerMask _targetLayerMask;
        private Bounds _area;
        private List<Obstacle> _destroyedObstacles = new List<Obstacle>();
        public Obstacle[] DestroyedObstacles => _destroyedObstacles.ToArray();
        
        public StarCandyEffect(LayerMask targetLayerMask, Bounds area)
        {
            _targetLayerMask = targetLayerMask;
            _area = area;
        }
        
        public void Effect()
        {
            Collider2D[] results = Physics2D.OverlapAreaAll(_area.min, _area.max, _targetLayerMask);

            MonoBehaviour.Instantiate(S_StarCandyExplosionEffect, _area.center, Quaternion.identity);
            
            for (int i = 0; i < results.Length; i++)
            {
                if (results[i] == null) continue;
                
                _destroyedObstacles.Add(results[i].GetComponent<Obstacle>());
                MonoBehaviour.Destroy(results[i].gameObject);
            }
        }
    }

    public class InvincibleEffect : IEffect
    {
        private MoonBunnyRigidbody _target;
        private LayerMask _from;
        private SpriteRenderer _renderer;
        private float _duration;
        private AnimationCurve _curve;

        public InvincibleEffect(MoonBunnyRigidbody rigidbody, LayerMask ignore, SpriteRenderer renderer, float duration, AnimationCurve curve)
        {
            _target = rigidbody;
            _from = ignore;
            _renderer = renderer;
            _duration = duration;
            _curve = curve;
        }

        public void Effect()
        {
            _target.StartCoroutine(Flickering());
        }

        IEnumerator Flickering()
        {
            _target.IgnoreCollision(_from);
            float timer = 0;
            
            Color color = Color.white;
            while (timer < _duration)
            {
                timer += Time.deltaTime;
                color = _renderer.color;
                _renderer.color = new Color(color.r, color.g, color.b, _curve.Evaluate(timer));
                yield return new WaitForEndOfFrame();
            }

            _renderer.color = Color.white;
            _target.DontIgnoreCollision(_from);
        }
    }
    
    public class TransformForceEffect : IEffect, IUpdatable
    {
        public Transform Target;
        public Transform To;

        private TimeUpdatable _timeUpdatable;
        public TransformForceEffect(Transform target, Transform to)
        {
            Target = target;
            To = to;
        }

        public void Update(float delta)
        {
            Target.transform.position = To.position;
        }

        public void Effect()
        {
            _timeUpdatable = new TimeUpdatable(this, 1);
            UpdateManager.instance.Register(_timeUpdatable);
        }

        public void Quit()
        {
            UpdateManager.instance.Unregister(_timeUpdatable);
        }
    }

    public class SlowEffect : IEffect
    {
        public static Sprite SpiderWebDebuffSprite;

        private MoonBunnyRigidbody _target;
        private float _slow;
        private float _duration;
        
        public SlowEffect(MoonBunnyRigidbody target, float slow, float duration)
        {
            _target = target;
            _slow = slow;
            _duration = duration;
        }

        public void Effect()
        {
            _target.ChangeDelta(_slow, _duration);

            Character character;
            if (_target.TryGetComponent(out character))
            {
                character.DebuffSpriteRenderer.sprite = SpiderWebDebuffSprite;
                
                UpdateManager.instance.Delay(() =>
                {
                    character.DebuffSpriteRenderer.sprite = null;
                }, _duration);
            }
        }
    }

    public class ThunderEffect : IEffect
    {
        public static GameObject S_ThunderEffectPrefab;
        private static LayerMask S_characterLayerMask = LayerMask.GetMask("Character");
        private static float S_duration = 3;

        public static event Action<float> OnThunderAttack;

        [RuntimeInitializeOnLoadMethod]
        static void ClearEvents()
        {
            OnThunderAttack = null;
        }

        private int _targetColumn;
        private float _warningDuration;

        private Vector2 areaMin;
        private Vector2 areaMax;
        
        public ThunderEffect(int targetColumn, float warningDuration)
        {
            _targetColumn = targetColumn;
            _warningDuration = warningDuration;
        }

        public void Effect()
        {
            areaMin = GridTransform.ToReal(new Vector2Int(_targetColumn, GridTransform.GridYMin)) - GridTransform.GetGridSize() / 2;
            areaMax = GridTransform.ToReal(new Vector2Int(_targetColumn, GameManager.instance.Stage.Spec.Height)) + GridTransform.GetGridSize() / 2;

            WarningEffect warningEffect = new WarningEffect(new Rect(areaMin, areaMax - areaMin), _warningDuration);
            warningEffect.Effect();
            
            UpdateManager.instance.Delay(() =>
            {
                Vector3 thunderPosition = new Vector3(_targetColumn * GridTransform.GridSetting.GridWidth + GridTransform.OriginInReal.x,
                    GameObject.FindWithTag("Player").transform.position.y, 0);
                MonoBehaviour.Instantiate(S_ThunderEffectPrefab, thunderPosition, Quaternion.identity, GameObject.FindWithTag("Obstacles").transform);
                Attack();
            }, _warningDuration);
        }

        void Attack()
        {
            var result = Physics2D.OverlapArea(areaMin, areaMax, S_characterLayerMask);

            if (result == null)
            {
                return;
            }

            Character character = result.GetComponentInParent<Character>();

            if (character)
            {
                if (character.Rigidbody.IsIgnoring(LayerMask.GetMask("Obstacle"))) return;
                
                character.Hit(null);
                character.isIgnoringFlip = true;
                OnThunderAttack?.Invoke(S_duration);
                
                UpdateManager.instance.Delay(() =>
                {
                    character.isIgnoringFlip = false;
                }, S_duration);
            }
        }
    }

    public class WarningEffect : IEffect
    {
        public static GameObject S_WarningEffectPrefab;
            
        private Rect _area;
        private float _duration;
        
        public WarningEffect(Rect area, float duration)
        {
            _area = area;
            _duration = duration;
        }

        public void Effect()
        {
            Warning warning = MonoBehaviour.Instantiate(S_WarningEffectPrefab, _area.center, Quaternion.identity).GetComponent<Warning>();
            warning.Size = _area.size;
            
            UpdateManager.instance.Delay(() =>
            {
                MonoBehaviour.Destroy(warning);
            }, _duration);
        }
    }

    public class ShootingStarEffect : IEffect
    {
        public static GameObject S_ShootingStarEffectPrefab;
        public static int S_ShootingStarNumber = 5;
        
        private int _targetRow;
        private int _targetColumn;

        private Vector2 areaMin;
        private Vector2 areaMax;
        
        public ShootingStarEffect(int targetRow, int targetColumn)
        {
            _targetRow = targetRow;
            _targetColumn = targetColumn;
            
            areaMin = GridTransform.ToReal(new Vector2Int(_targetColumn - 1, _targetRow)) - GridTransform.GetGridSize() / 2;
            areaMax = GridTransform.ToReal(new Vector2Int(_targetColumn + 1, _targetRow + 1)) + GridTransform.GetGridSize() / 2;
        }

        public void Effect()
        {
            Transform parent = GameObject.FindWithTag("Obstacles").transform;
            for (int i = 0; i < S_ShootingStarNumber; i++)
            {
                Vector3 shootingStarPosition = new Vector3(Random.Range(areaMin.x, areaMax.x), Random.Range(areaMin.y, areaMax.y), 0);
                MonoBehaviour.Instantiate(S_ShootingStarEffectPrefab, shootingStarPosition, Quaternion.identity, parent);
            }
        }
    }

    public class BoostEffect : IEffect
    {
        public virtual void Effect()
        {
            
        }
    }

    public class RocketBoostEffect : BoostEffect
    {
        public const string BoostName = "RocketBoost";
            
        public static float Speed = 10;
        public static float Duration = 6;

        private MoonBunnyRigidbody _rigidbody;

        public RocketBoostEffect(MoonBunnyRigidbody rigidbody)
        {
            _rigidbody = rigidbody;
        }
        
        public override void Effect()
        {
            new RocketEffect(_rigidbody, Speed, Duration).Effect();
        }
    }

    public class StarCandyBoostEffect : BoostEffect, IUpdatable
    {
        public const string BoostName = "StarCandyBoost";

        public static GameObject S_CoinPrefab;
        private static LayerMask _targetLayerMask = LayerMask.GetMask("Obstacle");
        private static int S_effectNumber = 10;
        public static float S_EffectInterval = 1;

        private List<StarCandyEffect> _effects = new List<StarCandyEffect>();
        private TimeUpdatable _timeUpdatable;

        private int _effectNumber;
        private float _effectInterval;
        private Transform _parent;

        public StarCandyBoostEffect(int effectNumber = -1, float interval = -1)
        {
            if (effectNumber > 0) _effectNumber = effectNumber;
            else _effectNumber = S_effectNumber;

            if (interval > 0) _effectInterval = interval;
            else _effectInterval = S_EffectInterval;
            
            for (int i = 0; i < _effectNumber; i++)
            {
                Vector2 center = GridTransform.ToReal(new Vector2Int(0, i * 3 + 1));
                _effects.Add(new StarCandyEffect(_targetLayerMask, new Bounds(center, new Vector2(20, GridTransform.GridSetting.GridHeight * 3))));
            }

            _parent = GameObject.FindWithTag("Items").transform;
        }
        
        public override void Effect()
        {
            _timeUpdatable = new TimeUpdatable(this, 1);
            UpdateManager.instance.Register(_timeUpdatable);
        }

        private float _timer = 0;
        
        public void Update(float delta)
        {
            _timer += delta;

            if (_timer >= _effectInterval)
            {
                if (_effects.Count <= 0)
                {
                    UpdateManager.instance.Unregister(_timeUpdatable);
                    return;
                }
                
                _effects[0].Effect();
                Obstacle[] destroyedObstacles = _effects[0].DestroyedObstacles;
                foreach (Obstacle obstacle in destroyedObstacles)
                {
                    Vector3 point = obstacle.transform.position;

                    MonoBehaviour.Instantiate(S_CoinPrefab, point + new Vector3(-GridTransform.GridSetting.GridWidth/2, GridTransform.GridSetting.GridHeight/2), Quaternion.identity, _parent);
                    MonoBehaviour.Instantiate(S_CoinPrefab, point + new Vector3(GridTransform.GridSetting.GridWidth/2, GridTransform.GridSetting.GridHeight/2), Quaternion.identity, _parent);
                    MonoBehaviour.Instantiate(S_CoinPrefab, point + new Vector3(-GridTransform.GridSetting.GridWidth/2, -GridTransform.GridSetting.GridHeight/2), Quaternion.identity, _parent);
                    MonoBehaviour.Instantiate(S_CoinPrefab, point + new Vector3(GridTransform.GridSetting.GridWidth/2, -GridTransform.GridSetting.GridHeight/2), Quaternion.identity, _parent);
                }
                _effects.RemoveAt(0);
                _timer = 0;
            }
        }
    }

    public class MagnetBoostEffect : BoostEffect
    {
        public const string BoostName = "MagnetBoost";

        public static float Power = 1;

        private Character _character;

        public MagnetBoostEffect(Character character)
        {
            _character = character;
        }
        
        public override void Effect()
        {
            new MagnetEffect(_character, Power).Effect();
        }
    }

    public class DoubleGoldBoostEffect : BoostEffect
    {
        public const string BoostName = "DoubleGoldBoost";

        private Stage _stage;
        private float _multiplier;
        
        public DoubleGoldBoostEffect(Stage stage, float multiplier)
        {
            _stage = stage;
            _multiplier = multiplier;
        }
        
        public override void Effect()
        {
            _stage.GoldMultiplier = _multiplier;
        }
    }
}