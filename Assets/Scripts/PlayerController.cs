using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D),typeof(TouchingDirection), typeof(Damageable))]
public class PlayerController : MonoBehaviour
{
    private Vector2 moveInput;
    private Rigidbody2D rigidBody2D;
    public float walkSpeed = 5f;
    public float runSpeed = 8f;
    public float airWalkSpeed = 3f;
    private Animator animator;
    public float jumpImpulse = 10f;
    private TouchingDirection _touchingDirection;
    Damageable _damageable;
    
    // La velocidad de movimiento actual depende de si el jugador está en el suelo, en el aire, corriendo o caminando.
    public float CurrentMoveSpeed
    {
        get
        {
            if (CanMove)
            {
                if (IsMoving && !_touchingDirection.IsOnWall)
                {
                    if (_touchingDirection.IsGrounded)
                    {
                        if (IsRunning)
                        {
                            return runSpeed;
                        }else
                        {
                            return walkSpeed;
                        }
                    }
                    else
                    {
                        //Air move
                        return airWalkSpeed;
                    }
                }
                else
                {
                    // Idle speed is 0
                    return 0;
                }
                
            }
            else
            {
                return 0;
            }
            
        }
    }
    [SerializeField]
    private bool _isMoving = false;
    
    // Propiedad que actualiza el valor de la variable de animación
    public bool IsMoving
    {
        get
        {
            return _isMoving;
        }
        set
        {
            _isMoving = value;
            animator.SetBool(AnimationStrings.isMoving, value);
        }
    }
    
    [SerializeField]
    private bool _isRunning = false;
    
    public bool IsRunning
    {
        get
        {
            return _isRunning;
        }
        set
        {
            _isRunning = value;
            animator.SetBool(AnimationStrings.isRunning, value);
        }
    }
    
    public bool _isFacingRight = true;

    // Propiedad que actualiza el valor de la variable de animación
    public bool IsFacingRight
    {
        get { return _isFacingRight;}
        set
        {
            // Voltea sólo si el valor es nuevo
            if (_isFacingRight != value)
            {
                // Voltea la escala local para que el jugador mire en la dirección opuesta
                transform.localScale *= new Vector2(-1,1);
            }
            _isFacingRight = value;
        }
        
    }

    public bool CanMove
    {
        get
        {
            return animator.GetBool(AnimationStrings.camMove);
        }
    }

    

    public bool IsAlive
    {
        get
        {
            return animator.GetBool(AnimationStrings.isAlive);
        }
    }
    private void Awake()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        _touchingDirection = GetComponent<TouchingDirection>();
        _damageable = GetComponent<Damageable>();
    }
    
    private void FixedUpdate()
    {
        if (!_damageable.LockVelocity)
        {
            rigidBody2D.velocity = new Vector2(moveInput.x * CurrentMoveSpeed, rigidBody2D.velocity.y);
        }
        animator.SetFloat(AnimationStrings.yVelocity, rigidBody2D.velocity.y);
    }

    // Llamado por el evento de animación
    public void OnMove(InputAction.CallbackContext context)
    {
        //
        moveInput = context.ReadValue<Vector2>();
        if (IsAlive)
        {
            IsMoving = moveInput != Vector2.zero;
            SetFacingDirection(moveInput);
        }else
        {
            IsMoving = false;
        }
    }

    private void SetFacingDirection(Vector2 moveInput)
    {
        if (moveInput.x > 0 && !IsFacingRight)
        {
            //Face the right
            IsFacingRight = true;
        }else if(moveInput.x < 0 && IsFacingRight)
        {
            //Face the left
            IsFacingRight = false;
        }
    }


    public void OnRun(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            IsRunning = true;
        }else if (context.canceled)
        {
            IsRunning = false;
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        
        if (context.started && _touchingDirection.IsGrounded && CanMove)
        {
            animator.SetTrigger(AnimationStrings.jumpTrigger);
            rigidBody2D.velocity = new Vector2(rigidBody2D.velocity.x, jumpImpulse);
        }
    }
    
    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            animator.SetTrigger(AnimationStrings.attackTrigger);
        }
    }
    
    public void OnRangedAttack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            animator.SetTrigger(AnimationStrings.rangedAttackTrigger);
        }
    }
    
    //
    public void OnHit(int damage, Vector2 knockback)
    {
        rigidBody2D.velocity = new Vector2(knockback.x, rigidBody2D.velocity.y + knockback.y);
    }
}
