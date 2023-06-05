using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UnitCellManager : MonoBehaviour
{
    private UnitCell cell;                                  // UnitCell�� �޾ƿ��� ����
    [SerializeField] private GameObject unitData;           // ���̾��Ű ���� ���ӿ�����Ʈ
    [SerializeField] private Image unitImage;               // ������ �̹���
    [SerializeField] private TextMeshProUGUI unitName;      // ������ �̸�
    [SerializeField] private TextMeshProUGUI unitCount;     // ������ ����

    private void Awake()
    {
        cell = new UnitCell();                              // UnitCell ù �ʱ�ȭ
    }

    public void Refresh()                                   // UnitCell �ʱ�ȭ
    {
        if (cell.unitCount <= 0)                            // cell�� ���ּ��� 0 ���ϸ�
        {
            cell.unit_Status = new Unit_Status_NoUnit();    // cell���� ���� ������ �ʱ�ȭ
        }

        if (cell.unit_Status.code == "")                    // cell�� ���� �ڵ尡 ������
        {
            unitData.SetActive(false);                      // ���ӿ�����Ʈ ��Ȱ��ȭ
        }
        else                                                // cell�� ���� �ڵ尡 ������
        {
            unitData.SetActive(true);                       // ���ӿ�����Ʈ Ȱ��ȭ
            unitImage.sprite = cell.unit_Status.unitSprite; // �̹����� cell ���� �̹��� 
            unitName.text = cell.unit_Status.unitName;      
            unitCount.text = cell.unitCount.ToString();
        }
    }

    public void Refresh(UnitCell unitCell)                  // ���� �޾ƿͼ� �ʱ�ȭ
    {
        cell = unitCell;
        Refresh();
    }
}
