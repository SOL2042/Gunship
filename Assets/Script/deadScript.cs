using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class deadScript : MonoBehaviour
{
    GameObject deadEffect;
    GameObject go;



    void Start()
    {
        deadEffect = Resources.Load<GameObject>("Prefabs/BigExplosion");
        go = Resources.Load<GameObject>("Prefabs/ROCKET_LCHR_DESTROYED");
    }

    
    void Update()
    {
        
    }


    private void OnTriggerEnter(Collider other)
    {
        Instantiate(go,transform.position,Quaternion.identity);
        Instantiate(deadEffect,transform.position, Quaternion.identity);
        gameObject.SetActive(false);
        Destroy(gameObject);
    }
}
