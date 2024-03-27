using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Luke : MonoBehaviour
{
    public GameObject Circle;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Circle.gameObject.SetActive(true);
            Circle.GetComponent<Animator>().SetTrigger("LukeAttack");
            transform.parent.GetComponent<Boss>().isattack = true;
        }
    }
}
