using System;
using System.Diagnostics;
using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;
using Debug = UnityEngine.Debug;

public class GameManager : MonoBehaviour
{
    public enum SaveType { Checkpoint, User }
    public enum GameLevel { FirstLevel, SecondLevel, ThirdLevel, BossFight }
    
    //GAME EVENTS
    public static event Action<GameLevel> OnGameStart;
    public static event Action OnGameOver,OnGameEnd,OnGameWon,OnGamePause,OnGameResume;
    public static event Action<SaveType> OnGameSave;
    public static event Action OnPlayerDeath;
    public static event Action<GameObject> OnInteraction;
    public static event Action<GameObject> OnEnemyKill;
    public static event Action<GameObject> OnCollection;

    //CHECKPOINT EVENTS
    public static event Action<Transform> OnCheckpointReached;
    public static bool GameIsRunning, GameIsPaused, GameIsOver;

    public static Camera MainCamera;
    public static CinemachineBrain cameraBrain;
    public static AudioManager audioManager;

    public static bool cheat = false;
    
    
    //DEFAULT FUNCTIONS
    private void Awake()
    {
        GameLevel level=PlayerPrefs.GetInt("Level") switch
        {
            0 => GameLevel.FirstLevel,
            1 => GameLevel.SecondLevel,
            2 => GameLevel.ThirdLevel,
            3 => GameLevel.BossFight,
            _ => GameLevel.FirstLevel
        };
        MainCamera = Camera.main;
        cameraBrain = MainCamera.GetComponent<CinemachineBrain>();
        audioManager = FindObjectOfType<AudioManager>();
        GameStart(level);
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !GameIsOver && !GameIsPaused && GameIsRunning) Pause();   
    }

    public static void GameStart(GameLevel level)
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1;
        GameIsOver = false;
        GameIsRunning = true;
        GameIsPaused = false;
        cameraBrain.enabled = true;
        audioManager.ThemeTransition("FirstLevelTheme", 2);
        OnGameStart?.Invoke(level);
        Debug.Log("Game started");
    }

    public static void GameOver()
    {
        GameIsOver = true;
        GameIsRunning = false;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        cameraBrain.enabled = false;
        OnGameOver?.Invoke();
        Debug.Log("Game lost");
    }
    
    public static void GameEnd()
    {
        GameIsRunning = false;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
        OnGameEnd?.Invoke();
        Debug.Log("Game ended");
    }
    
    public static void Pause()
    {
        GameIsPaused = true;
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        cameraBrain.enabled = false;
        OnGamePause?.Invoke();
        Debug.Log("Game paused");
    }
    
    public static void Resume()
    {
        OnGameResume?.Invoke();
        GameIsPaused = false;
        Time.timeScale = 1;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        cameraBrain.enabled = true;
        Debug.Log("Game resumed");
    }

    public static void GameSave(SaveType saveType)
    {
        Debug.Log("Game saved");
        OnGameSave?.Invoke(saveType);
    }

    public static void CheckpointReached(Transform checkpoint) => OnCheckpointReached?.Invoke(checkpoint);

    public static void PlayerDeath()
    {
        //audioManager.Play("PlayerDeath"); - DISABLED: It bugs the PlayerDeath
        cameraBrain.enabled = false;
        GameIsRunning = false;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        OnPlayerDeath?.Invoke();
        Debug.Log("Player died");
    }

    public static void PlayerInteracted(GameObject Object)
    {
        Debug.Log("Interacting with something");
        OnInteraction?.Invoke(Object);
    }

    public static void EnemyKilled(GameObject Enemy)
    {
        Debug.Log("Killed an enemy");
        OnEnemyKill?.Invoke(Enemy);
    }

    public static void Collected(GameObject Collectable)
    {
        Debug.Log("collected a Collectable");
        OnCollection?.Invoke(Collectable);
    }

    public void playButtonPress()
    {
        audioManager.Play("ButtonPress");
    }
    public void cheatMode(bool check)
    {
        if (check)
        {
            cheat = true;
            if (PlayerMovement.normalMode) PlayerPowerUps.IncreaseSPeed();
            if (!PlayerAttack.HasFireFist) PlayerPowerUps.IncreaseAttack();
            PlayerPowerUps.GodModeEnabled = true;
            PlayerPowerUps.InfiniteMana = true;

        }
        else
        {
            cheat = false;
            PlayerPowerUps.GodModeEnabled = false;
            PlayerPowerUps.InfiniteMana = false;
            PlayerPowerUps.DecreaseSPeed();
            PlayerPowerUps.DecreaseAttack();
        }
    }

    internal static void GameWon()
    {
        GameIsPaused = true;
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        cameraBrain.enabled = false;
        Debug.Log("Game is won");
        audioManager.Play("GameWonTheme");
        OnGameWon?.Invoke();
    }
}