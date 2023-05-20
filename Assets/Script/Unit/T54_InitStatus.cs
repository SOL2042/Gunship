using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T54_InitStatus : Unit_Status
{
    public T54_InitStatus()
    {
        isSelected = false;
        unitSideType = 0;
        code = "0002";
        unitName = "T54";
        unitDescripts = "T54, ally tank";

        maxHp = 1000;
        currentHp = 1000;
        def = 5;

        unitSpeed = 20;
        unitshootRange = 100;

        misileAtk = 0;
        rocketAtk = 0;
        bulletAtk = 500;

        bulletRPM = 0;
        rocketRPM = 0;
        misileRPM = 0;
    }
}
