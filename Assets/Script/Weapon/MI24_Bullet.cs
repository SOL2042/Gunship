using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MI24_Bullet : MonoBehaviour
{

    private float speed;
    Rigidbody rgb;
    GameObject gunEffect;

    public Vector3 target;
    public bool hit;
    

    public void Speed(float speed)
    {
        this.speed = speed;
    }
    private void Start()
    {
        rgb = GetComponent<Rigidbody>();

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