using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class deadScript : MonoBehaviour
{

    
    
    
    void Start()
    {
       
        
    }

    
    void Update()
    {
        
    }


    private void OnTriggerEnter(Collider other)
    {
        GameObject deadEffect = Resources.Load<GameObject>("Prefabs/BigExplosion");
        GameObject go = Resources.Load<GameObject>("Prefabs/ROCKET_LCHR_DESTROYED");
        Instantiate(go,transform);
        Instantiate(deadEffect,transform);
        
        
    }
}
