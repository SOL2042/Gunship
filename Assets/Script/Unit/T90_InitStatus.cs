using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T90_InitStatus : Unit_Status
{
    public T90_InitStatus()
    {
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
