using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

[RequireComponent (typeof (PhotonView))]

public class DamageableEntity : MonoBehaviourPunCallbacks, IPunObservable {
    public int health;

    public void TakeDamage (int damageAmount) {
        health -= damageAmount;

        if (health < 0) {
            Destroy (this.gameObject);

        }
    }

    public void OnPhotonSerializeView (PhotonStream stream, PhotonMessageInfo info) {
        if (stream.IsWriting) {
            //this is our character since it's in writing state
            // stream.SendNext()

            stream.SendNext (health);
        } else {
            //this is a network player because it's in Read, not write
            //stream.ReceiveNext();

            health = (int) stream.ReceiveNext ();
        }
    }
}