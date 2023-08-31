using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DisplayPlayerStats : MonoBehaviour
{
    private TextMeshProUGUI Lives, Health, Mana, Stamina, StartParameters, PowerupCooldowns, OtherInfos;
    // Start is called before the first frame update
    void Start()
    {
        /*
        Lives = GameObject.Find("LivesDisplay").GetComponent<TextMeshProUGUI>();
        Health = GameObject.Find("HealthDisplay").GetComponent<TextMeshProUGUI>();
        Mana = GameObject.Find("ManaDisplay").GetComponent<TextMeshProUGUI>();
        Stamina = GameObject.Find("StaminaDisplay").GetComponent<TextMeshProUGUI>();
        StartParameters = GameObject.Find("StartParametersDisplay").GetComponent<TextMeshProUGUI>();
        PowerupCooldowns = GameObject.Find("PowerupCooldownsDisplay").GetComponent<TextMeshProUGUI>();
        OtherInfos = GameObject.Find("OtherInfosDisplay").GetComponent<TextMeshProUGUI>();
        */
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.GameIsOver) gameObject.SetActive(false);
        /*
        Lives.text = "Lives:\n" + PlayerManager.CurrentLives;
        Health.text = "Health:\n" + PlayerManager.CurrentHealth;
        Mana.text = "Mana:\n" + PlayerManager.CurrentMana;
        Stamina.text = "Stamina:\n" + PlayerManager.CurrentStamina;
        StartParameters.text = "Start Parameters:\n" + PlayerManager.StartParams;
        string displayString = "Powerup Cooldowns:\n";
        foreach (KeyValuePair<KeyCode, Cooldown> ability in AbilityManager.PlayerAbilities)
        {
            float currentCooldown = ability.Value._nextCooldownTime - Time.time;
            displayString += ability.Key.ToString() + ": " + (currentCooldown > 0 ? currentCooldown : 0) + "\n";
        }
        PowerupCooldowns.text = displayString;
        OtherInfos.text = "Last checkpoint:\n" + PlayerManager.LastCheckpoint;
        */
    }
}
