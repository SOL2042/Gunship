using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitCell
{
    private int unitCount;
    private Unit_Status unit_Status;

    public UnitCell()
    {
        unit_Status = new Unit_Status_NoUnit();   
    }
}
