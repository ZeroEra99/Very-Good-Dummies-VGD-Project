using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;
    public Image fill;

    private void Update()
    {
        updateShownMaxStaminaDisplayed();
        updateCurrentStaminaDisplayed();
    }

    private void updateShownMaxStaminaDisplayed()
    {
        slider.maxValue = PlayerManager.MaxStamina;
    }

    private void updateCurrentStaminaDisplayed()
    {
        slider.value = PlayerManager.CurrentStamina;
    }
}
