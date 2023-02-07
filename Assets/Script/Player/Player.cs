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

    

  
}
