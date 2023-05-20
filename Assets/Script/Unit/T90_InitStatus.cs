using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T90_InitStatus : Unit_Status
{
    public T90_InitStatus()
    {
        isSelected = false;
        unitSideType = 0;
        code = "0001";
        unitName = "T90";
        unitDescripts = "T90, enemy";

        maxHp = 1000;
        currentHp = 1000;
        def = 5;
        
        unitSpeed = 20;
        unitshootRange = 100;

        misileAtk = 0;
        rocketAtk = 0;
        bulletAtk = 10;
        
        bulletRPM = 0;
        rocketRPM = 0;
        misileRPM = 0;
    }
}
