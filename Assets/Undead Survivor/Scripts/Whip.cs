using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Whip : MonoBehaviour
{
    float timeToDisable = 0.4f;
    float timer;

    private void OnEnable()
    {
        timer = timeToDisable;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = GameManager.instance.player.transform.position;
    }

    private void LateUpdate()
    {
        timer -= Time.deltaTime;
        if(timer < 0f)
        {
            gameObject.SetActive(false);
        }
    }
}
