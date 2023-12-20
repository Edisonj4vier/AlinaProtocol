using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirection), typeof(Damageable))]
public class EnemyKnight : MonoBehaviour
{
    public float walkAcceleration = 3f;
    public float maxSpeed = 3f;
    public float walkStopRate = 0.05f;
    public DetectionZone attackZone;
    private Rigidbody2D _rigidBody2D;
    private Animator _animator;
    private TouchingDirection _touchingDirection;
    Damageable _damageable;
    public DetectionZone cliffDetectionZone;
    public enum WalkableDirection { Right, Left }
    
    private WalkableDirection _walkDirection;
    private Vector2 _walkDirectionVector = Vector2.right;

    public WalkableDirection WalkDirection
    {
        get { return _walkDirection; }
        set
        {
            if (_walkDirection != value)
            {
                //Direction flipped
                gameObject.transform.localScale = new Vector2(gameObject.transform.localScale.x * -1, gameObject.transform.localScale.y);
                if (value == WalkableDirection.Right)
                {
                    _walkDirectionVector = Vector2.right;
                }
                else if (value == WalkableDirection.Left)
                {
                    _walkDirectionVector = Vector2.left;
                }
            }
            _walkDirection = value;
        }
    }
    
    public bool hasTarget = false;
    public bool HasTarget
    {
        get { return hasTarget;}
        set
        {
            hasTarget = value;
            _animator.SetBool(AnimationStrings.hasTarget, value);

        } }
    
    public bool CanMove
    {
        get
        {
            return _animator.GetBool(AnimationStrings.camMove);
        }
    }
    private void Awake()
    {
        _rigidBody2D = GetComponent<Rigidbody2D>();
        _touchingDirection = GetComponent<TouchingDirection>();
        _animator = GetComponent<Animator>();
        _damageable = GetComponent<Damageable>();
    }
    void Update()
    {
        HasTarget = attackZone.detectedCollider2Ds.Count > 0;
        if (AttackCooldown > 0)
        {
            AttackCooldown -= Time.deltaTime;
        }
    }

    public float AttackCooldown
    {
        get
        {
            return _animator.GetFloat(AnimationStrings.attackCooldown);
        }
        set
        {
            _animator.SetFloat(AnimationStrings.attackCooldown, Mathf.Max(value, 0));
        }
    }

   
    private void FixedUpdate()
    {
        // Si el personaje está en el aire, no se puede mover.
        if (_touchingDirection.IsGrounded && _touchingDirection.IsOnWall)
        {
            FlipDirection();
        }
        
        if (!_damageable.LockVelocity)
        {
            if (CanMove && _touchingDirection.IsGrounded)
                // Acerleración del personaje 
            {
                _rigidBody2D.velocity = new Vector2(Mathf.Clamp(
                    _rigidBody2D.velocity.x +(walkAcceleration * _walkDirectionVector.x * Time.fixedDeltaTime), -maxSpeed, maxSpeed),
                    _rigidBody2D.velocity.y);

            }
            else
            {
                // Detener el personaje
                _rigidBody2D.velocity = new Vector2(Mathf.Lerp(_rigidBody2D.velocity.x,0,walkStopRate), _rigidBody2D.velocity.y);
            }
        }
        
    }

   
    private void FlipDirection()
    {   
        // cambiar la dirección del personaje
        if (WalkDirection == WalkableDirection.Right)
        {
            WalkDirection = WalkableDirection.Left;
        }else if (WalkDirection == WalkableDirection.Left)
        {
            WalkDirection = WalkableDirection.Right;
        }
        else
        {
            Debug.LogError("Current walkable direction is not set to legal values of right or left");
        }
    }
    
    public void OnHit(int damage, Vector2 knockback)
    {
        // velocidad del personaje en función del knockback
        _rigidBody2D.velocity = new Vector2(knockback.x, _rigidBody2D.velocity.y + knockback.y);
    }

    public void OnCliffDetected()
    {
        if (_touchingDirection.IsGrounded)
        {
            FlipDirection();
        }
    }
    
}
