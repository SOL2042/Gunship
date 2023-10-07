using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class USBase : UnitData
{
    #region ΩÃ±€≈Ê ∆–≈œ
    private static USBase _instance;
    public static USBase instance
    {
        get
        {
            if (_instance == null) _instance = FindObjectOfType<USBase>();
            return _instance;
        }
    }
    #endregion
    public USBase()
    {
        totalData = new USBase_Status();
        myData = new USBase_Status();
    }
    void Start()
    {
        Refresh();
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
    public override void PostHit(WeaponData data)
    {

    }
}
