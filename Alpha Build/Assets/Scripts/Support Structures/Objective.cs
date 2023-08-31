using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ObjectiveType { Interacting, Killing, Collecting };
[System.Serializable]
public class Objective : MonoBehaviour
{
    public bool finished = false;
    [SerializeField] private GameManager.GameLevel objectiveLevel;
    public ObjectiveType objectiveType;
    public Transform connectedCheckpoint;
    public string objectiveName;

    public void Awake()
    {
        /*switch (objectiveType)
        {
            case ObjectiveType.Interacting:
                objectiveName = "Interact with " + gameObject.name;
                break;
            case ObjectiveType.Killing:
                objectiveName = "Kill " + gameObject.name;
                break;
            case ObjectiveType.Collecting:
                objectiveName = "Collect " + gameObject.name;
                break;
        }*/
        
        GameManager.OnGameStart += LoadProgress;
        GameManager.OnGameSave += SaveProgress;
    }

    private void OnDestroy()
    {
        GameManager.OnGameStart -= LoadProgress;
        GameManager.OnGameSave -= SaveProgress;
    }

    public void Update()
    {
        if (objectiveType == ObjectiveType.Collecting)
            transform.localRotation = Quaternion.Euler(0f, Time.time * 100f, 0);
    }

    public void objectiveFinished()
    {
        if (!finished)
        {
            GameManager.audioManager.Play("ObjectiveFinished");
            finished = true;
        }
    }
    private void SaveProgress(GameManager.SaveType saveType)
    {
        PlayerPrefs.SetInt(objectiveName, finished ? 1 : 0);
        PlayerPrefs.Save();
    }
    private void LoadProgress(GameManager.GameLevel level)
    {
        if (PlayerPrefs.GetInt("SaveExists") == 1) finished = PlayerPrefs.GetInt(objectiveName) == 1;
        else
        {
            if (objectiveLevel >= level) finished = false;
            else finished = true;
        }
    }
}