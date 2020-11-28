using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float lifespan;
    public int damage;

    public playerControl initiatingPlayer;

    // Update is called once per frame
    void Update()
    {
        lifespan -= Time.deltaTime;

        if(lifespan <= 0){
            KillThisObject();
        
        }
    }

    protected virtual void OnCollisionEnter(Collision collisionInfo)
    {
        KillThisObject();

        DamageableEntity d = collisionInfo.collider.GetComponent<DamageableEntity>();
        if(d != null){
            // initiatingPlayer.DealDamage(d, damage);

            d.TakeDamage(damage, "");
        }
    }

    private void KillThisObject(){
        gameObject.GetComponent<Collider>().enabled = false;
        gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        gameObject.GetComponent<GravityAffectedEntity>().enabled = false;
        
        Destroy(gameObject, 1f);
    }
}
