using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaField : MonoBehaviour
{
    public GameObject Lava;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            GameObject temp = Instantiate(Lava, 
                transform.position, Quaternion.identity);
        }
    }
}
