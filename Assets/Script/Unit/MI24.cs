using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MI24 : UnitData
{
    public MI24()
    {
        totalData += new Mi24_Status();
        myData += new Mi24_Status();
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public override void PostHit(UnitData data, RaycastHit hit)
    {

    }

    public override void SetHit(UnitData data)
    {

    }



}
