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

    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.forward * 10f;
    }
}
