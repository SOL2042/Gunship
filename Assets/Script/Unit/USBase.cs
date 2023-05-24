using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class USBase : UnitData
{
    private static USBase _instance;

    public static USBase instance
    {
        get
        {
            if (_instance == null) _instance = FindObjectOfType<USBase>();
            return _instance;
        }
    }
    public USBase()
    {
        totalData = new USBase_Status();
        myData = new USBase_Status();
    }
    // Start is called before the first frame update
    void Start()
    {
        Refresh();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 15)
        {
            myData.currentHp = myData.currentHp - (100 - myData.def);
            Refresh();
            Die();
        }
    }
    private void Refresh()
    {
        totalData = new Unit_Status();
        totalData += myData;
    }

    private void Die()
    {
        if (totalData.currentHp <= 0)
        {
            gameObject.SetActive(false);
        }
    }
    public override void PostHit(UnitData data, RaycastHit hit)
    {

    }

    public override void SetHit(UnitData data)
    {

    }
}
