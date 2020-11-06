using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (playerControl))]

public class GravityAffectedPlayer : GravityAffectedEntity {
    public GameObject ourCamera;

    public float rotationSpeed = 120.0f;
    // public float translationSpeed = 10.0f;  
    // public float height = 2.0f;             //height from ground level
    private Transform centre; //transform for planet
    private float radius; //calculated radius from collider
    public SphereCollider planetCollider; //collider for planet

    private playerControl ourPlayer;

    void Start () {
        ourPlayer = GetComponent<playerControl> ();
        ourPlayer.ourGravityPlayer = this;

        if (!ourPlayer.photonView.IsMine) return;

        if (planetCollider == null) {
            centre = transform;
            radius = 0;
        } else {
            //consider scale applied to planet transform (assuming uniform, just pick one)
            radius = planetCollider.radius * planetCollider.transform.localScale.y;
            centre = planetCollider.transform;

        }

    }

    public override void FixedUpdate () {

        if (!ourPlayer.photonView.IsMine || transform == null || centre == null) return;

        Vector3 surfaceNormal = transform.position - centre.position;
        surfaceNormal.Normalize ();

        transform.rotation = Quaternion.FromToRotation (transform.up, surfaceNormal) * transform.rotation;

    }
    public override void SetGravityObject (GameObject go) {
        if (go.transform == null) return;

        base.SetGravityObject (go);
        planetCollider = go.transform.GetComponent<SphereCollider> ();
        radius = planetCollider.radius * planetCollider.transform.localScale.y;
        centre = planetCollider.transform;
    }
}