using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    Player player;

    private Vector3 normal;
    private float speed;
    Rigidbody rgb;
    GameObject gunEffect;

    public Vector3 target;
    public bool hit;
    [SerializeField]
    Transform bulletPosition;
    

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
        player = FindObjectOfType<Player>();
        rgb = GetComponent<Rigidbody>();
        bulletPosition = player.transform.GetChild(0).GetChild(3).transform;
        
        gunEffect = Resources.Load<GameObject>("Prefabs/DustExplosion");

        rgb.AddForce(bulletPosition.forward * 12000);
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other);
        GameObject go = Instantiate(gunEffect, transform.position, Quaternion.identity);
        Destroy(go, 3);
        Destroy(gameObject);
    }
}
