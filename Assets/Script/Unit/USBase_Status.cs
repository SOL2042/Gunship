using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class USBase_Status : Unit_Status
{
    public USBase_Status()
    {
        isSelected = false;
        unitSideType = 0;
        code = "0003";
        unitName = "USBase";
        unitDescripts = "USBase";

        maxHp = 10000;
        currentHp = 10000;
        def = 50;

        unitshootRange = 100;

        misileAtk = 0;
        rocketAtk = 0;
        bulletAtk = 500;

        bulletRPM = 0;
        rocketRPM = 0;
        misileRPM = 0;
    }
}
