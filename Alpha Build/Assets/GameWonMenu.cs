using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameWonMenu : MonoBehaviour
{
    public void OnGameResume() => GameManager.Resume();
    public void OnGameEnd() => GameManager.GameEnd();

    private void Start()
    {
        gameObject.SetActive(false);
        GameManager.OnGameWon += EnableMenuUI;
        GameManager.OnGameResume += DisableMenuUI;
    }

    private void OnDestroy()
    {
        GameManager.OnGameWon -= EnableMenuUI;
        GameManager.OnGameResume -= DisableMenuUI;
    }


    private void EnableMenuUI() => gameObject.SetActive(true);
    private void DisableMenuUI() => gameObject.SetActive(false);
}
