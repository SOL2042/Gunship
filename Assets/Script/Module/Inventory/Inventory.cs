using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    public UnitCell[] unitCell;                 // 유닛칸 배열

    public Inventory()                          // 인벤토리 초기화
    {
        unitCell = new UnitCell[5];             // 유닛칸 5개 생성
        Start();                                
    }

    public Inventory(int length)                // 외부에서 받아와서 유닛칸 생성
    {
        unitCell = new UnitCell[length];
        Start();
    }

    private void Start()                            // 유닛셀 배열마다 새 유닛셀 생성
    {
        for (int i = 0; i < unitCell.Length; i++)
        {
            unitCell[i] = new UnitCell();
        }
    }

    public void AddUnit(Unit_Status data, int count)                // 유닛 추가
    {
        int blankCell = -1;                                         // 비어있는 칸
        bool alreadyAdded = false;                                  // 이미 추가했는지 확인하는 변수

        for (int i = 0; i < unitCell.Length; i++)                   // 순서대로 유닛칸이 비어있는지 차있는지 전부 확인하는 반복문 
        {
            if (unitCell[i].unit_Status.code == "")                 // unitCell[i]의 코드가 없으면
            {
                if (blankCell == -1) blankCell = i;                 // blankCell이 -1 이면 i를 대입
            }
            else                                                    // unitCell[i]의 코드가 있으면
            {
                if (unitCell[i].unit_Status.code == data.code)      // unitCell[i]의 코드가 data의 코드와 같다면
                {
                    unitCell[i].unitCount += count;                 // unitCell[i]의 숫자에 매개변수 count를 더해줌
                    alreadyAdded = true;                            // 이미 추가되었다는걸로 변경
                    break;                                          // 반복문에서 벗어남
                }
            }
        }

        if (blankCell != -1)                                        // blankCell이 앞서 확인한 i면
        {
            if (!alreadyAdded)                                      // 이미 추가되지 않았다면
            {
                unitCell[blankCell].unit_Status = data;             // unitCell[i]의 유닛데이터에 매개변수 data
                unitCell[blankCell].unitCount = count;              // unitCell[i]의 유닛 숫자에 매개변수 count
            }
        }
        else                                                        // blankCell이 -1이면
        {
            if (!alreadyAdded)                                      // 이미 추가되지 않았다면                              
            {
                Debug.Log("유닛창이 꽉참");
            }
        }
    }

    public void RemoveItem(Unit_Status data, int count)
    {
        for (int i = 0; i < unitCell.Length; i++)
        {
            if (unitCell[i].unit_Status.code == data.code)
            {
                if (unitCell[i].unitCount <= count)
                {
                    count -= unitCell[i].unitCount;
                    unitCell[i].unit_Status = new Unit_Status_NoUnit();
                    unitCell[i].unitCount = 0;
                }
                else
                {
                    unitCell[i].unitCount -= count;
                    break;
                }
            }
        }
    }

    public int FindUnitCount(Unit_Status data)
    {
        int result = 0;

        for (int i = 0; i < unitCell.Length; i++)
        {
            if (unitCell[i].unit_Status.code == data.code)
            {
                result += unitCell[i].unitCount;
            }
        }
        return result;
    }

    public void Result()
    {
        for (int i = 0; i < unitCell.Length; i++)
        {
            Debug.Log(unitCell[i].unit_Status.unitName);
        }
    }
}