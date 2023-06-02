using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    public UnitCell[] unitCell;

    public Inventory()
    {
        unitCell = new UnitCell[5];
        Start();
    }

    public Inventory(int length)
    {
        unitCell = new UnitCell[length];
        Start();
    }

    private void Start()
    {
        for (int i = 0; i < unitCell.Length; i++)
        {
            unitCell[i] = new UnitCell();
        }
    }

    public void AddItem(Unit_Status data, int count)
    {
        int blankCell = -1;
        bool alreadyAdded = false;

        for (int i = 0; i < unitCell.Length; i++)
        {
            if (unitCell[i].unit_Status.code == "")
            {
                if (blankCell == -1) blankCell = i;
            }
            else
            {
                if (unitCell[i].unit_Status.code == data.code)
                {
                    unitCell[i].unitCount += count;
                    alreadyAdded = true;
                    break;
                }
            }
        }

        if (blankCell != -1)
        {
            if (!alreadyAdded)
            {
                unitCell[blankCell].unit_Status = data;
                unitCell[blankCell].unitCount = count;
            }
        }
        else
        {
            if (!alreadyAdded)
            {
                Debug.Log("ÅÛÃ¢ÀÌ ²ËÂü");
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

    public int FindItemCount(Unit_Status data)
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