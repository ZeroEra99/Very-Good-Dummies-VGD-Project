using System.Collections;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    public GameObject explosion;
    private readonly float _vanishingTime = 1;
    public static int _minDamage = 30, _maxDamage=40;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            int damage = Random.Range(_minDamage, _maxDamage);
            enemy.ReduceHealth(damage,enemy);
            Destroy(gameObject);
            var noob = Instantiate(explosion, enemy.transform.position, enemy.transform.rotation);
            Destroy(noob, 1);
        }

    }

    private void OnEnable()
    {
        StartCoroutine(VanishingTime());
        AudioSource audiosource = gameObject.AddComponent<AudioSource>();
        GameManager.audioManager.PlayLocal("Fireball", audiosource);
    }

    private IEnumerator VanishingTime()
    {
        yield return new WaitForSeconds(_vanishingTime);
        Destroy(gameObject);
    }
}
