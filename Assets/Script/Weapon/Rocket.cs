using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    float speed;
    Rigidbody rgb;
    GameObject explosionPrefab;

    public Vector3 target;
    public bool hit;

    public float MaxSpeed;
    public float accelAmount;
    public float lifeTime;

    public void Launch(float launchSpeed)
    {
        speed = launchSpeed;
    }
    private void Awake()
    {
        rgb = GetComponent<Rigidbody>();
    }
    void Start()
    {
        explosionPrefab = Resources.Load<GameObject>("Prefabs/BigExplosion");
        Destroy(gameObject, lifeTime);
       
    }
    private void FixedUpdate()
    {
        if (speed < MaxSpeed)
        {
            speed += accelAmount * Time.fixedDeltaTime;
        }
        rgb.velocity = transform.forward * speed;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    

    private void OnDisable()
    {
        rgb.velocity = Vector3.zero;
        
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other);
        GameObject go = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        Destroy(go, 3);
        Destroy(gameObject);
    }
}
