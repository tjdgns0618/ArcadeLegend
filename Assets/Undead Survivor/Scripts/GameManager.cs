using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [Header("# Game Control")]
    public bool isLive;
    public float gameTime = 0;
    public float maxGameTime = 2 * 10f;
    public float gameVolume;
    public int stageLevel;
    public bool gamePaused = false;
    public bool bossSpawn;
    public bool waveOn;
    [Header("# Player Info")]
    public float health;
    public float maxHealth = 100;
    public int level;
    public int killCount;
    public int exp;
    public int[] nextExp = { 3, 5, 10, 30, 60, 100, 150, 210, 280 };
    public int playerId;
    public float skillCool;
    public float skillTimer;
    [Header("# Game Object")]
    public PoolManager pool;
    public Player player;
    public PauseManager pauseManager;
    public LevelUp uiLevelUp;
    public Result uiResult;
    public GameObject Unlock;
    public GameObject enemyCleaner;
    public Slider volumeSlider;
    public Slider SfxSlider;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        Stop();
        isLive = true;
        stageLevel = PlayerPrefs.GetInt("Stage");
        volumeSlider.value = PlayerPrefs.GetFloat("BgmVolume");
        gameVolume = PlayerPrefs.GetFloat("BgmVolume");
        SfxSlider.value = PlayerPrefs.GetFloat("SfxVolume");
    }


    public void GameStart(int id)
    {
        playerId = id;
        health = maxHealth;

        player.gameObject.SetActive(true);
        if (playerId == 1)
        {
            uiLevelUp.Select(1);
            uiLevelUp.Select(6);
        }
        else if (playerId == 0)
        {
            uiLevelUp.Select(6);
            uiLevelUp.Select(7);
        }

        AudioManager.instance.PlayBgm(true);
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Select);

        waveOn = false;
    }

    public void GameOver()
    {
        StartCoroutine(GameOverRoutine());
    }

    IEnumerator GameOverRoutine()
    {
        isLive = false;

        yield return new WaitForSeconds(0.5f);

        uiResult.gameObject.SetActive(true);
        uiResult.Lose();
        Stop();

    }

    public void GameVictory()
    {
        StartCoroutine(GameVictoryRoutine());
    }

    IEnumerator GameVictoryRoutine()
    {
        isLive = false;
        enemyCleaner.SetActive(true);

        yield return new WaitForSeconds(1f);

        uiResult.gameObject.SetActive(true);
        uiResult.Win();

        if (stageLevel == 1)
        {
            yield return new WaitForSeconds(1f);
            SceneManager.LoadScene("EndingScene", LoadSceneMode.Single);
        }
            

        if (!PlayerPrefs.HasKey("Clear"))
        {
            Unlock.gameObject.SetActive(true);
        }

        PlayerPrefs.SetInt("Clear", 1);
        Stop();

        AudioManager.instance.PlayBgm(false);
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Win);        
    }

    public void Invoke_Retry()
    {
        Time.timeScale = 1f;
        Invoke("Retry", 0.2f);
    }

    public void Retry()
    {
        SceneManager.LoadScene("Essential", LoadSceneMode.Single);
        SceneManager.LoadScene("Stage" + ((PlayerPrefs.GetInt("Stage")+1).ToString()), LoadSceneMode.Additive);
    }

    void Update()
    {
        if (!isLive)
            return;

        gameTime += Time.deltaTime;
    }

    public void GetExp()
    {
        if (!isLive)
            return;

        exp++;
        if(exp == nextExp[Mathf.Min(level, nextExp.Length-1)])
        {
            level++;
            exp = 0;
            uiLevelUp.Show();
        }
    }

    public void Stop()
    {
        isLive = false;
        Time.timeScale = 0f;
    }

    public void Resume()
    {
        isLive = true;
        Time.timeScale = 1f;
    }
}
