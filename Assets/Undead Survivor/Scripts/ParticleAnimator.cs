using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleAnimator : MonoBehaviour
{
    private double lasttime;
    private ParticleSystem particle;

    private void Awake()
    {
        particle = GetComponent<ParticleSystem>();
    }


    // Start is called before the first frame update
    void Start()
    {
        lasttime = Time.realtimeSinceStartup;
    }

    // Update is called once per frame
    void Update()
    {
        float deltaTime = Time.realtimeSinceStartup - (float)lasttime;

        particle.Simulate(deltaTime, true, false);

        lasttime = Time.realtimeSinceStartup;
    }
}
