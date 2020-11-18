using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

[RequireComponent (typeof (PhotonView))]

public class DamageableEntity : MonoBehaviourPunCallbacks, IPunObservable {
    public int health;

    private bool dying = false;

    public ParticleSystem ourParticleSystem;

    public void Update () {
        // Debug.LogWarning(ourParticleSystem);
        // Debug.LogWarning(dying);
        // Debug.LogWarning(ourParticleSystem.isPlaying);

        if (ourParticleSystem != null && dying && !ourParticleSystem.isPlaying) {
            ourParticleSystem.Stop ();
            ourParticleSystem.Play (true);
        }
    }

    public void TakeDamage (int damageAmount, string attackingPlayer) {
        if (dying) return;

        health -= damageAmount;

        if (health <= 0) {
            if (photonView.IsMine) {
                playerControl player = GetComponent<playerControl> ();

                if (player == null) {
                    PhotonNetwork.Destroy (this.gameObject);

                } else {
                    //player died routine, handle this

                    player.enabled = false;
                    dying = true;

                    player.ourRigidbody.AddForce (player.transform.up * 100, ForceMode.VelocityChange);

                    RespawnEntity ();

                }
            }
        }
    }

    public void OnPhotonSerializeView (PhotonStream stream, PhotonMessageInfo info) {
        if (stream.IsWriting) {
            //this is our character since it's in writing state
            // stream.SendNext()

            stream.SendNext (health);
            stream.SendNext (dying);
        } else {
            //this is a network player because it's in Read, not write
            //stream.ReceiveNext();

            health = (int) stream.ReceiveNext ();
            dying = (bool) stream.ReceiveNext ();

            // if(health <= 0)dying = true;

        }
    }

    public void RespawnEntity () {
        float respawnTimer = 5f;

        StartCoroutine (RespawnCoroutine (respawnTimer));

    }

    private IEnumerator RespawnCoroutine (float time) {
        while (time > 0f) {

            time -= Time.deltaTime;

            com.RoboticBanana.MoonBounce.GameManager.Instance.UpdateRespawnGraphic (Mathf.RoundToInt (time));

            yield return new WaitForEndOfFrame ();

        }

        gameObject.transform.position = com.RoboticBanana.MoonBounce.GameManager.Instance.PickRespawnPoint ().position;

        this.health = 20;
        dying = false;

        playerControl player = gameObject.GetComponent<playerControl> ();
        player.enabled = true;
        player.ourRigidbody.velocity = Vector3.zero;

    }
}