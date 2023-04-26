using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnitData : MonoBehaviour
{
    public Unit_Status totalData = new Unit_Status();
    public Unit_Status myData = new Unit_Status();
    //public List<BuffSkill> buffSkill = new List<BuffSkill>();

    public abstract void PostHit(UnitData data, RaycastHit hit);
    public abstract void SetHit(UnitData data);
}
