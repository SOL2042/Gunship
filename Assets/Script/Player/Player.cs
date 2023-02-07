using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] Transform bulletPosition;
    [SerializeField] Transform enemyPosition;

    public float Speed
    {
        get
        {
            return speed;
        }
    }

    public float speed = 3;
    Rigidbody rgb;
    float rotSpeed = 100f;
   
    private float fireTimer = 0;


    HellFire_Missile hellFire;

    private void Awake()
    {
        rgb = gameObject.GetComponent<Rigidbody>();
    }
    private void Start()
    {
        

    }
    void Update()
    {
        Move();
        
        if (Input.GetMouseButton(0))
        {
            fireTimer += Time.deltaTime;
            if (fireTimer >= 0.1f)
            {
                Fire();
                fireTimer = 0;
                
            }
        }
        else
        {
            CeaseFire();
            fireTimer = 0;
        }
        
        
       
    }

    private void Move()
    {
        rgb.velocity = Vector3.Lerp(rgb.velocity, Vector3.zero, Time.deltaTime);
        
        if (Input.GetKey(KeyCode.A))
        {
            rgb.AddRelativeForce(new Vector3(-speed, 0, 0));
            
        }
        if (Input.GetKey(KeyCode.D))
        {
            rgb.AddRelativeForce(new Vector3(speed, 0, 0));
            
        }
        if (Input.GetKey(KeyCode.W))
        {
            rgb.AddRelativeForce(new Vector3(0, 0, speed));
        }

        if (Input.GetKey(KeyCode.S))
        {
            rgb.AddRelativeForce(new Vector3(0, 0, -speed));
        }

        if (Input.GetKey(KeyCode.Space))
        {
            rgb.AddForce(new Vector3(0, speed, 0));
        }

        if (Input.GetKey(KeyCode.LeftControl))
        {
            rgb.AddForce(new Vector3(0, -speed, 0));
        }

        if (Input.GetKey(KeyCode.C))
        {
            rgb.AddForce(new Vector3(0, -speed, 0));
        }

        if (Input.GetKey(KeyCode.Q))
        {
               
            gameObject.transform.Rotate(Vector3.Lerp(new Vector3(0, -rotSpeed * Time.deltaTime, 0), Vector3.zero, Time.deltaTime));

        }
        if (Input.GetKey(KeyCode.E))
        {
            gameObject.transform.Rotate(Vector3.Lerp(new Vector3(0, rotSpeed * Time.deltaTime, 0), Vector3.zero, Time.deltaTime));
        }
    }

    private void Fire()
    {
        gameObject.transform.GetChild(0).GetChild(2).gameObject.SetActive(true);
        GameObject go = Resources.Load<GameObject>("Prefabs/Bullet");

        //go.GetComponent<Bullet>().Shoot(bulletPosition.position + Vector3.forward, 10f);
       
        GameObject bullet = Instantiate(go, bulletPosition.position + new Vector3(Random.Range(-0.3f, 0.3f), Random.Range(-0.3f, 0.3f), 0), transform.rotation);
        bullet.layer = 6;
        Destroy(bullet, 2);
    }

    private void WeaponChange()
    {
        
    }


    private void CeaseFire()
    {
        gameObject.transform.GetChild(0).GetChild(2).gameObject.SetActive(false);
    }
    
    
}
