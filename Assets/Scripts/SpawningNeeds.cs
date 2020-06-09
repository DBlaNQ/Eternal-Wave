using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SpawningNeeds : MonoBehaviour
{

    [SerializeField] GameObject roundTimer = null;

    [SerializeField] GameObject zombie = null;
    [SerializeField] GameObject ammoPack = null;
    [SerializeField] GameObject healthPack = null;
    GameObject[] spawners;
    GameObject[] eSpawners;

    bool isPCalled = false;
    bool isECalled = false;

    float time;

    int enemyNum = 1;

    private void Start()
    { 
        spawners = GameObject.FindGameObjectsWithTag("Spawner");
        eSpawners = GameObject.FindGameObjectsWithTag("EnemySpawner");
        time = 20f;
    }

    private void Update()
    {
        if (!isPCalled)
        {
            StartCoroutine(Spawn(5f));
            isPCalled = true;
        }
        if (!isECalled)
        {
            StartCoroutine(SpawnEnemy(time));
            isECalled = true;
        }

        time -= Time.deltaTime;
        if (time < 0) time = 20f;
        roundTimer.GetComponent<TextMeshProUGUI>().text = Math.Round(time).ToString();
    }

    IEnumerator Spawn(float time)
    {
        yield return new WaitForSeconds(time);
        var randomN = UnityEngine.Random.Range(1, 7);
        var randomT = UnityEngine.Random.Range(1, 10);
        if(randomT <= 5)
        {
            //Ammo
            Instantiate(ammoPack, spawners[randomN].transform.position, Quaternion.identity);
        }
        else
        {
            //HealthPacks
            Instantiate(healthPack, spawners[randomN].transform.position, Quaternion.identity);
        }
        isPCalled = false;
    }

    IEnumerator SpawnEnemy(float time)
    {
        yield return new WaitForSeconds(time);
        for (int i = 0; i < enemyNum; i++)
        {
            var randomES = UnityEngine.Random.Range(1, 4);
            Instantiate(zombie, eSpawners[randomES].transform);
        }
        isECalled = false;
        enemyNum += 2;
    }
}
