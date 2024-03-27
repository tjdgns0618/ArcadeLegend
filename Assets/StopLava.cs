using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopLava : MonoBehaviour
{
    private void OnParticleSystemStopped()
    {
        Destroy(transform.parent.gameObject) ;
    }
}
