using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Vector3 normal;
    private float speed;
    Rigidbody rgb;
    GameObject gunEffect;

    public Vector3 target;
    public bool hit;

    //public void Shoot(Vector3 normal, float speed)
    //{
    //    this.normal = normal;
    //    this.speed = speed;
    //}
    public Bullet(Vector3 target, bool hit)
    {
        this.target = target;
        this.hit = hit;
    }
    private void Awake()
    {
        
    }
    private void Start()
    {
        rgb = GetComponent<Rigidbody>();
        gunEffect = Resources.Load<GameObject>("Prefabs/DustExplosion");
    }
    // Update is called once per frame
    void Update()
    {
        if(gameObject.activeInHierarchy)
        transform.Translate(Vector3.forward * Time.deltaTime * 300);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other);
        
        GameObject go = Instantiate(gunEffect, transform.position, Quaternion.identity);
        Destroy(go, 3);

        Destroy(gameObject);
    }

}
