using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UnlockStage : MonoBehaviour
{
    public GameObject lockStage;
    public GameObject unlockStage;

    // Start is called before the first frame update
    void Start()
    {
        Unlock();
    }

    public void Unlock()
    {
        if (PlayerPrefs.HasKey("Clear"))
        {
            lockStage.SetActive(false);
            unlockStage.SetActive(true);
        }
    }

    public void Clear()
    {
        PlayerPrefs.SetInt("Clear", 1);
    }

    public void UnClear()
    {
        PlayerPrefs.DeleteKey("Clear");
    }
}
