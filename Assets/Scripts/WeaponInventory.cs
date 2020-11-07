using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class WeaponInventory : MonoBehaviourPun {
    public Weapon[] weaponList;
    public int activeWeaponIndex = 0;

    void Start () {
        SwapWeapon (activeWeaponIndex);

    }

    [PunRPC]
    public Weapon SwapWeapon (int index) {
        for (int i = 0; i < weaponList.Length; i++) {
            if (i == index) {
                weaponList[i].gameObject.SetActive (true);
                activeWeaponIndex = i;

            } else {
                weaponList[i].gameObject.SetActive (false);

            }
        }

        return weaponList[index];

    }
}