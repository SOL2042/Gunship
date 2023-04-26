using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankBullet : MonoBehaviour
{

    T90 t90;
    Transform bulletPosition;
    private float speed;
    Rigidbody rgb;
    GameObject gunEffect;

    public Vector3 target;
    public bool hit;
    
    public void Speed(ref float speed)
    {
        this.speed = speed;
    }
    public TankBullet(Vector3 target, bool hit)
    {
        this.target = target;
        this.hit = hit;
    }
    private void Awake()
    {
        //t90 = FindObjectOfType<T90>();
    }
    private void Start()
    {
        //rgb = GetComponent<Rigidbody>();
        //bulletPosition = t90.GetComponent<T90>().transform.GetChild(16).GetChild(0).GetChild(0).transform;

        gunEffect = Resources.Load<GameObject>("Prefabs/DustExplosion");

        //rgb.AddForce(bulletPosition.forward * 12000);


    }
    // Update is called once per frame
    void Update()
    {
        transform.Translate(new Vector3(0, 0, 200 * Time.deltaTime));

    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other);
        GameObject go = Instantiate(gunEffect, transform.localPosition, Quaternion.identity);
        Destroy(go, 3);
        Destroy(gameObject);
    }
}
