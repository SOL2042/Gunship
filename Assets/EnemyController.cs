using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private GameObject t90;
    List<GameObject> t90s;
    [SerializeField] Transform resPosition;
    float RandomX;
    float RandomZ;
    float resTime = 3;
    float timer = 0;
    private void Awake()
    {
        RandomX = Random.Range(-200, 200);
        RandomZ = Random.Range(-5, 5);
        t90s = new List<GameObject>();
        t90 = Resources.Load("Prefabs/T90LP ForrestWavyCamo") as GameObject;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RandomX = Random.Range(-200, 200);
        RandomZ = Random.Range(-5, 5);
        timer += Time.deltaTime;
        if(timer >= resTime)
        {
            if (t90s.Count <= 8)
            {
                Instantiate();
                for (int i = 0; i < t90s.Count; i++)
                {
                    t90s[i].SetActive(true);
                }
            }
        }
    }


    private void Instantiate()
    {
        for (int i = 0; i < 8; i++)
        {
            t90s.Add(Instantiate(t90, new Vector3(RandomX, 0, RandomZ), Quaternion.identity));

            t90s[i].SetActive(false);
        }
    }


}
