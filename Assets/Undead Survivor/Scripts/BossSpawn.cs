using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpawn : MonoBehaviour
{
    public Transform[] spawnPoint;
    public GameObject[] boss;
    public float levelTime;

    bool canSpawn = true;

    int level;
    float timer;

    private void Update()
    {
        if (GameManager.instance.gameTime > 40f && canSpawn)
        {
            SpawnBoss();
            canSpawn = false;
        }
    }

    void SpawnBoss()
    {
        GameObject enemy = Instantiate(boss[GameManager.instance.stageLevel]);
        enemy.transform.position = spawnPoint[Random.Range(1, spawnPoint.Length)].position;
        enemy.transform.localScale = new Vector3(1, 1, 1);
        GameManager.instance.bossSpawn = true;
    }

}
