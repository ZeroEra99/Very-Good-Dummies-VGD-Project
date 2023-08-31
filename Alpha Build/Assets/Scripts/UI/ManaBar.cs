using UnityEngine;
using UnityEngine.UI;

public class ManaBar : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;
    public Image fill;

    private void Update()
    {
        updateShownMaxManaDisplayed();
        updateCurrentManaDisplayed();
    }

    private void updateShownMaxManaDisplayed()
    {
        slider.maxValue = PlayerManager.MaxMana;
    }

    private void updateCurrentManaDisplayed()
    {
        slider.value = PlayerManager.CurrentMana;
    }
}
