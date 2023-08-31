using UnityEngine;
public class GameOverMenu : MonoBehaviour
{
    
    public void OnGameEnd() => GameManager.GameEnd();
    
    private void Start()
    {
        gameObject.SetActive(false);
        GameManager.OnGameOver += EnableGameOverMenuUI;
    }

    private void OnDestroy()
    {
        GameManager.OnGameOver -= EnableGameOverMenuUI;
    }

    void EnableGameOverMenuUI()
    {
        gameObject.SetActive(true);
    }

}
