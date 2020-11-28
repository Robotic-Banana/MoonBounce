using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfettiLauncherProjectile : Projectile {
    protected override void OnCollisionEnter(Collision collisionInfo)
    {
        base.OnCollisionEnter(collisionInfo);

        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.GetComponent<Collider>().enabled = false;
        sphere.transform.position = collisionInfo.contacts[0].point;
        sphere.transform.localScale = Vector3.one * 4;
        sphere.GetComponent<Renderer>().material.color = Color.green;
        Destroy(sphere, 1f);

        Collider[] overlappedColliders = Physics.OverlapSphere(sphere.transform.position, 4f);
        
        DamageableEntity d;
        for(int i = 0; i < overlappedColliders.Length; i ++){
            d = overlappedColliders[i].GetComponent<DamageableEntity>();

            if(d != null){
                d.GetComponent<Rigidbody>().AddForce((d.transform.position - sphere.transform.position) * 10, ForceMode.Impulse);

                d.TakeDamage(damage , "");

            }
        }
    }
}