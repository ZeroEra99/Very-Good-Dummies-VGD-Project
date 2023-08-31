using System;
using UnityEngine;

/*
 * To be applied to checkpoint objects.
 */
public class checkPoint : MonoBehaviour
{
    [SerializeField] private GameManager.GameLevel checkPointLevel;
    public ObjectivesManager objectivesManager;
    [SerializeField] private int _isActive;
    private void Awake()
    {
        objectivesManager = FindObjectOfType<ObjectivesManager>();
        GameManager.OnGameStart += LoadProgress;
        GameManager.OnGameSave += SaveProgress;
    }

    private void OnDestroy()
    {
        GameManager.OnGameStart -= LoadProgress;
        GameManager.OnGameSave -= SaveProgress;
    }

    private void SaveProgress(GameManager.SaveType saveType)
    {
        PlayerPrefs.SetInt(name + "isActive", _isActive);
        PlayerPrefs.Save();
    }

    private void LoadProgress(GameManager.GameLevel level)
    {
        if(PlayerPrefs.GetInt("SaveExists")==1)
        {
            Debug.Log("Loading saved checkpoint");
            _isActive = PlayerPrefs.GetInt(name + "isActive");
        }
        else
        {
            Debug.Log("Loading new checkpoint");
            if (checkPointLevel >= level)_isActive = 1;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (_isActive == 1 && other.CompareTag("PlayerBody") && objectivesManager.objectivesCompleted)
        {
            GameManager.audioManager.Play("Checkpoint");
            //This modifies the spawnPoint.
            GameManager.CheckpointReached(transform);
            GameManager.GameSave(GameManager.SaveType.Checkpoint);
            //This disables the checkPoint after it's been reached.
            _isActive = 0;
            Debug.Log("Checkpoint reached! " + name);
        }
    }
}
