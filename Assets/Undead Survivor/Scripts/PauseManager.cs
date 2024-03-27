using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public void Invoke_Exit()
    {
        Time.timeScale = 1f;
        Invoke("ExitGame", 0.2f);
    }

    public void ExitGame()
    {
        SceneManager.LoadScene("GameTitle");
    }

}
