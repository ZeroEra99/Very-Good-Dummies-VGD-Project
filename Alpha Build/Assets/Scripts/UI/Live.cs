using UnityEngine.UI;
using UnityEngine;

public class Live : MonoBehaviour
{
    public int lives;

    [SerializeField] public Image[] hearts;

    // Update is called once per frame
    void Update()
    {
        UpdateLive();
    }

    public void UpdateLive()
    {
        lives = PlayerManager.CurrentLives;

        for(int i = 0; i < hearts.Length; i++)
        {
            if (i >= lives)
                hearts[i].color = Color.black;
        }


    }
}
