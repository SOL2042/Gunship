using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankBullet : WeaponData
{
    private float speed;
    private float damage;
    GameObject gunEffect;

    public Vector3 target;
    public bool hit;
    
    public void Speed(ref float speed)
    {
        this.speed = speed;
    }
    public void Damage(ref float damage)
    {
        this.damage = damage;
    }
   
    private void Start()
    {
        gunEffect = Resources.Load<GameObject>("Prefabs/DustExplosion");
    }
    // Update is called once per frame
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
