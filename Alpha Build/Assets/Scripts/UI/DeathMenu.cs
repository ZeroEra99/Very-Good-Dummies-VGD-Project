using UnityEngine;

public class DeathMenu : MonoBehaviour
{
    public void OnGameStart()
    {
        PlayerManager.AddLives(-1);
        PlayerPrefs.SetInt("Lives",PlayerManager.CurrentLives);
        PlayerPrefs.Save();
        GameManager.GameStart(PlayerPrefs.GetInt("Level") switch
        {
            0 => GameManager.GameLevel.FirstLevel,
            1 => GameManager.GameLevel.SecondLevel,
            2 => GameManager.GameLevel.ThirdLevel,
            3 => GameManager.GameLevel.BossFight,
            _ => GameManager.GameLevel.FirstLevel
        });
    }

    private void Start()
    {
        gameObject.SetActive(false);
        GameManager.OnPlayerDeath += EnableDeathMenuUI;
        GameManager.OnGameStart += DisableDeathMenuUIfix;
        GameManager.OnGameOver += DisableDeathMenuUI;
        GameManager.OnGameEnd += DisableDeathMenuUI;
    }

    private void OnDestroy()
    {
        GameManager.OnPlayerDeath -= EnableDeathMenuUI;
        GameManager.OnGameStart -= DisableDeathMenuUIfix;
        GameManager.OnGameOver -= DisableDeathMenuUI;
        GameManager.OnGameEnd -= DisableDeathMenuUI;
    }

    private void EnableDeathMenuUI() => gameObject.SetActive(true);
    private void DisableDeathMenuUI() => gameObject.SetActive(false);
    private void DisableDeathMenuUIfix(GameManager.GameLevel level) => gameObject.SetActive(false);
}