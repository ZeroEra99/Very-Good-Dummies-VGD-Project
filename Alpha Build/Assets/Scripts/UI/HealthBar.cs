using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;
    public Image fill;

    private void Update()
    {
        updateShownMaxHealthDisplayed();
        updateCurrentHealthDisplayed();
    }

    private void updateShownMaxHealthDisplayed()
    {
        slider.maxValue = PlayerManager.MaxHealth;
        //fill.color = gradient.Evaluate(1f);
    }

    private void updateCurrentHealthDisplayed()
    {
        slider.value = PlayerManager.CurrentHealth;
        //fill.color = gradient.Evaluate(slider.normalizedValue);
    }
}
