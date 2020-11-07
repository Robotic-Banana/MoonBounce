using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncePad : MonoBehaviour
{
    void OnCollisionEnter(Collision collisionInfo)
    {
        Rigidbody collisionRigidbody = collisionInfo.gameObject.GetComponent<Rigidbody>();
        if(collisionRigidbody != null){
            collisionRigidbody.AddForce(transform.up * 40, ForceMode.VelocityChange);

        }
    }
}
