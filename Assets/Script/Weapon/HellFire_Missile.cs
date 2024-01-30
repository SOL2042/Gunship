using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HellFire_Missile : WeaponData
{
    Rigidbody rgb;
    Transform target;
    float turningForce = 4f;

    GameObject explosionPrefab;

    public float MaxSpeed;
    public float accelAmount;
    public float lifeTime;
    float speed;
    float boresightAngle = 50;
    public float targetSearchSpeed;
    public float lockDistance;
    public void Damage(ref float damage)
    {
        this.damage = damage;
    }
    public void Launch(Transform target, float launchSpeed) //target : 유도 대상 launchSpeed : 초기속도 layer: 대상의 레이어
    {
        this.target = target;
        speed = launchSpeed;
    }
    private void Awake()
    {
        rgb = GetComponent<Rigidbody>();
    }
    private void Start()
    {
        explosionPrefab = Resources.Load<GameObject>("Prefabs/BigExplosion");
        Destroy(gameObject, lifeTime);
    }
    private void FixedUpdate()
    {
        LookAtTarget();
        if (speed < MaxSpeed)
        {
            speed += accelAmount * Time.fixedDeltaTime;
        }
        rgb.velocity = transform.forward * speed;
    }
    private void OnTriggerEnter(Collider other)
    {
        GameObject go = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        Destroy(go, 3);
        Destroy(gameObject);
    }
    private void OnDisable()
    {
        rgb.velocity = Vector3.zero;
        rgb.angularVelocity = Vector3.zero;
    }
    void LookAtTarget()
    {
        if (target == null)
        {
            return;
        }

        Vector3 targetDir = target.position - transform.position;
        float angle = Vector3.Angle(targetDir, transform.forward);

        if (angle > boresightAngle)
        {
            target = null;
            return;
        }
        if(target.gameObject.activeInHierarchy != true)
        {
            target = null;
        }
        Quaternion lookRotation = Quaternion.LookRotation(targetDir);
        rgb.rotation = Quaternion.Slerp(rgb.rotation, lookRotation, turningForce * Time.deltaTime);
    }
}
