using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponInventory : MonoBehaviour {
    public Weapon[] weaponList;
    public int activeWeaponIndex = 0;

    void Start () {
        SwapWeapon (activeWeaponIndex);

    }
    
    public void SwapWeapon (int index) {
        for (int i = 0; i < weaponList.Length; i++) {
            if (i == index) {
                weaponList[i].gameObject.SetActive (true);

            } else {
                weaponList[i].gameObject.SetActive (false);

            }
        }
    }
}