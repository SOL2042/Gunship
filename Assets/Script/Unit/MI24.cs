using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MI24 : UnitData
{
    Transform target;
    


    public MI24()
    {
        totalData += new Mi24_Status();
        myData += new Mi24_Status();
    }
    void Start()
    {
        target = FindObjectOfType<Player>().transform;
    }

    void Update()
    {
        Find();
    }

    private void Find()
    {
        float distance = Vector3.Distance(transform.position, target.position);

        transform.Translate(Vector3.forward * Time.deltaTime * 50);

    }
    public override void PostHit(UnitData data, RaycastHit hit)
    {

    }
    public override void SetHit(UnitData data)
    {

    }



}
