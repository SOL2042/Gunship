using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HellFire_Missile : MonoBehaviour
{
    //�ʱ�ӵ�
    //������ ���
    Rigidbody rgb;
    Transform target;
    float turningForce;

    GameObject explosionPrefab;

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
