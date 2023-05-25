using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit_Status_NoUnit : Unit_Status
{
    public Unit_Status_NoUnit()
    {
        unitType = UnitType.Default;
        unitSprite = null;
        isSelected = false;
        unitSideType = 0;
        code = "";
        unitName = "";
        unitDescripts = "";

        maxHp = 0;
        currentHp = 0;
        def = 0;

        unitSpeed = 0;
        unitshootRange = 0;

        misileAtk = 0;
        rocketAtk = 0;
        bulletAtk = 0;

        bulletRPM = 0;
        rocketRPM = 0;
        misileRPM = 0;
    }
}
