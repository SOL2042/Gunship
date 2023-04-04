using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private GameObject t90;
    List<GameObject> t90s;


    private void Awake()
    {
        t90s = new List<GameObject>();
        t90 = Resources.Load("Prefabs/T90LP ForrestWavyCamo") as GameObject;
    }
    // Start is called before the first frame update
    void Start()
    {
        Instantiate(t90);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
