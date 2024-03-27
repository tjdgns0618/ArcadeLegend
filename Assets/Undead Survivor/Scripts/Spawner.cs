using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Transform[] spawnPoint;
    public SpawnData[] spawnData;
    public float levelTime;

    int level;
    float timer;
    float leveltimer;
    public bool canSpawn = true;

    private void Awake()
    {
        spawnPoint = GetComponentsInChildren<Transform>();
        levelTime = GameManager.instance.maxGameTime / spawnData.Length;
        if (GameManager.instance.stageLevel == 0)
            level = 0;
        else
            level = 3;
    }

    void Update()
    {
        if (!GameManager.instance.isLive)
            return;

        timer += Time.deltaTime;
        leveltimer += Time.deltaTime;

        if (leveltimer > 10)
        {
            leveltimer = 0;
            level++;
        }

        switch (GameManager.instance.stageLevel)
        {
            case 0:
                if (level > 2)
                    level = 2;
                break;
            case 1:
                if (level > 5)
                    level = 5;
                break;
        }
                
        if(timer > spawnData[level].spawnTime)
        {
            timer = 0;
            Spawn();
        }

    }

    void Spawn()
    {
        GameObject enemy = GameManager.instance.pool.Get(0);
        enemy.transform.position = spawnPoint[Random.Range(1, spawnPoint.Length)].position;
        enemy.transform.localScale = new Vector3(1, 1, 1);
        enemy.GetComponent<Enemy>().Init(spawnData[level]);
    }

    void SpawnElite()
    {
        GameObject enemy = GameManager.instance.pool.Get(0);
        enemy.transform.position = spawnPoint[Random.Range(1, spawnPoint.Length)].position;
        enemy.transform.localScale = new Vector3(2, 2, 2);
        enemy.GetComponent<Enemy>().Init(spawnData[level]);
        enemy.GetComponent<Enemy>().health = enemy.GetComponent<Enemy>().maxHealth * 3;
        enemy.GetComponent<Enemy>().isElite = true;
    }

}

[System.Serializable]
public class SpawnData
{
    public float spawnTime;
    public int spriteType;
    public int health;
    public float speed;
}
