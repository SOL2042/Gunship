using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private static EnemyController _instance;

    public static EnemyController instance
    {
        get
        {
            if (_instance == null) _instance = FindObjectOfType<EnemyController>();
            return _instance;
        }
    }

    private GameObject t90;
    List<GameObject> t90s;
    [SerializeField] Transform resPosition;
    float RandomX;
    float RandomZ;
    float resTime = 3;
    float timer = 0;
    private void Awake()
    {
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
        RandomX = Random.Range(-400, 400);
        RandomZ = Random.Range(400, 401);
        timer += Time.deltaTime;
        if(timer >= resTime)
        {
            Instantiate();
            for (int i = 0; i < t90s.Count; i++)
            {
                t90s[i].SetActive(true);
            }
            timer = 0;
        }
    }

    private void Instantiate()
    {
        if (t90s.Count <= 8)
        {
            t90s.Add(Instantiate(t90, new Vector3(RandomX, 0, RandomZ), Quaternion.Euler(0,180,0)));
        }
    }


}
