using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MI24_Bullet : WeaponData
{
    Rigidbody rgb;
    GameObject gunEffect;

    public Vector3 target;
    public bool hit;

    public void Damage(ref float damage)
    {
        this.damage = damage;
    }
    private void Start()
    {
        rgb = GetComponent<Rigidbody>();
        gunEffect = Resources.Load<GameObject>("Prefabs/DustExplosion");
    }
    
    void Update()
    {
        transform.Translate(new Vector3(0, 0, 200 * Time.deltaTime));
    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject go = Instantiate(gunEffect, transform.localPosition, Quaternion.identity);
        Destroy(go, 3);
        Destroy(gameObject);
    }
}