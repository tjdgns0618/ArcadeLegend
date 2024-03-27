using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gear : MonoBehaviour
{
    public ItemData.ItemType type;
    public float rate;

    float speed;

    public void Init(ItemData data)
    {
        // Basic Set
        name = "Gear " + data.itemId;
        transform.parent = GameManager.instance.player.transform;
        transform.localPosition = Vector3.zero;

        // Property Set
        type = data.itemType;
        rate = data.damages[0];
        ApplyGear();
    }

    public void LevelUp(float rate)
    {
        //Debug.Log(gameObject.transform.name);
        this.rate = rate;
        ApplyGear();
    }

    void ApplyGear()
    {
        switch (type)
        {
            case ItemData.ItemType.Glove:
                //Debug.Log("±€∑Ø∫Í ApplyGear " + gameObject.transform.name);
                RateUp();
                break;
            case ItemData.ItemType.Shoe:
                //Debug.Log("Ω¥¡Ó ApplyGear " + gameObject.transform.name);
                SpeedUp();
                break;
        }

        //Debug.Log("out");
    }

    void RateUp()
    {
        Weapon[] weapons = transform.parent.GetComponentsInChildren<Weapon>();

        foreach(Weapon weapon in weapons)
        {
            switch (weapon.id)
            {
                case 0:
                    float speed = 150 * Character.WeaponSpeed;
                    weapon.speed = -speed + (-speed * rate);
                    break;
                case 7:
                    speed = 1.5f * Character.WeaponRate;
                    weapon.whipspeed = speed * (1f-rate);
                    break;
                default:
                    speed = 0.5f * Character.WeaponRate;
                    weapon.speed = speed * (1f - rate);
                    break;
            }
        }
    }

    void SpeedUp()
    {
        speed = GameManager.instance.player.speed * Character.Speed;

        GameManager.instance.player.speed = speed + speed * rate;
    }

}
