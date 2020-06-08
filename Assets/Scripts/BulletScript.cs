using System.Collections;
using System.Security.Cryptography;
using UnityEngine;

public class BulletScript : MonoBehaviour
{

    Rigidbody rb;
    float speed = 5f;
    float damage = 20f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (gameObject.CompareTag("Bullet"))
        {
            rb.velocity = transform.forward * speed;
        }
        else if (gameObject.CompareTag("BulletS"))
        {
            
            StartCoroutine(DestroyBullet(2f));
        }
    }

    IEnumerator DestroyBullet(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Zombie"))
        {
            Destroy(gameObject);
        }
        else
        {
            other.GetComponent<EnemyManager>().health -= damage;
            Destroy(gameObject);
        }
    }
}
