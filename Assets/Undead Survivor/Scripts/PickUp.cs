using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PickUp : MonoBehaviour
{
    public GameObject player;
    public Vector3 target;
    private void Update()
    {
        target = player.transform.position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Exp")
        {
            collision.transform.DOMove(target, 0.5f);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Exp")
        {
            collision.transform.DOMove(target, 0.5f);
        }
    }
}
