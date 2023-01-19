using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HellFire_Missile : MonoBehaviour
{
    private void Start()
    {
        
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            transform.Translate(Vector3.forward * 0.1f);
        }
        
    }

}
