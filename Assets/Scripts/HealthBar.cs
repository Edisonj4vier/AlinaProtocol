using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider healthSlider;
    Damageable _playerDamageable;
    public TMP_Text healthBarText;

    public void Awake()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player == null)
        {
            Debug.Log("No player found in the scene. Make sure it has the 'Player'");
        }
        _playerDamageable = player.GetComponent<Damageable>();
    }

    private void Start()
    {
        healthSlider.value = CalculateSliderPercentage(_playerDamageable.Health, _playerDamageable.MaxHealth);
        healthBarText.text = "HP " + _playerDamageable.Health + " / " + _playerDamageable.MaxHealth;
    }

    private void OnEnable()
    {
        _playerDamageable.healthChanged.AddListener(OnPlayerHealthChanged);
    }

    private void OnDisable()
    {
        _playerDamageable.healthChanged.RemoveListener(OnPlayerHealthChanged);
    }

    private float CalculateSliderPercentage(float currentHealth, float maxHealth)
    {
        return currentHealth / maxHealth;
    }

    private void OnPlayerHealthChanged(int newHealth, int maxHealth)
    {
        healthSlider.value = CalculateSliderPercentage(newHealth, maxHealth);
        healthBarText.text = "HP " + newHealth + " / " + maxHealth;
    }
}
