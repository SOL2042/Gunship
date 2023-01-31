using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HellFire_Missile : MonoBehaviour
{
    //초기속도
    //유도될 대상
    Rigidbody rgb;
    Transform target;
    float turningForce;

    GameObject explosionPrefab;

    public float MaxSpeed;
    public float accelAmount;
    public float lifeTime;
    float speed;

    public void Launch(Transform target, float launchSpeed, int layer) //target : 유도 대상 launchSpeed : 초기속도 layer: 대상의 레이어
    {
        this.target = target;
        speed = launchSpeed;
        gameObject.layer = layer;
    }

    private void Start()
    {
        explosionPrefab = Resources.Load<GameObject>("Prefabs/BigExplosion");
        rgb = GetComponent<Rigidbody>();
        Destroy(gameObject, lifeTime);
    }
    private void Update()
    {
        if(speed < MaxSpeed)
        {
            speed += accelAmount * Time.deltaTime;
        }
        transform.Translate(0, 0, speed * Time.deltaTime);
        gameObject.transform.LookAt(target);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other);

        GameObject go = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        Destroy(go, 3);

        Destroy(gameObject);
    }
    




}
