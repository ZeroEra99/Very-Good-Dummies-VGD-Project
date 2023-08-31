using System.Collections;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    //Object references
    public static Animator animator;
    //gameobject for effects
    public static GameObject healthEffect;
    public static GameObject manaEffect;
    //Stats parameters
    public static readonly int MaxLives = 3;
    public static readonly int MaxHealth = 100;
    public static readonly int MaxMana = 100;
    public static readonly float MaxStamina = 100;
    private static readonly int DefaultLives = 3;
    private static readonly int DefaultHealth = 80;
    private static readonly int DefaultMana = 80;
    private static readonly int DefaultStamina = 100;


    //Spawn variables
    public static Transform SpawnPoint;
    public static Transform LastCheckpoint;
    
    //Stats variables
    public static int CurrentLives;
    public static int CurrentHealth;
    public static float CurrentMana;
    public static float CurrentStamina;
    public static bool IsAlive;

    public static int ShieldSManaUse = 2;


    // Start is called before the first frame update
    private void Awake()
    {
        animator = GetComponent<Animator>();
        SpawnPoint = GameObject.Find("Spawn").transform;
        healthEffect = GameObject.Find("Health Effect");
        manaEffect = GameObject.Find("Mana Effect");
        healthEffect.SetActive(false);
        manaEffect.SetActive(false);
        

        //Events
        GameManager.OnGameStart += LoadPlayerPrefs;
        GameManager.OnCheckpointReached += SetSpawnPoint;
        GameManager.OnGameSave += SaveProgress;
    }

    private void Update()
    {
        if (!IsAlive) return;
        if (CurrentHealth < 1)
        {
            IsAlive = false;
            animator.SetTrigger("death");
            PlayerAttack.Shield.SetActive(false);
            if (CurrentLives > 0) GameManager.PlayerDeath();
            else if (CurrentLives < 1) GameManager.GameOver();
        }

        if (healthEffect.activeInHierarchy || manaEffect.activeInHierarchy) //mana and health collectible effect duration
            StartCoroutine(Wait());

        if(PlayerAttack.CurrentSkill == PlayerAttack.Skill.Shield && !PlayerPowerUps.InfiniteMana) 
            CurrentMana -= ShieldSManaUse * Time.deltaTime;
    }

    private void OnDestroy()
    {
        //Stats events
        GameManager.OnGameStart -= LoadPlayerPrefs;
        GameManager.OnCheckpointReached -= SetSpawnPoint;
        GameManager.OnGameSave -= SaveProgress;
    }

    //Spawn Functions
    private void Spawn()
    {
        PlayerAttack.CurrentSkill = PlayerAttack.Skill.None;
        animator.Play("Walking Tree");
        GameObject.Find("Player Body").transform.position = SpawnPoint.transform.position;
        Physics.SyncTransforms();
        Debug.Log("Player spawned at " + SpawnPoint);
    }

    private void SetSpawnPoint(Transform checkpoint)
    {
        LastCheckpoint = checkpoint;
        SpawnPoint = checkpoint;
        PlayerPrefs.SetString("LastCheckpoint", LastCheckpoint.name);
        PlayerPrefs.SetInt("SaveType", (int)GameManager.SaveType.Checkpoint);
        PlayerPrefs.Save();
        Debug.Log("Spawn point set");
    }

    public static void AddLives(int amount)
    {
        if (CurrentLives + amount > MaxLives) CurrentLives = MaxLives;
        else CurrentLives += amount;
        Debug.Log("Lives set to " + CurrentLives);
    }
    public static void AddHealth(int amount)
    {
        if (PlayerPowerUps.GodModeEnabled && amount < 0) return;
        if (PlayerAttack.CurrentSkill == PlayerAttack.Skill.Shield && amount < 0) { 
            float newAmount = amount - (amount * 0.6f);
            CurrentHealth += (int)newAmount;
        }
        else if (CurrentHealth + amount > MaxHealth) CurrentHealth = MaxHealth;
        else CurrentHealth += amount;
        Debug.Log("Health set to " + CurrentHealth);
    }

    public static void AddMana(float amount)
    {
        if (PlayerPowerUps.InfiniteMana && amount < 0) return;
        if (CurrentMana + amount > MaxMana) CurrentMana = MaxMana;
        else if (CurrentMana + amount < 0) CurrentMana = 0;
        else CurrentMana += amount;
        Debug.Log("Mana set to " + CurrentMana);
    }
    public static void AddStamina(int amount)
    {
        if (CurrentStamina + amount > MaxStamina) CurrentStamina = MaxStamina;
        else if (CurrentStamina + amount < 0) CurrentStamina = 0;
        else CurrentStamina += amount;
        Debug.Log("Stamina set");
    }



    private void SavePosition()
    {
        Transform player = GameObject.Find("Player Body").transform;
        //Save the coordinates of the player in the playerprefs
        PlayerPrefs.SetFloat("PlayerX", player.position.x);
        PlayerPrefs.SetFloat("PlayerY", player.position.y);
        PlayerPrefs.SetFloat("PlayerZ", player.position.z);
        //Save the rotation of the player in the playerprefs
        PlayerPrefs.SetFloat("PlayerRotX", player.rotation.x);
        PlayerPrefs.SetFloat("PlayerRotY", player.rotation.y);
        PlayerPrefs.SetFloat("PlayerRotZ", player.rotation.z);
        PlayerPrefs.SetFloat("PlayerRotW", player.rotation.w);
    }

    private Transform LoadPosition()
    {
        Transform spawnPoint = new GameObject("PlayerPosition").transform;
        //Load the coordinates of the player from the playerprefs
        float x = PlayerPrefs.GetFloat("PlayerX");
        float y = PlayerPrefs.GetFloat("PlayerY");
        float z = PlayerPrefs.GetFloat("PlayerZ");
        //Load the rotation of the player from the playerprefs
        float rotX = PlayerPrefs.GetFloat("PlayerRotX");
        float rotY = PlayerPrefs.GetFloat("PlayerRotY");
        float rotZ = PlayerPrefs.GetFloat("PlayerRotZ");
        float rotW = PlayerPrefs.GetFloat("PlayerRotW");
        //Return the coordinates and the rotation of the player
        spawnPoint.position = new Vector3(x, y, z);
        spawnPoint.rotation = new Quaternion(rotX, rotY, rotZ, rotW);
        return spawnPoint;
    }
    private void SaveProgress(GameManager.SaveType saveType)
    {
        if (saveType == GameManager.SaveType.Checkpoint) PlayerPrefs.SetString("LastCheckPoint", LastCheckpoint.name);
        else if (saveType == GameManager.SaveType.User) SavePosition();

        PlayerPrefs.SetInt("SaveExists", 1);
        PlayerPrefs.SetInt("Lives", CurrentLives);
        PlayerPrefs.SetInt("Health", CurrentHealth);
        PlayerPrefs.SetFloat("Mana", CurrentMana);
        PlayerPrefs.SetFloat("Stamina", CurrentStamina);
        
        PlayerPrefs.Save();
        Debug.Log("Progress saved");
    }

    private void LoadPlayerPrefs(GameManager.GameLevel level)
    {
        int saveType = PlayerPrefs.GetInt("SaveType");
        LoadProgress((GameManager.SaveType)saveType, level);
        Spawn();
    }
    private void LoadProgress(GameManager.SaveType saveType, GameManager.GameLevel level)
    {
        if (PlayerPrefs.GetInt("SaveExists") == 1)
        {
            IsAlive = true;
            CurrentLives = PlayerPrefs.GetInt("Lives");
            CurrentHealth = PlayerPrefs.GetInt("Health");
            CurrentMana = PlayerPrefs.GetFloat("Mana");
            CurrentStamina = PlayerPrefs.GetFloat("Stamina");
            if (saveType == GameManager.SaveType.User) SpawnPoint = LoadPosition();
            else if(saveType==GameManager.SaveType.Checkpoint)SpawnPoint = GameObject.Find(PlayerPrefs.GetString("LastCheckpoint")).transform;
            LastCheckpoint = GameObject.Find(PlayerPrefs.GetString("LastCheckpoint")).transform;
            Debug.Log("Progress loaded");
        }else
        {
            IsAlive = true;
            CurrentLives = DefaultLives;
            CurrentHealth = DefaultHealth;
            CurrentMana = DefaultMana;
            CurrentStamina = DefaultStamina;
            switch (level)
            {
                case GameManager.GameLevel.FirstLevel:
                    SpawnPoint = GameObject.Find("Spawn").transform;
                    LastCheckpoint = GameObject.Find("Spawn").transform;
                    break;
                case GameManager.GameLevel.SecondLevel:
                    SpawnPoint = GameObject.Find("Second level").transform;
                    LastCheckpoint = GameObject.Find("Second level").transform;
                    break;
                case GameManager.GameLevel.ThirdLevel:
                    SpawnPoint = GameObject.Find("Third level").transform;
                    LastCheckpoint = GameObject.Find("Third level").transform;
                    break;
                case GameManager.GameLevel.BossFight:
                    SpawnPoint = GameObject.Find("Boss level").transform;
                    LastCheckpoint = GameObject.Find("Boss level").transform;
                    break;
            }
            PlayerPrefs.SetString("SpawnPoint", SpawnPoint.name);
            PlayerPrefs.SetString("LastCheckpoint", LastCheckpoint.name);
            PlayerPrefs.Save();
            Debug.Log("Default stats set.");
        }
    }

    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(1.3f);
        healthEffect.SetActive(false);
        manaEffect.SetActive(false);
    }
}
