using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitCell
{
    public int unitCost = 0;               // 유닛셀에 있는 유닛 숫자
    public Unit_Status unit_Status;         // 유닛의 정보를 받아오는 변수

    public UnitCell()
    {
        unit_Status = new Unit_Status_NoUnit();   //변수 초기화
    }
}
