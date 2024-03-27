using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public float speed;
    public float health;
    public float maxHealth;
    public float time = 2f;
    public float damageTime = 0f;

    public Rigidbody2D target;
    public Transform[] targets;
    public GameObject laser;

    bool isLive;
    public bool isattack;
    public bool isElite;
    private float laserTimer = 0;
    private float laserTiming = 8f;

    Rigidbody2D rigid;
    Collider2D coll;
    SpriteRenderer spriter;
    Animator anim;
    WaitForFixedUpdate wait; // 다음 fixedupdate까지 기다린다

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

        if (!isattack)
            Move();

        damageTime += Time.fixedDeltaTime;

        if (GameManager.instance.stageLevel == 0)
        {
            laserTimer += Time.fixedDeltaTime;

            if (laserTimer > laserTiming)
            {
                laser.SetActive(true);
                laserTimer = 0;
            }
        }
    }

    private void LateUpdate()
    {
        if (!isLive)
            return;
        if (!GameManager.instance.isLive)
            return;

        spriter.flipX = target.position.x < rigid.position.x;

        if(GameManager.instance.stageLevel == 0)
            laser.transform.localScale = target.position.x < rigid.position.x ? new Vector3(-1,1,1) : new Vector3(1,1,1);
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Bullet") || !isLive || this.gameObject.layer == 7 )
            return;

        if (collision.CompareTag("Bullet"))
        {
            health -= collision.GetComponent<Bullet>().damage;
            StartCoroutine(KnockBack(1));

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
                if (GameManager.instance.isLive)
                    AudioManager.instance.PlaySfx(AudioManager.Sfx.Dead);
                GameManager.instance.GameVictory();
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
                               
                GameManager.instance.GameVictory();
            }
            damageTime = 0f;
        }
    }

    void Move()
    {
        Vector2 dirVec = target.position - rigid.position;
        Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + nextVec);
        rigid.velocity = Vector2.zero;
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
    }
}
