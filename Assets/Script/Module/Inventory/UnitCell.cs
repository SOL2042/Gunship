using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitCell
{
    public int unitCost = 0;               // ���ּ��� �ִ� ���� ����
    public Unit_Status unit_Status;         // ������ ������ �޾ƿ��� ����

    public UnitCell()
    {
        unit_Status = new Unit_Status_NoUnit();   //���� �ʱ�ȭ
    }
}
