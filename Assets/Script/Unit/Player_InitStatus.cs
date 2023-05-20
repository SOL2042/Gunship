using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_InitStatus : Unit_Status
{
    public Player_InitStatus()
    {
        isSelected = false;
        unitSideType = 0;
        code = "0000";
        unitName = "Player";
        unitDescripts = "Player";
        
        maxHp = 1000;
        currentHp = 1000;
        def = 0;

        unitSpeed = 10;
        

        misileAtk = 500;
        rocketAtk = 0;
        bulletAtk = 100;

        bulletRPM = 0;
        rocketRPM = 0;
        misileRPM = 0;
    }
}
