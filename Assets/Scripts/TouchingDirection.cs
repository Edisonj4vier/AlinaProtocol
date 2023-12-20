using UnityEngine;
// Uses the collider to check directions to see if the object is currently on the fround, touching the wall, or touching the celing.
public class TouchingDirection : MonoBehaviour
{
    private CapsuleCollider2D _touchingCollider;
    private Animator _animator;
    public ContactFilter2D castFilter2D;
    public float groundDistance = 0.05f;
    public float wallDistance = 0.2f;
    public float ceilingDistance = 0.05f;
    private RaycastHit2D[] _groundHits = new RaycastHit2D[5];
    private RaycastHit2D[] _wallHits = new RaycastHit2D[5];
    private RaycastHit2D[] _ceilingHits = new RaycastHit2D[5];
    [SerializeField] private bool isGrounded;
    [SerializeField] private bool isOnWall;
    [SerializeField] private bool isOnCeiling;
    private Vector2 WallCheckDirection => gameObject.transform.localScale.x > 0 ? Vector2.right : Vector2.left;


    public bool IsGrounded
    {
        get
        {
            return isGrounded;
        }
        set
        {
            isGrounded = value;
            _animator.SetBool(AnimationStrings.isGrounded, value);
        }
    }
    public bool IsOnWall
    {
        get
        {
            return isOnWall;
        }
        set
        {
            isOnWall = value;
            _animator.SetBool(AnimationStrings.isOnWall, value);
        }
    }
    public bool IsOnCeiling
    {
        get
        {
            return isOnCeiling;
        }
        set
        {
            isOnCeiling = value;
            _animator.SetBool(AnimationStrings.isOnCeiling, value);
        }
    }

    private void Awake()
    {
        _touchingCollider = GetComponent<CapsuleCollider2D>();
        _animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        IsGrounded = _touchingCollider.Cast(Vector2.down, castFilter2D, _groundHits, groundDistance) > 0;
        IsOnWall = _touchingCollider.Cast(WallCheckDirection, castFilter2D, _wallHits, wallDistance) > 0;
        IsOnCeiling = _touchingCollider.Cast(Vector2.up, castFilter2D, _ceilingHits, ceilingDistance) > 0;  
    }

}
