using System;
using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;

public class PlayerAttack : MonoBehaviour
{
    
    public enum Skill {None,Fist,Fireball,Shield}
    //Object references
    private Animator _animator;
    private Enemy _nearEnemy;
    public static GameObject Wand;
    public static GameObject FireFist;
    public static GameObject Shield;
    [SerializeField] private Transform characterCamera;
    [SerializeField] private Rigidbody playerBullet;
    //Skill parameters
    public static  float FistCooldown=2;
    public static  float FireballCooldown =2;
    private float _lastFistTime;
    private float _lastFireballTime;
    public static int FireBallManaUse = 10;
    public static float BulletSpeed=1800;
    //Skill variables
    public static bool HasFist = false;
    public static bool HasFireFist = false;
    public static bool HasFireball = false;
    public static bool HasShield = false;
    public static Skill CurrentSkill;
    public static int MinFistDamage = 20, MaxFistDamage=30;



    private void Awake()
    {
        GameManager.OnGameStart += LoadProgress;
        GameManager.OnGameSave += SaveProgress;
        _animator = GetComponent<Animator>();
        characterCamera = GameObject.Find("Main Camera").transform;
        Wand = GameObject.Find("Wand");
        FireFist = GameObject.Find("fireFist");
        Shield = GameObject.Find("Shield");
        Wand.SetActive(false);
        FireFist.SetActive(false);
        Shield.SetActive(false);
        CurrentSkill = Skill.None;
        
        FistCooldown=2;
        FireballCooldown =2;
        FireBallManaUse = 10; 
        BulletSpeed=1800;
        MinFistDamage = 20;
        MaxFistDamage=30;
        HasFireFist = false;
        PlayerPowerUps.GodModeEnabled = false;
        PlayerPowerUps.InfiniteMana = false;
        PlayerBullet._minDamage = 30;
        PlayerBullet._maxDamage = 40;
    }

    private void OnDestroy()
    {
        GameManager.OnGameStart-= LoadProgress;
        GameManager.OnGameSave-= SaveProgress;
    }

    void Update()
    {
        if (!PlayerManager.IsAlive || GameManager.GameIsPaused) return;
        SkillSelection();
        InputManagement();
        FireFistEffect();
        ShieldEffect();
    }

