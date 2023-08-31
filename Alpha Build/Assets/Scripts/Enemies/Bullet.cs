using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class Bullet : MonoBehaviour
{
    public GameObject explosion;
    private readonly float _vanishingTime = 1;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerBody"))
        {
            PlayerManager playerManager  = other.GetComponent<PlayerManager>();
            PlayerManager.AddHealth(Random.Range(-5,-10));
            Destroy(gameObject);
            var noob = Instantiate(explosion, other.transform.position, other.transform.rotation);
            Destroy(noob, 1);
        }
        
        }

    private void OnEnable()
    {
        AudioSource audiosource = gameObject.AddComponent<AudioSource>();
        GameManager.audioManager.PlayLocal("Fireball", audiosource);
        StartCoroutine(vanishingTime());
    }

    private IEnumerator vanishingTime()
    {
        yield return new WaitForSeconds(_vanishingTime);
        Destroy(gameObject);
    }
}
