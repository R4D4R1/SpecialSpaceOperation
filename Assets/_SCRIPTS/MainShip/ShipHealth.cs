using System;
using UnityEngine;

public class ShipHealth : MonoBehaviour , IDamagable
{
    public static ShipHealth Instance;

    [Header("Health")]
    [SerializeField] private int MaxHealth;
    [SerializeField] private GameManager gameManager;

    private int health;
    public Action<int> OnHealthChanged;

    public int CurrentHealth
    {
        get => health;
        set
        {
            health = Mathf.Clamp(value, 0, MaxHealth);
            OnHealthChanged?.Invoke(health);
        }
    }

    

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        CurrentHealth = MaxHealth;
    }

    public void Heal(int healAmount)
    {
        if (CurrentHealth < 100)
        {
            CurrentHealth += healAmount;
        }
    }

    public void TakeDamage(int damage)
    {
        if (CurrentHealth > 0)
        {
            CurrentHealth -= damage;
        
            if (CurrentHealth == 0)
            {
                ShipShooting.Instance.DisableShooting();
                gameManager.StartGameOver();
            }
        }
    }

}
