using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] Transform bulletPosition;
    float speed = 3;
    Rigidbody rgb;
    float rotSpeed = 100f;
    private float distance = 10f;
    private float angleX = 90f, angleY;
    private Vector3 cusVec;
    private float MaxVelocityX = 10f, MaxVelocityY = 10f, MaxVelocityZ= 10f;
    private float camSpeed = 200f;

    private float bulletTimer = 0;



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
        angleX -= Input.GetAxis("Mouse Y");
        angleY += Input.GetAxis("Mouse X");

        cusVec = new Vector3(Input.mousePosition.x - Screen.width / 2, 0, Input.mousePosition.y - Screen.height / 2).normalized;
        
        Movement();
        Move();
        Camera.main.transform.localPosition = gameObject.transform.localPosition + Vector3.back * 10 + Vector3.up * distance;
        Camera.main.transform.rotation = Quaternion.Euler(angleX, angleY, 0);

        

        if (Input.GetMouseButton(0))
        {
            bulletTimer += Time.deltaTime;
            if (bulletTimer >= 0.1f)
            {
                Fire();
                bulletTimer = 0;
            }
        }
        else
        {
            CeaseFire();
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
            transform.Rotate(new Vector3(0, -rotSpeed * Time.deltaTime, 0));
        }
        if (Input.GetKey(KeyCode.E))
        {
            transform.Rotate(new Vector3(0, rotSpeed * Time.deltaTime, 0));
        }
    }

    private void Fire()
    {
        gameObject.transform.GetChild(0).GetChild(2).gameObject.SetActive(true);
        GameObject go = Resources.Load<GameObject>("Prefabs/Bullet");
        Instantiate(go, bulletPosition);
        

    }
    private void CeaseFire()
    {
        gameObject.transform.GetChild(0).GetChild(2).gameObject.SetActive(false);
    }
    private void Movement()
    {
        Vector3 moveDir = Camera.main.transform.rotation * cusVec;
        
        moveDir.Normalize();
        Camera.main.transform.position += moveDir * camSpeed * Time.deltaTime;
    }
}
