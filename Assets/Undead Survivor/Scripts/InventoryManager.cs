using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public List<GameObject> weaponSlots = new List<GameObject>(6);
    public int[] weaponLevels = new int[6];


    public void AddWeapon(int slotindex, GameObject item)
    {
        weaponSlots[slotindex] = item;
    }

    public void AddGearItem(int slotindex, Weapon item)
    {
    }



}
