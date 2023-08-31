using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerUps : MonoBehaviour
{
    public Image godMode;
    public Image speedHack;
    public Image fireFist;


    void Update()
    {
        updateCurrentPowerUp();
    }


    private void updateCurrentPowerUp()
    {
        if (PlayerPowerUps.GodModeEnabled)
            godMode.GetComponent<Image>().color = new Color32(255, 255, 225, 250);
       
        else
            godMode.GetComponent<Image>().color = new Color32(75, 75, 75, 250);

        if (!PlayerMovement.normalMode)
            speedHack.GetComponent<Image>().color = new Color32(255, 255, 225, 250);
        else
            speedHack.GetComponent<Image>().color = new Color32(75, 75, 75, 250);

        if (PlayerAttack.HasFireFist)
            fireFist.GetComponent<Image>().color = new Color32(255, 255, 225, 250);
        else
            fireFist.GetComponent<Image>().color = new Color32(75, 75, 75, 250);
    }

}