    private void SkillSelection()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && HasFist)
        {
            Wand.SetActive(false);
            CurrentSkill = Skill.Fist;
            Shield.SetActive(false);
            Debug.Log("Melee attack selected");
        }

        if (Input.GetKeyDown(KeyCode.Alpha2) && HasFireball )
        {
            FireFist.SetActive(false);
            Wand.SetActive(true);
            Shield.SetActive(false);
            CurrentSkill = Skill.Fireball;
            Debug.Log("Ranged attack selected");
        }


        if (Input.GetKeyDown(KeyCode.Alpha3) && HasShield && PlayerManager.CurrentMana >= PlayerManager.ShieldSManaUse)
        {
            FireFist.SetActive(false);
            Wand.SetActive(false);
            Shield.SetActive(true);
            CurrentSkill = Skill.Shield;
            Debug.Log("Shield attack selected");
        }
    }

    private void InputManagement()
    {
        if (Input.GetMouseButton(0))
        {
            switch (CurrentSkill)
            {
                case Skill.Fist:
                    Fist(_nearEnemy);
                    break;
                case Skill.Fireball:
                    Fireball();
                    break;
                case Skill.Shield:
                    break;
                case Skill.None:
                    break;
            }
        }
    }

    private void Fist(Enemy enemy)
    {
        if (Time.time - _lastFistTime >= FistCooldown)
        {
            AudioSource audiosource = gameObject.AddComponent<AudioSource>();
            GameManager.audioManager.PlayLocal("meleeAttack", audiosource);
            _animator.SetTrigger("hook");
            Debug.Log("Fist damage = " + MinFistDamage + " " + MaxFistDamage);
            if (_nearEnemy != null)
            {
                int damage = Random.Range(MinFistDamage, MaxFistDamage);
                enemy.ReduceHealth(damage,enemy);
            }
            _lastFistTime = Time.time;
        }
    }

    private void Fireball()
    {
        if (Time.time - _lastFireballTime >= FireballCooldown  &&  PlayerManager.CurrentMana >= FireBallManaUse)
        {
            _animator.SetTrigger("magicAttack");
            Debug.Log("Fireball damage = " + " " + PlayerBullet._minDamage + PlayerBullet._maxDamage);
            StartCoroutine(WaitFire());
            PlayerManager.AddMana(-FireBallManaUse);
            _lastFireballTime = Time.time;        }
    }

    private void SaveProgress(GameManager.SaveType saveType)
    {
        PlayerPrefs.SetInt("HasMeleeAttack", Convert.ToInt32(HasFist));
        PlayerPrefs.SetInt("HasRangedAttack", Convert.ToInt32(HasFireball));
        PlayerPrefs.SetInt("HasShield", Convert.ToInt32(HasShield));
        PlayerPrefs.SetInt("CurrentAttack", (int)CurrentSkill);
        PlayerPrefs.Save();
    }

    private void LoadProgress(GameManager.GameLevel level)
    {
        if (PlayerPrefs.GetInt("SaveExists") == 1)
        {
            Debug.Log("Loading saved weapons");
            HasFist = Convert.ToBoolean(PlayerPrefs.GetInt("HasMeleeAttack"));
            HasFireball = Convert.ToBoolean(PlayerPrefs.GetInt("HasRangedAttack"));
            HasShield = Convert.ToBoolean(PlayerPrefs.GetInt("HasShield"));
            CurrentSkill = (Skill)PlayerPrefs.GetInt("CurrentAttack");
        }
        else
        {
            switch (level)
            {
                case GameManager.GameLevel.FirstLevel:
                    Debug.Log("Loading first level weapons");
                    HasFist = false;
                    HasFireball = false;
                    HasShield = false;
                    CurrentSkill = Skill.None;
                    break;
                case GameManager.GameLevel.SecondLevel:
                    Debug.Log("Loading second level weapons");
                    HasFist = true;
                    HasFireball = false;
                    HasShield = false;
                    CurrentSkill = Skill.Fist;
                    break;
                case GameManager.GameLevel.ThirdLevel:
                    Debug.Log("Loading third level weapons");
                    HasFist = true;
                    HasFireball = true;
                    HasShield = false;
                    CurrentSkill = Skill.Fist;
                    break;
                case GameManager.GameLevel.BossFight:
                    Debug.Log("Loading boss level weapons");
                    HasFist = true;
                    HasFireball = true;
                    HasShield = true;
                    CurrentSkill = Skill.Fist;
                    break;
                default:
                    Debug.Log("Error. Loading first level weapons");
                    HasFist = false;
                    HasFireball = false;
                    HasShield = false;
                    CurrentSkill = Skill.None;
                    break;
            }
            SaveProgress(GameManager.SaveType.User);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy")) _nearEnemy = other.GetComponent<Enemy>();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy")) _nearEnemy = null;
    }

    private void FireFistEffect()
    {
        if (CurrentSkill == Skill.Fist && HasFireFist)
            FireFist.SetActive(true);
        else
            FireFist.SetActive(false);
    }

    private void ShieldEffect()
    {
        if (CurrentSkill == Skill.Shield && PlayerManager.CurrentMana >= PlayerManager.ShieldSManaUse)
            Shield.SetActive(true);
        else
            Shield.SetActive(false);
    }

    private IEnumerator WaitFire()
    {
        yield return new WaitForSeconds(0.5f);
        Transform thisTransform = transform;
        Rigidbody bulletClone = Instantiate(playerBullet, thisTransform.position + new Vector3(0, 2f, 0) + Vector3.forward,
            thisTransform.rotation);
        Vector3 bulletDirection = characterCamera.forward;
        bulletClone.AddForce(bulletDirection.normalized * BulletSpeed);
    }
}
