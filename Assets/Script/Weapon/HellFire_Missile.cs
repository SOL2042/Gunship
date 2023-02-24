using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HellFire_Missile : MonoBehaviour
{
    //�ʱ�ӵ�
    //������ ���
    Rigidbody rgb;
    Transform target;
    float turningForce = 2.5f;

    GameObject explosionPrefab;

    public float MaxSpeed;
    public float accelAmount;
    public float lifeTime;
    float speed;
    float boresightAngle = 50;

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
        LookAtTarget();
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other);
        GameObject go = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        Destroy(go, 3);
        Destroy(gameObject);
    }


    void LookAtTarget()
    {
        if (target == null)
            return;

        Vector3 targetDir = target.position - transform.position;
        float angle = Vector3.Angle(targetDir, transform.forward);

        if (angle > boresightAngle)
        {
            target = null;
            return;
        }

        Quaternion lookRotation = Quaternion.LookRotation(targetDir);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, turningForce * Time.deltaTime);
    }


}
