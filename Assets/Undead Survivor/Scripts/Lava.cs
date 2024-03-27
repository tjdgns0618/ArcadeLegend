using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lava : MonoBehaviour
{
    public ItemData LavaBottle;
    public float damage = 0.5f;
    public ParticleSystem fireEffect;

    private void Awake()
    {
        damage = GameObject.Find("Weapon 5").GetComponent<Weapon>().damage;
        var main = fireEffect.main;
        main.duration = 1.5f;
    }
}