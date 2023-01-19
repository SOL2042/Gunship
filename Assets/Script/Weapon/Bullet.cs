using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Vector3 normal;
    private float speed;
   
    public void Shoot(Vector3 normal, float speed)
    {
        this.normal = normal;
        this.speed = speed;
    }

    private void Awake()
    {
       
    }
    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward);
    }
}
