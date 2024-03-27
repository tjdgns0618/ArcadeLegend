using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public Vector2 inputvec;
    public float speed;
    public Scanner scanner;
    Rigidbody2D rigid;
    SpriteRenderer spriter;
    Animator anim;
    public RuntimeAnimatorController[] animcon;
    public int playerId;

    public float lastHorizontalVector;

    // Start is called before the first frame update
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        scanner = GetComponent<Scanner>();
    }

    private void OnEnable()
    {
        speed *= Character.Speed;
        anim.runtimeAnimatorController = animcon[GameManager.instance.playerId];
        playerId = GameManager.instance.playerId;
    }

    private void FixedUpdate()
    {
        if (!GameManager.instance.isLive)
            return;

        Vector2 nextVec = inputvec.normalized * speed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + nextVec);

        if(inputvec.x != 0)
        {
            lastHorizontalVector = inputvec.x;
        }
    }

    void OnMove(InputValue value)
    {
        inputvec = value.Get<Vector2>();
    }

    private void LateUpdate()
    {
        if (!GameManager.instance.isLive)
            return;

        anim.SetFloat("Speed", inputvec.magnitude);

        if (inputvec.x != 0 )
        {
            spriter.flipX = inputvec.x < 0;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!GameManager.instance.isLive || collision.CompareTag("Obstacle"))
            return;                

        if (GameManager.instance.health <= 0)
        {
            for (int i = 2; i < transform.childCount; i++)
                transform.GetChild(i).gameObject.SetActive(false);

            anim.SetTrigger("Dead");
            GameManager.instance.GameOver();

            GameManager.instance.isLive = false;
        }
    }

    
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!GameManager.instance.isLive || collision.gameObject.CompareTag("Obstacle"))
            return;
        
        //Debug.Log("지속뎀지");

        GameManager.instance.health -= Time.deltaTime * 10f;

        if (GameManager.instance.health <= 0)
        {
            for (int i = 2; i < transform.childCount; i++)
                transform.GetChild(i).gameObject.SetActive(false);

            anim.SetTrigger("Dead");
            GameManager.instance.GameOver();

            GameManager.instance.isLive = false;

            AudioManager.instance.PlayBgm(false);
            
            AudioManager.instance.PlaySfx(AudioManager.Sfx.SwLose + GameManager.instance.playerId);
        }
    }
}
