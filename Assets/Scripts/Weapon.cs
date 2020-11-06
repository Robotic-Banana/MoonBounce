using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class Weapon : MonoBehaviour {
    public int damage;

    public float fireRate;

    public playerControl weaponOwner;

    private float remainingTimeTillFire = 0f;

    public GameObject weaponTrail;

    // Update is called once per frame
    public virtual void Update () {
        if (remainingTimeTillFire > 0f) {
            remainingTimeTillFire -= Time.deltaTime;

        }
    }

    public bool PullTheoreticalTrigger () {
        return remainingTimeTillFire <= 0f;

    }

    public virtual void FireWeapon () {
        remainingTimeTillFire = this.fireRate;

    }
}