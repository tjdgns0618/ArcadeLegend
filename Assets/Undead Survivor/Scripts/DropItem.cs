using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItem : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {        
        if(collision.tag == "Player")
        {
            GameManager.instance.GetExp();
            gameObject.SetActive(false);
        }
    }

    private void OnDisable()
    {
        Destroy(gameObject, 1f);
    }


}
