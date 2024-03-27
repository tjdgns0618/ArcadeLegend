using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField]
    GameObject panel;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && Time.timeScale != 0)
        {
            if (panel.activeInHierarchy == false)
            {
                GameManager.instance.gamePaused = true;
                Pause();
            }
        }
    }

    public void Pause()
    {
        panel.SetActive(true);
        GameManager.instance.Stop();
    }

    public void UnPause()
    {
        GameManager.instance.gamePaused = false;
        panel.SetActive(false);
        GameManager.instance.Resume();
    }

}
