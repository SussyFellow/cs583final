using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject enemyPrefab;
    public GameObject player;
    public Transform enemySpawnPoint;
    public float enemySpawnDelay;
    public int enemyLimit;
    [HideInInspector] public List<GameObject> enemyList; 
    float enemySpawnTimer;
    

    void Start()
    {
        enemyList = new List<GameObject>();
        enemySpawnTimer = enemySpawnDelay;
    }

    // Update is called once per frame
    void Update()
    {
        enemySpawnTimer -= Time.deltaTime;
        if (enemySpawnTimer < 0)
        {
            spawnEnemy();
        }
    }


    void spawnEnemy()
    {
        if (enemyList.Count < enemyLimit)
        {
            enemyList.Add(Instantiate(enemyPrefab, enemySpawnPoint.position, enemySpawnPoint.rotation));
            EnemyMain enemyScript = enemyList[^1].GetComponent<EnemyMain>(); 
            enemyScript.player = player;
            enemyScript.manager = this;
            enemySpawnTimer = enemySpawnDelay;
        }

    }
}
