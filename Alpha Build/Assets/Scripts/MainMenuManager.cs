using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    AudioManager audioManager;
    int _level = 0;

    private void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
    }
    public void NewGame()
    {
        PlayerPrefs.SetInt("SaveExists", 0);
        PlayerPrefs.SetInt("Level", 0);
        PlayerPrefs.Save();
        SceneManager.LoadScene("GameView", LoadSceneMode.Single);
        Debug.Log("Started a new Game");
    }
    public void LoadGame()
    {
        if (PlayerPrefs.GetInt("SaveExists") == 1)
        {
            SceneManager.LoadScene("GameView", LoadSceneMode.Single);
            Debug.Log("Loaded an existing Game");
        }
        else
        {
            Debug.Log("Save does not exist");
        }
    }

    public void SetLevel(int level)
    {
        _level = level;
    }

    public void LoadLevel ()
    {
        if (_level > 0)
        {
            PlayerPrefs.SetInt("Level", _level);
            PlayerPrefs.SetInt("SaveExists", 0);
            PlayerPrefs.Save();
            Debug.Log("Loaded from level "+ _level);
            SceneManager.LoadScene("GameView", LoadSceneMode.Single);
        }
        // Load Level _level
    }

    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

    public void playAudio(string sound)
    {
        audioManager.Play(sound);
    }

    public void setVolume(float volume)
    {
        audioManager.setVolume(volume);
        PlayerPrefs.SetFloat("Volume", volume);
        PlayerPrefs.Save();
    }
    public void SetMouseSensitivity(float sensitivity)
    {
        PlayerPrefs.SetFloat("CameraSensitivity", sensitivity);
        PlayerPrefs.Save();
    }
}
