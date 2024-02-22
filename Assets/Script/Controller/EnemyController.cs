using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    #region ΩÃ±€≈Ê
    private static EnemyController _instance;
    public static EnemyController instance
    {
        get
        {
            if (_instance == null) _instance = FindObjectOfType<EnemyController>();
            return _instance;
        }
    }
    #endregion

    private int _killCount;
    public int killCount
    {
        set
        {
            _killCount = value;
            
        }
        get
        {
            return _killCount;
        }
    }


    private GameObject t90; 
    public List<GameObject> t90s;
    [SerializeField] Transform t90ResPosition;
    public float RandomX;
    public float RandomZ;
    float level;
    public float waveRound;

    private GameObject mi24;
    public List<GameObject> mi24s;
    [SerializeField]
    Transform mi24ResPosition;

    float lastRespwanTime;
    float respwanInterval = 4;

    float resTime = 3;
    float timer = 0;
    private void Awake()
    {
        t90s = new List<GameObject>();
        
        t90 = Resources.Load("Prefabs/T90LP ForrestWavyCamo") as GameObject;
    }
    void Update()
    {
        Debug.Log($"killCount : {killCount}");
        if (killCount % 10 == 0)
        {
            LevelUp();
        }
        RandomX = Random.Range(-100, 100);
        RandomZ = Random.Range(800, 900);
        timer += Time.deltaTime;
        if (timer >= resTime)
        {
            Instantiate();
            Respwan();
            timer = 0;
        }
    }

    private void Respwan()
    {
        for (int i = 0; i < t90s.Count; i++)
        {
            if (t90s[i].gameObject.activeInHierarchy == false)
            {
                t90s[i].SetActive(true);
            }
        }
    }

    private void Instantiate()
    {
        if (t90s.Count <= 8 + level * 3)
        {
            t90s.Add(Instantiate(t90, new Vector3(RandomX, 0, RandomZ), Quaternion.Euler(0,180,0)));
        }
    }

    private void LevelUp()
    {
        level++;
    }
}
