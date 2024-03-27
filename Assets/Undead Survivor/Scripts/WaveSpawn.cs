using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawn : MonoBehaviour
{
    public Transform[] spawnPoint;
    public SpawnData[] spawnData;
    public float levelTime;

    int level;
    int wavePoint;
    float timer;
    float leveltimer;
    int spawnLength = 0;
    float waveTimer = 0;
    float waveCool = 5;

    private void Awake()
    {
        spawnPoint = GetComponentsInChildren<Transform>();
        levelTime = GameManager.instance.maxGameTime / spawnData.Length;

        wavePoint = Random.Range(1, spawnPoint.Length);
        GameManager.instance.waveOn = false;

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
        waveTimer += Time.deltaTime;

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

        if (timer > 0.1f && spawnLength <= 5)
        {
            timer = 0;
            Spawn();
            spawnLength++;
        }
        else if (spawnLength >= 5 && !GameManager.instance.waveOn)
        {
            GameManager.instance.waveOn = true;
            waveTimer = 0;
        }
        else if(waveTimer > waveCool && GameManager.instance.waveOn)
        {
            GameManager.instance.waveOn = false;
            spawnLength = 0;
            timer = 0;
        }
    }

    void Spawn()
    {
        GameObject enemy = GameManager.instance.pool.Get(0);
        enemy.transform.position = spawnPoint[wavePoint].position;
        enemy.layer = 7;
        enemy.transform.localScale = new Vector3(1, 1, 1);
        enemy.GetComponent<Enemy>().Init(spawnData[level]);
    }
}
