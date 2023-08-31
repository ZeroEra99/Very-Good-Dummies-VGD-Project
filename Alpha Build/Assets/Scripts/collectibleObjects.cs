using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;


public class collectibleObjects : MonoBehaviour
{
    private int _amount;
    private bool _wasCollected;
    [SerializeField] GameManager.GameLevel collectibleLevel;
    private void Awake()
    {
        GameManager.OnGameStart += LoadProgress;
        GameManager.OnGameSave += SaveProgress;
    }

    private void Start()
    {
        if(_wasCollected)gameObject.SetActive(false);
        else gameObject.SetActive(true);
        
    }

    public void Update()
    {
        transform.localRotation = Quaternion.Euler(0f, Time.time * 100f, 0);
    }

    private void OnDestroy()
    {
        GameManager.OnGameStart -= LoadProgress;
        GameManager.OnGameSave -= SaveProgress;
    }

    public void OnTriggerEnter(Collider other)
    {
        _amount = Random.Range(33, 50);
        if (other.CompareTag("PlayerBody"))
        {
            if (CompareTag("manaCollectible")) {
                PlayerManager.manaEffect.SetActive(true);
                PlayerManager.AddMana(_amount);
                Debug.Log("User has collected " + _amount + " mana");
            }
            if (CompareTag("healthCollectible")){
                PlayerManager.healthEffect.SetActive(true);
                PlayerManager.AddHealth(_amount);
                Debug.Log("User has collected " + _amount + " health");
            }
            _wasCollected = true;
            GameManager.audioManager.Play("ObjectiveFinished");
            GameManager.Collected(this.gameObject);
            gameObject.SetActive(false);
        }
    }

    private void LoadProgress(GameManager.GameLevel level)
    {
        if (PlayerPrefs.GetInt("SaveExists") == 1)_wasCollected = Convert.ToBoolean(PlayerPrefs.GetInt(name));
        else
        {
            if (collectibleLevel >= level)
            {
                _wasCollected = false;
                SaveProgress(0);
            }
            else
            {
                _wasCollected = true;
                SaveProgress(0);
            }
        }
    }

    private void SaveProgress(GameManager.SaveType saveType)
    {
        PlayerPrefs.SetInt(name,Convert.ToInt32(_wasCollected));
        PlayerPrefs.Save();
    }
}
