using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    public enum EnemyType { Melee, Ranged, Guard, Boss }


    private ItemDrop getItem;
    [SerializeField] public EnemyType enemyType;
    [SerializeField] public GameManager.GameLevel enemyLevel;
    public SpriteRenderer miniMapIcon;
    public Animator animator;
    EnemyHealthBar healthBar;
    private NavMeshAgent _agent;
    private Transform _player;
    [SerializeField] private Rigidbody enemyBullet;
    private LayerMask _groundLayer, _playerLayer;
    
    public bool isAlive;
    public int currentHealth;
    
    public int maxHealth;
    private float _sightRange;
    private float _walkPointRange;
    private float _attackRange;
    private float _attackCooldown;
    private readonly float _bulletSpeed=1200f;
    private float _lastAttackTime;
    private bool _lastAlive;
    public bool bossSecondPhase;

    private Vector3 _walkPoint;
    private bool _walkPointSet;
    
    

    private bool once=true;

    private void Awake()
    {
        GameManager.OnGameStart += LoadEnemies;
        GameManager.OnGameSave += SaveEnemies;
        SetStats();
        _agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        healthBar = GetComponentInChildren<EnemyHealthBar>();
        miniMapIcon = GetComponentInChildren<SpriteRenderer>();
        _player = GameObject.Find("Player Body").transform;
        _groundLayer = LayerMask.GetMask("Ground");
        _playerLayer = LayerMask.GetMask("Player");
        getItem = GetComponent<ItemDrop>();
    }

    private void OnDestroy()
    {
        GameManager.OnGameStart -= LoadEnemies;
        GameManager.OnGameSave -= SaveEnemies;
    }



    // Update is called once per frame
    void Update()
    {
        if(isAlive)
        {
            if (currentHealth <= 0) Death();
            Vector3 v = _agent.velocity;
            animator.SetFloat("hInput", v.x);
            animator.SetFloat("vInput", v.y);
            
            if (Physics.CheckSphere(transform.position,_sightRange/2,_playerLayer) && PlayerManager.IsAlive) transform.LookAt(_player);
            if (Physics.CheckSphere(transform.position, _attackRange, _playerLayer) && PlayerManager.IsAlive) AttackPlayer(enemyType);
            if (Physics.CheckSphere(transform.position, _sightRange, _playerLayer) && PlayerManager.IsAlive) ChasePlayer();
            else if (enemyType!=EnemyType.Boss)Patrolling();
            Vector3 distanceToWalkPoint = transform.position - _walkPoint;
            if (distanceToWalkPoint.magnitude < 3.5f) _walkPointSet = false;
        }
    }
    
    void SearchWalkPoint()
    {
        float randomZ = Random.Range(-_walkPointRange, _walkPointRange);
        float randomX = Random.Range(-_walkPointRange, _walkPointRange);
        Vector3 transformPosition = transform.position;
        _walkPoint = new Vector3(transformPosition.x + randomX, transformPosition.y, transformPosition.z + randomZ);
        if (Physics.Raycast(_walkPoint, -transform.up, 2f, _groundLayer)) _walkPointSet = true;
    }

    void Patrolling()
    {
        if (!_walkPointSet) SearchWalkPoint();
        else _agent.SetDestination(_walkPoint);
    }

    void ChasePlayer()
    {
        if (once && enemyType==EnemyType.Boss)
        {
            once = false;
            GameManager.audioManager.Play("BossTheme");
        }
        _agent.SetDestination(_player.position);
    }

    private void AttackPlayer(EnemyType enemyType)
    {
        if(Time.time - _lastAttackTime <= _attackCooldown)return;
        switch (enemyType)
        {
            case EnemyType.Melee:
                meleeAttack();
                break;
            case EnemyType.Ranged:
                rangedAttack();
                break;
            case EnemyType.Guard:
                meleeAttack();
                break;  
            case EnemyType.Boss:
                if(Physics.CheckSphere(transform.position, _attackRange / 5, _playerLayer))meleeAttack();
                else if (bossSecondPhase && !Physics.CheckSphere(transform.position, _attackRange / 1.3f, _playerLayer)) rangedAttack();
                break;
        }
    }

    private void meleeAttack()
    {
        PlayerManager.AddHealth(Random.Range(-8, -15));
        animator.SetTrigger("swip");
        _lastAttackTime = Time.time;
        AudioSource audiosource = gameObject.AddComponent<AudioSource>();
        GameManager.audioManager.Play("meleeAttack", audiosource);
    }

    private IEnumerator WaitFire()
    {
        yield return new WaitForSeconds(1.25f);
        Transform thisTransform = transform;
        Rigidbody bulletClone = Instantiate(enemyBullet, thisTransform.position + new Vector3(0, 1f, 0), thisTransform.rotation);
        Vector3 bulletDirection = _player.position - transform.position;
        bulletClone.AddForce(bulletDirection.normalized * _bulletSpeed);
    }
    private void rangedAttack()
    {
        animator.SetTrigger("swip");
        StartCoroutine(WaitFire());
        AudioSource audiosource2 = gameObject.AddComponent<AudioSource>(); // should be fixed now
        GameManager.audioManager.Play("rangeAttack", audiosource2); // should be fixed now
        _lastAttackTime = Time.time;
    }

    private void Death()
    {
        isAlive = false;
        animator.SetTrigger("death");
        miniMapIcon.enabled = false;
        GameManager.EnemyKilled(gameObject);
        if (getItem != null)
        {
            getItem.DropItem();
            Debug.Log("Dropped an Item " + getItem);
        }
        if(enemyType==EnemyType.Boss)
        {
            GameManager.GameWon();
        }
    }

    public void ReduceHealth(int amount, Enemy enemy)
    {
        if (enemy == this && isAlive)
        {
            currentHealth -= amount;
            healthBar.UpdateHealthBar(); //- Disabled, it fucks things up after killing the first enemy
            if (enemyType == EnemyType.Boss && currentHealth < maxHealth / 2)
            {
                bossSecondPhase = true;
                _agent.speed = 3;
                _attackCooldown = 1.5f;
            }
        }
    }

    private void SetStats()
    {
        switch (enemyType)
        {
            case EnemyType.Melee:
                _sightRange = 10f;
                _walkPointRange = 6f;
                _attackRange = 2f;
                _attackCooldown = 4f;
                currentHealth=maxHealth = 60;
                break;
            case EnemyType.Ranged:
                _sightRange = 14f;
                _walkPointRange = 6f;
                _attackRange = 10f;
                _attackCooldown = 5f;
                currentHealth=maxHealth = 90;
                break;
            case EnemyType.Guard:
                _sightRange = 20f;
                _walkPointRange = 0f;
                _attackRange = 2.5f;
                _attackCooldown = 6f;
                currentHealth=maxHealth = 120;
                break;
            case EnemyType.Boss:
                _sightRange = 24f;
                _walkPointRange = 12f;
                _attackRange = 14f;
                _attackCooldown = 3f;
                currentHealth=maxHealth = 400;
                break;
        }
    }
    private void SaveEnemies(GameManager.SaveType saveType)
    {
        PlayerPrefs.SetInt(name + "_currentHealth", currentHealth);
        PlayerPrefs.SetInt(name + "_isAlive", isAlive ? 1 : 0);
        if(enemyType==EnemyType.Boss)PlayerPrefs.SetInt(name + "bossSecondPhase", bossSecondPhase ? 1 : 0);
        PlayerPrefs.Save();
    }
    
    private void LoadEnemies(GameManager.GameLevel level)
    {
        if (PlayerPrefs.GetInt("SaveExists") == 1)
        {
            Debug.Log("Loading saved enemies");
            currentHealth = PlayerPrefs.GetInt(name + "_currentHealth");
            isAlive = PlayerPrefs.GetInt(name + "_isAlive") == 1; 
            if (enemyType == EnemyType.Boss)bossSecondPhase = PlayerPrefs.GetInt(name + "bossSecondPhase") == 1;
            if (!isAlive) 
            { 
                animator.SetTrigger("death"); 
                miniMapIcon.enabled = false;
            }
            else 
            { 
                animator.SetTrigger("alive"); 
                miniMapIcon.enabled = true;
            }
        }
        else
        {
            Debug.Log("Loading new enemies");
            if (enemyLevel >= level) 
            { 
                currentHealth = maxHealth; 
                isAlive = true; 
                animator.Play("Walking tree");
                animator.SetTrigger("alive"); 
                miniMapIcon.enabled = true; 
                if (enemyType == EnemyType.Boss) bossSecondPhase = false;
            }
            else 
            { 
                animator.SetTrigger("death"); 
                miniMapIcon.enabled = false; 
                currentHealth = 0; 
                isAlive = false; 
                if (enemyType == EnemyType.Boss) bossSecondPhase = false;
            }
        }
    }
}
