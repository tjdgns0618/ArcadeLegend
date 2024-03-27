using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleButtonManager : MonoBehaviour
{
    public Slider volume_slider;
    public Slider Sfx_slider;

    private void Awake()
    {
        Time.timeScale = 1f;
        volume_slider.value = PlayerPrefs.GetFloat("BgmVolume");
        Sfx_slider.value = PlayerPrefs.GetFloat("SfxVolume");
    }

    private void Update()
    {
        PlayerPrefs.SetFloat("BgmVolume", volume_slider.value);
        PlayerPrefs.SetFloat("SfxVolume", Sfx_slider.value);
    }

    public void Stage1_invoke()
    {
        //Debug.Log("Stage1 Ω√¿€");
        Invoke("Stage1", 0.2f);
    }

    public void Stage2_invoke()
    {

        Invoke("Stage2", 0.2f);
    }

    public void Stage1()
    {
        SceneManager.LoadScene("Essential", LoadSceneMode.Single);
        SceneManager.LoadScene("Stage1", LoadSceneMode.Additive);
        Time.timeScale = 1f;
        PlayerPrefs.SetInt("Stage", 0);
    }
    public void Stage2()
    {
        SceneManager.LoadScene("Essential", LoadSceneMode.Single);
        SceneManager.LoadScene("Stage2", LoadSceneMode.Additive);
        Time.timeScale = 1f;
        PlayerPrefs.SetInt("Stage", 1);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
