using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationDestroy : MonoBehaviour
{
    Animator anim;
    private void Start()
    {
        anim = gameObject.GetComponent<Animator>();        
    }

    public void SetAttack()
    {
        transform.parent.parent.GetComponent<Animator>().SetTrigger("Attack");
    }

    public void LukeAttack()
    {
        transform.parent.parent.GetComponent<Animator>().SetTrigger("LukeAttack");

        transform.Find("FireballExplode").gameObject.SetActive(true);

        transform.parent.parent.GetComponent<Boss>().isattack = true;
    }

    public void LukeExplosive()
    {
        GameObject.Find("FireballExplode").transform.gameObject.SetActive(false);
        gameObject.SetActive(false);
        transform.parent.parent.GetComponent<Boss>().isattack = false;
    }

    public void PlaySound()
    {
        GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("SfxVolume");
        GetComponent<AudioSource>().Play();
    }

    public void Destroying()
    {
        Destroy(gameObject);
    }

    public void Deactive()
    {
        gameObject.SetActive(false);
    }

    public void DeactiveParent()
    {
        gameObject.transform.parent.gameObject.SetActive(false);
    }
}
