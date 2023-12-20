using UnityEngine;
using UnityEngine.Events;

public class Damageable : MonoBehaviour
{
    public UnityEvent<int, Vector2> damageableHit;
    public UnityEvent damageableDeath;
    public UnityEvent<int, int> healthChanged;
    Animator _animator;
    [SerializeField] private int maxHealth = 100;
    
    public int MaxHealth
    {
        get
        {
            return maxHealth;
        }
        set
        {
            maxHealth = value; }
    }
    
    [SerializeField] private int health = 100;

    public int Health
    {
        get
        {
            return health;
        }
        set
        {
            health = value;
            healthChanged?.Invoke(health, MaxHealth);
            // Si la salud cae por debajo de 0, el personaje ya no está vivo.
            if(health <= 0)
            {
                IsAlive = false;
            }
        }
    }
    
    [SerializeField] private bool isAlive = true;
    [SerializeField] private bool isInvincible = false;
    private float _timeSinceHit = 0;
    public float invincibilityTime = 0.25f;
    
    public bool IsAlive
    {
        get
        {
            return isAlive;
        }
        set
        {
            isAlive = value;
            _animator.SetBool(AnimationStrings.isAlive, value);
            Debug.Log("IsAlive set: " + value);

            if (value == false)
            {
                damageableDeath.Invoke();
            }
        }
    }
    // Bloquea la velocidad del personaje para que no se mueva mientras está en el aire.
    public bool LockVelocity
    {
        get
        {
            return _animator.GetBool(AnimationStrings.lockVelocity);
        }

        set
        {
            _animator.SetBool(AnimationStrings.lockVelocity, value);
        }
    }
    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (isInvincible)
        {
            if (_timeSinceHit > invincibilityTime)
            {
                // Remover invencibilidad
                isInvincible = false;
                _timeSinceHit = 0;
            }
            _timeSinceHit += Time.deltaTime;
        }
    }

  
    // Daña al personaje. Retorna true si el personaje está vivo y puede ser dañado.
    public bool Hit(int damage, Vector2 knockback)
    {
        if (IsAlive && !isInvincible)
        {
            Health -= damage;
            isInvincible = true;
            // Llama a los eventos de daño
            _animator.SetTrigger(AnimationStrings.hitTrigger);
            LockVelocity = true;
            damageableHit?.Invoke(damage, knockback);
            CharacterEvents.characterDamaged.Invoke(gameObject, damage);
            return true;
        }
        // Si el personaje no está vivo, no se puede dañar.
        return false;
    }

    public bool Heal(int healthRestore)
    {
        if (IsAlive && Health < MaxHealth)
        {
            int maxHeal = Mathf.Max(MaxHealth - Health, 0);
            int actualHeal = Mathf.Min(maxHeal, healthRestore);
            Health += actualHeal;
            CharacterEvents.characterHealed(gameObject, actualHeal);
            return true;
        }
        return false;
    }

    
}
