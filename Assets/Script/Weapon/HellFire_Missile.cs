using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HellFire_Missile : MonoBehaviour
{
    //�ʱ�ӵ�
    //������ ���

    Transform target;
    float turningForce;

    public float MaxSpeed;
    public float accelAmount;
    public float lifeTime;
    float speed;

    public void Launch(Transform target, float launchSpeed, int layer) //target : ���� ��� launchSpeed : �ʱ�ӵ� layer: ����� ���̾�
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
