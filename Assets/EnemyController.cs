using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private GameObject t90;
    List<GameObject> t90s;
    [SerializeField] Transform resPosition;
    float x;
    float z;
    float resTime = 3;
    float timer = 0;
    private void Awake()
    {
        x = Random.Range(-100, 100);
        z = Random.Range(-5, 5);
        t90s = new List<GameObject>();
        t90 = Resources.Load("Prefabs/T90LP ForrestWavyCamo") as GameObject;
    }
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 8; i++)
        {
            t90s.Add(Instantiate(t90, resPosition.position, Quaternion.identity));
            t90s[i].SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if(timer >= resTime)
        {
            for (int i = 0; i < t90s.Count; i++)
            {
                if (timer >= resTime)
                {
                    if (t90s[i].activeInHierarchy != true)
                    {
                        t90s[i].SetActive(true);
                        timer = 0;
                    }
                    else
                    {
                        t90s[i+1].SetActive(true);
                        timer = 0;
                    }
                }
            }
        }
    }
}
