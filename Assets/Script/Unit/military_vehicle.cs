using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class military_vehicle : MonoBehaviour
{
    Rigidbody rgb;
    // Start is called before the first frame update
    void Start()
    {
        rgb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        rgb.AddRelativeForce(Vector3.forward * Time.deltaTime * 2000);
    }
}
