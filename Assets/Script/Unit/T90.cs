using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T90 : MonoBehaviour
{
    Player target;
    GameObject turret;
    GameObject mainGun;
    Rigidbody rgb;
    float rotSpeed = 30;
    float timer = 0;
    void Start()
    {
        rgb = GetComponent<Rigidbody>();
        target = FindObjectOfType<Player>();
        turret = GameObject.Find("T90LP ForrestWavyCamo/Cube.017");
        mainGun = GameObject.Find("T90LP ForrestWavyCamo/Cube.017/Cube.018");
    }

    void Update()
    {
        rgb.velocity = Vector3.Lerp(rgb.velocity, Vector3.zero, Time.deltaTime);
        timer += Time.deltaTime;
        if (timer < 10)
        {
            rgb.AddRelativeForce(Vector3.forward * Time.deltaTime * 1000);
        }
        else if (timer > 10 && timer <= 13.1)
        {
            gameObject.transform.Rotate(Vector3.Lerp(new Vector3(0, -rotSpeed * Time.deltaTime, 0), Vector3.zero, Time.deltaTime));
            if(timer >= 13)
            {
                timer = 0;
            }
        }
        
        

        
        Aim();
    }

    private void Aim()
    {
        //포탑 회전
        //turret.transform.rotation = Quaternion.LookRotation(new Vector3(target.transform.position.x, 0 , target.transform.position.z));

        turret.transform.rotation = Quaternion.Euler(0, turret.transform.rotation.y, 0);
        turret.transform.LookAt(target.gameObject.transform);




        //주포각
        //mainGun.transform.rotation = Quaternion.LookRotation(new Vector3(0, target.transform.position.y, 0));

       // mainGun.transform.rotation = Quaternion.Euler(mainGun.transform.rotation.x, 0, 0);
       // mainGun.transform.LookAt(target.gameObject.transform);
        

    }


}
