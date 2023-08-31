using UnityEngine;
public class PauseMenu : MonoBehaviour
{
    public void OnGameResume() => GameManager.Resume();
    public void OnGameSave()
    {
        PlayerPrefs.SetInt("SaveType", (int)GameManager.SaveType.User);
        PlayerPrefs.Save();
        GameManager.GameSave(GameManager.SaveType.User);
    }

    private void Start()
    {
        gameObject.SetActive(false);
        GameManager.OnGamePause += EnablePauseMenuUI;
        GameManager.OnGameResume += DisablePauseMenuUI;
    }
    private void OnDestroy()
    {
        GameManager.OnGamePause -= EnablePauseMenuUI;
        GameManager.OnGameResume -= DisablePauseMenuUI;
    }

    private void EnablePauseMenuUI() => gameObject.SetActive(true);
    private void DisablePauseMenuUI() => gameObject.SetActive(false);
}

