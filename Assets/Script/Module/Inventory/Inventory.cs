using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    public UnitCell[] unitCell;                 // ����ĭ �迭

    public Inventory()                          // �κ��丮 �ʱ�ȭ
    {
        unitCell = new UnitCell[5];             // ����ĭ 5�� ����
        Start();                                
    }

    public Inventory(int length)                // �ܺο��� �޾ƿͼ� ����ĭ ����
    {
        unitCell = new UnitCell[length];
        Start();
    }

    private void Start()                            // ���ּ� �迭���� �� ���ּ� ����
    {
        for (int i = 0; i < unitCell.Length; i++)
        {
            unitCell[i] = new UnitCell();
        }
    }

    public void AddUnit(Unit_Status data, int count)                // ���� �߰�
    {
        int blankCell = -1;                                         // ����ִ� ĭ
        bool alreadyAdded = false;                                  // �̹� �߰��ߴ��� Ȯ���ϴ� ����

        for (int i = 0; i < unitCell.Length; i++)                   // ������� ����ĭ�� ����ִ��� ���ִ��� ���� Ȯ���ϴ� �ݺ��� 
        {
            if (unitCell[i].unit_Status.code == "")                 // unitCell[i]�� �ڵ尡 ������
            {
                if (blankCell == -1) blankCell = i;                 // blankCell�� -1 �̸� i�� ����
            }
            else                                                    // unitCell[i]�� �ڵ尡 ������
            {
                if (unitCell[i].unit_Status.code == data.code)      // unitCell[i]�� �ڵ尡 data�� �ڵ�� ���ٸ�
                {
                    unitCell[i].unitCount += count;                 // unitCell[i]�� ���ڿ� �Ű����� count�� ������
                    alreadyAdded = true;                            // �̹� �߰��Ǿ��ٴ°ɷ� ����
                    break;                                          // �ݺ������� ���
                }
            }
        }

        if (blankCell != -1)                                        // blankCell�� �ռ� Ȯ���� i��
        {
            if (!alreadyAdded)                                      // �̹� �߰����� �ʾҴٸ�
            {
                unitCell[blankCell].unit_Status = data;             // unitCell[i]�� ���ֵ����Ϳ� �Ű����� data
                unitCell[blankCell].unitCount = count;              // unitCell[i]�� ���� ���ڿ� �Ű����� count
            }
        }
        else                                                        // blankCell�� -1�̸�
        {
            if (!alreadyAdded)                                      // �̹� �߰����� �ʾҴٸ�                              
            {
                Debug.Log("����â�� ����");
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