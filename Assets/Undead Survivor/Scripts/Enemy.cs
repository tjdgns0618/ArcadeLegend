using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Enemy : MonoBehaviour
{
    public float speed;
    public float health;
    public float maxHealth;
    public float time = 2f;
    public float damageTime = 0f;

    public RuntimeAnimatorController[] animCon;
    [HideInInspector]
    public Rigidbody2D target;
    public GameObject exp;
    public Transform[] targets;
    public Ease ease;

    bool isLive;
    public bool isElite;

    Rigidbody2D rigid;
    Collider2D coll;
    SpriteRenderer spriter;
    Animator anim;
    WaitForFixedUpdate wait; // 다음 fixedupdate까지 기다린다

    // Start is called before the first frame update
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        spriter = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        wait = new WaitForFixedUpdate();        
    }

    private void FixedUpdate()
    {
        if (!isLive || anim.GetCurrentAnimatorStateInfo(0).IsName("Hit"))
            return;
        if (GameManager.instance.waveOn != false)
            this.gameObject.layer = 6;
        if (!GameManager.instance.isLive || this.gameObject.layer == 7)
            return;
        

        Vector2 dirVec = target.position - rigid.position;
        Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + nextVec);
        rigid.velocity = Vector2.zero;

        damageTime += Time.fixedDeltaTime;
    }

    private void LateUpdate()
    {
        if (!isLive)
            return;
        if (!GameManager.instance.isLive)
            return;        

        spriter.flipX = target.position.x < rigid.position.x;
    }

    private void OnEnable()
    {
        target = GameManager.instance.player.GetComponent<Rigidbody2D>();
        isLive = true;        
        coll.enabled = true;
        rigid.simulated = true;
        spriter.sortingOrder = 2;
        anim.SetBool("Dead", false);
        health = maxHealth;
        isElite = false;
    }

    public void Init(SpawnData data)
    {
        anim.runtimeAnimatorController = animCon[data.spriteType];
        speed = data.speed;
        maxHealth = data.health;
        health = data.health;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Bullet") || !isLive || this.gameObject.layer == 7)
            return;

        if (collision.CompareTag("Bullet"))
        {
            health -= collision.GetComponent<Bullet>().damage;
            StartCoroutine(KnockBack(3));

            if (health > 0)
            {       // hit action
                anim.SetTrigger("Hit");
                AudioManager.instance.PlaySfx(AudioManager.Sfx.Hit);
            }
            else
            {       // dead action
                isLive = false;
                coll.enabled = false;
                rigid.simulated = false;
                spriter.sortingOrder = 1;
                anim.SetBool("Dead", true);
                GameManager.instance.killCount++;
                if(GameManager.instance.isLive)
                    AudioManager.instance.PlaySfx(AudioManager.Sfx.Dead);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            GameManager.instance.health -= 5f;
            Debug.Log("뎀지-5");
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (this.gameObject.layer == 7)
            return;

        if (collision.CompareTag("Lava") && damageTime > time)
        {
            if (health > 0)
            {
                anim.SetTrigger("Hit");
                health -= collision.GetComponent<Lava>().damage;
                AudioManager.instance.PlaySfx(AudioManager.Sfx.Hit);
            }
            else
            {       // dead action
                isLive = false;
                coll.enabled = false;
                rigid.simulated = false;
                spriter.sortingOrder = 1;
                anim.SetBool("Dead", true);
                GameManager.instance.killCount++;
                if (GameManager.instance.isLive)
                    AudioManager.instance.PlaySfx(AudioManager.Sfx.Dead);
            }
            damageTime = 0f;
        }
    }

    IEnumerator KnockBack(float knockpower)
    {
        yield return wait;  // 다음 하나의 물리 프레임 딜레이 주기
        // yield return new WaitForSeconds(2f);  // 2초 쉬기
        Vector3 playerPos = GameManager.instance.player.transform.position;
        Vector3 dirVec = transform.position - playerPos;
        rigid.AddForce(dirVec.normalized * knockpower, ForceMode2D.Impulse);
    }

    private void Dead()
    {
        gameObject.SetActive(false);
        GameObject temp = Instantiate(exp);
        temp.transform.position = transform.position;
        temp.transform.DOMove(targets[UnityEngine.Random.Range(0, 3)].transform.position, 0.5f).SetEase(ease);
    }
}
