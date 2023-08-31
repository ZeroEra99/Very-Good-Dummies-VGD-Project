using System;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    private Enemy _currentEnemy;
    private Slider slider;
    private Image fill;
    private Image border;

    public Gradient gradient;

    private void Start()
    {
        _currentEnemy = GetComponentInParent<Enemy>();
        slider = GetComponent<Slider>();
        fill = transform.Find("Fill").GetComponent<Image>();
        border = transform.Find("Border").GetComponent<Image>();
        fill.gameObject.SetActive(false);
        border.gameObject.SetActive(false);
        UpdateHealthBar();
    }

    public void UpdateHealthBar()
    {
        if (_currentEnemy.currentHealth < 0)
        {
            fill.gameObject.SetActive(false);
            border.gameObject.SetActive(false);
        }
        else if (_currentEnemy.currentHealth < _currentEnemy.maxHealth)
        {
            fill.gameObject.SetActive(true);
            border.gameObject.SetActive(true);
        }
        slider.maxValue = _currentEnemy.maxHealth;
        slider.value = _currentEnemy.currentHealth;
        fill.color = gradient.Evaluate(1f);
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }
}