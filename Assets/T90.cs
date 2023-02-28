using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T90 : MonoBehaviour
{
    Player target;
    GameObject turret;
    GameObject mainGun;
    void Start()
    {
        target = FindObjectOfType<Player>();
        turret = GameObject.Find("T90LP ForrestWavyCamo/Cube.017");
        mainGun = GameObject.Find("T90LP ForrestWavyCamo/Cube.017/Cube.018");
    }

    void Update()
    {
        //transform.Translate(Vector3.forward * Time.deltaTime * 10);
        Aim();
    }

    private void Aim()
    {
        //포탑 회전
        //turret.transform.rotation = Quaternion.LookRotation(new Vector3(target.transform.position.x, 0 , target.transform.position.z));
       
        turret.transform.LookAt(target.gameObject.transform);
        turret.transform.rotation = Quaternion.Euler(0, turret.transform.rotation.y, 0);



        //주포각
        //mainGun.transform.rotation = Quaternion.LookRotation(new Vector3(0, target.transform.position.y, 0));
       
        mainGun.transform.LookAt(target.gameObject.transform);
        mainGun.transform.rotation = Quaternion.Euler(mainGun.transform.rotation.x, 0, 0);

    }


}
