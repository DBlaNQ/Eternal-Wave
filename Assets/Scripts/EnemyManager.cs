using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyManager : MonoBehaviour
{

    NavMeshAgent nma;

    float maxHealth = 0f;
    public float health = 0f;
    float damage = 0f;
    float speed = 0f;

    void Start()
    {
        nma = GetComponent<NavMeshAgent>();
        if (gameObject.CompareTag("Zombie"))
        {
            maxHealth = 50f;
            health = maxHealth;
            damage = 15f;
            speed = 1f;
        }    
    }

    void Update()
    {
        nma.destination = GameObject.Find("Player").transform.position;
        nma.speed = speed;

        if(health < 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerController>().health -= damage;
        }
    }
}
