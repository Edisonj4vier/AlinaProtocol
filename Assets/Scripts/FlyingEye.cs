using System.Collections.Generic;
using UnityEngine;

public class FlyingEye : MonoBehaviour
{
    public float flightSpeed = 2f;
    public float waypointReachedDistance = 0.1f;
    public DetectionZone biteDetectionZone;
    public Collider2D deathCollider;
    public List<Transform> waypoints;
    Animator _animator;
    Rigidbody2D _rigidBody2D;
    Damageable _damageable;
    int _wayPointNum = 0;
    Transform _nextWayPoint;
    
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
    public void Awake()
    {
        _animator = GetComponent<Animator>();
        _rigidBody2D = GetComponent<Rigidbody2D>();
        _damageable = GetComponent<Damageable>();
    }

    private void Start()
    {
        _nextWayPoint = waypoints[_wayPointNum];
    }

    private void OnEnable()
    {
        _damageable.damageableDeath.AddListener(OnDeath);
    }

    private void Update()
    {
        HasTarget = biteDetectionZone.detectedCollider2Ds.Count > 0;
    }
    

    private void FixedUpdate()
    {
        if (_damageable.IsAlive)
        {
            if (CanMove)
            {
                Flight();
            }
            else
            {
                _rigidBody2D.velocity = Vector2.zero;
            }
        }
        
    }

    private void Flight()
    {
        Vector2 directionToWaypoint = (_nextWayPoint.position - transform.position).normalized;
        
        float distance = Vector2.Distance(_nextWayPoint.position, transform.position);
        
        _rigidBody2D.velocity = directionToWaypoint * flightSpeed;
        UpdateDirection();

        if (distance <= waypointReachedDistance)
        {
            _wayPointNum++;
            if (_wayPointNum >= waypoints.Count)
            {
                _wayPointNum = 0;
            }
            
            _nextWayPoint = waypoints[_wayPointNum];

        }
    }

    private void UpdateDirection()
    {
        Vector3 locScale = transform.localScale;
        if (transform.localScale.x > 0)
        {
            if (_rigidBody2D.velocity.x < 0)
            {
                transform.localScale = new Vector3(-1 * locScale.x, locScale.y, locScale.z);
            }
        }
        else
        {
            if (_rigidBody2D.velocity.x > 0)
            {
                transform.localScale = new Vector3(-1 * locScale.x, locScale.y, locScale.z);
            }
        }
    }

    public void OnDeath()
    {
        _rigidBody2D.gravityScale = 2f;
        _rigidBody2D.velocity = new Vector2(0, _rigidBody2D.velocity.y);
        deathCollider.enabled = false;
    }
}
