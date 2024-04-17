using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int id;
    public int prefabId;
    public float damage;
    public int count;
    public float temp;

    public float speed;
    float timer;
    
    float lavatimer;
    public float lavaspeed;

    public float whiptimer;
    public float whipspeed;

    public float gunnerskilltimer;
    public float gunnerskillspeed;

    public float bringerspeed;
    public float bringerTimer;

    Player player;

    private void Awake()
    {
        player = GameManager.instance.player;
    }

    void Update()
    {
        if (!GameManager.instance.isLive)
            return;

        switch (id){
            case 0:
                transform.Rotate(Vector3.back * speed * Time.deltaTime);
                break;
            case 5:
                lavatimer += Time.deltaTime;
                if (lavatimer > lavaspeed)
                {
                    lavatimer = 0f;
                    Fire();
                }
                break;
            case 6:
                GameManager.instance.skillCool += Time.deltaTime;
                if (GameManager.instance.skillCool > GameManager.instance.skillTimer 
                    && Input.GetKeyDown(KeyCode.Space) && player.playerId == 0)
                {
                    GameManager.instance.skillCool = 0f;
                    Fire();
                }                
                break;
            case 7:
                whiptimer += Time.deltaTime;
                if(whiptimer > whipspeed)
                {
                    whiptimer = 0;
                    Attack();
                }
                break;
            case 8:
                bringerTimer += Time.deltaTime;
                if(bringerTimer > bringerspeed)
                {
                    bringerTimer = 0;
                    TombStone();
                }
                break;
            default:
                timer += Time.deltaTime;
                if (timer > speed)
                {
                    timer = 0f;
                    Fire();
                }
                if (GameManager.instance.skillCool > GameManager.instance.skillTimer && Input.GetKeyDown(KeyCode.Space) && player.playerId == 1)
                {
                    GameManager.instance.skillCool = 0f;
                    FireAround();
                }
                break;
        }
    }

    public void LevelUp(float damage, int count)
    {

        this.damage = damage * Character.Damage; ;
        this.count += count;

        if (id == 0)
            Batch();

        player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
    }

    public void Init(ItemData data)
    {
        // Basic Set    Player의 자식 개체로 생성
        name = "Weapon " + data.itemId;
        transform.parent = player.transform;
        transform.localPosition = Vector3.zero;

        // Property Set
        id = data.itemId;
        damage = data.baseDamage * Character.Damage;
        count = data.baseCount;

        for(int index = 0; index < GameManager.instance.pool.prefabs.Length; index++)
        {
            if(data.projectile == GameManager.instance.pool.prefabs[index])
            {
                prefabId = index;
                break;
            }
        }

        switch (id)
        {
            case 0:
                speed = -150 * Character.WeaponSpeed;
                Batch();
                break;
            case 5:
                lavaspeed = 3f * Character.WeaponRate;
                break;
            case 6:
                GameManager.instance.skillTimer = 6f;
                break;
            case 7:
                whipspeed = 1.5f * Character.WeaponRate;
                break;
            case 8:
                bringerspeed = 3f * Character.WeaponRate;
                break;
            default:
                speed = 0.5f * Character.WeaponRate;
                gunnerskillspeed = 4f;
                break;
        }

        player.BroadcastMessage("ApplyGear",SendMessageOptions.DontRequireReceiver);
    }

    void Batch()
    {
        for(int index = 0; index < count; index++)
        {
            Transform bullet;
            
            if(index < transform.childCount)        // 기존 오브젝트 먼저 활용
            {
                bullet = transform.GetChild(index);     
            }
            else                                                    // 모자란 오브젝트 풀링에서 가져오기
            {
                bullet = GameManager.instance.pool.Get(prefabId).transform;
                bullet.parent = transform;
            }

            bullet.localPosition = Vector3.zero;
            bullet.localRotation = Quaternion.identity;

            Vector3 rotVec = Vector3.forward * 360 * index / count;
            bullet.Rotate(rotVec);
            bullet.Translate(bullet.up * 1.5f, Space.World);
            bullet.GetComponent<Bullet>().Init(damage, -100, Vector3.zero); // per는 관통을 의미 -100은 무한을 의미
        }
    }

    public void Fire()
    {
        if (!player.scanner.nearestTarget)
            return;

        Vector3 targetPos = player.scanner.nearestTarget.position;
        Vector3 dir = targetPos - transform.position;
        dir = dir.normalized;

        Transform bullet = GameManager.instance.pool.Get(prefabId).transform;
        bullet.position = transform.position;
        bullet.rotation = Quaternion.FromToRotation(Vector3.up, dir);
        bullet.GetComponent<Bullet>().Init(damage, count, dir);

        AudioManager.instance.PlaySfx(AudioManager.Sfx.Range);
    }

    public void Attack()
    {
        Transform bullet = GameManager.instance.pool.Get(prefabId).transform;
        if (player.lastHorizontalVector > 0)
        {            
            bullet.localScale = new Vector3(1, 1, 1);
        }
        else
            bullet.localScale = new Vector3(-1, 1, 1);

        bullet.GetComponent<Bullet>().Init(damage, count, Vector3.zero);
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Melee);
    }

    public void FireAround()
    {
        int roundNumA = 50;

        for (int i = 0; i < roundNumA; i++) {
            Transform bullet = GameManager.instance.pool.Get(prefabId).transform;
            bullet.position = transform.position;
            bullet.rotation = Quaternion.identity;

            Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
            Vector2 dirVec = new Vector2(Mathf.Cos(Mathf.PI * 2 * i / roundNumA),
                                                             Mathf.Sin(Mathf.PI * 2 * i / roundNumA));
            rigid.AddForce(dirVec.normalized * 5, ForceMode2D.Impulse);

            Vector3 rotVec = Vector3.forward * 360 * i / roundNumA + Vector3.forward*90;
            bullet.transform.Rotate(rotVec);
        }
    }

    public void TombStone()
    {
        if (!player.scanner.nearestTarget)
            return;

        Vector3 targetPos = player.scanner.nearestTarget.position;

        Transform bullet = GameManager.instance.pool.Get(prefabId).transform;
        bullet.position = targetPos;
        bullet.GetComponent<Bullet>().Init(damage, count, Vector3.zero);
    }
}
