using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghosttrail : MonoBehaviour
{
    public GameObject ghostPrefab;
    public float delay = 1.0f;
    float delta = 0;

    SpriteRenderer spriteRender;
    public float destroyTime = 0.1f;
    public Color color;
    public Material material = null;

    private void Start()
    {

    }

    private void Update()
    {
        if (delta > 0) { delta -= Time.deltaTime; }
        else { delta = delay; createGhost(); }
    }

    void createGhost()
    {
        GameObject ghostObj = Instantiate(ghostPrefab, transform.position, transform.rotation);
        ghostObj.transform.localScale = transform.localScale;
        Destroy(ghostObj, destroyTime);

        spriteRender = ghostObj.GetComponent<SpriteRenderer>();
        spriteRender.sprite = spriteRender.sprite;
        spriteRender.color = color;
        if (material != null) spriteRender.material = material;
    }
}
