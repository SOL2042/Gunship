using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mi24_Status : Unit_Status
{
    public Mi24_Status()
    {
        maxHp = 5000f;
        currentHp = 5000f;
        def = 50f;

        unitSpeed = 20f;
        unitshootRange = 300f;

        isSelected = false;
        unitSideType = UnitSidetype.Default;
        flyMode = FlyMode.Default;
        unitType = UnitType.helicoptor;
        code = "0003";
        unitName = "Mi-24 Hind";
        unitDescripts = "Enemy Helicopter";
        unitSprite = null;
    }
    


}
