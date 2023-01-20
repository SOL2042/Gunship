using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HellFire_Missile : MonoBehaviour
{
    //초기속도
    //유도될 대상

    Transform target;
    float turningForce;

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
        Destroy(gameObject, lifeTime);
    }
    private void Update()
    {
        if(speed < MaxSpeed)
        {
            speed += accelAmount * Time.deltaTime;
        }
        transform.Translate(0, 0, speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other);
        Destroy(gameObject);
    }
    




}
