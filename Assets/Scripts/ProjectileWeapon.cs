﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileWeapon : Weapon
{
    public override void FireWeapon(){
        base.FireWeapon();

        GameObject newProjectile = new GameObject();
        newProjectile.transform.position = transform.position;

        newProjectile.layer = LayerMask.NameToLayer("Bullet");
        
        Projectile p = newProjectile.AddComponent<Projectile>();
        p.lifespan = 10f;
        p.damage = damage;
        p.initiatingPlayer = weaponOwner;

        GameObject.Instantiate(weaponTrail, newProjectile.transform.position, Quaternion.identity, newProjectile.transform);
        
        Rigidbody r = newProjectile.AddComponent<Rigidbody>();
        r.useGravity = false;
        r.AddForce(transform.forward * 300, ForceMode.VelocityChange);
        newProjectile.AddComponent<GravityAffectedEntity>();
        SphereCollider s = newProjectile.AddComponent<SphereCollider>();
        s.radius = 0.1f;

    }
}
