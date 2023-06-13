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
    [SerializeField] private TextMeshProUGUI unitCost;     // ������ ����

    private void Awake()
    {
        cell = new UnitCell();                              // UnitCell ù �ʱ�ȭ
    }

    public void Refresh()                                   // UnitCell �ʱ�ȭ
    {
        if (cell.unit_Status.code == "")                    // cell�� ���� �ڵ尡 ������
        {
            unitData.SetActive(false);                      // ���ӿ�����Ʈ ��Ȱ��ȭ
        }
        else                                                // cell�� ���� �ڵ尡 ������
        {
            unitData.SetActive(true);                       // ���ӿ�����Ʈ Ȱ��ȭ
            unitImage.sprite = cell.unit_Status.unitSprite; // �̹����� cell ���� �̹��� 
            unitName.text = cell.unit_Status.unitName;      
            unitCost.text = cell.unitCost.ToString();
        }
    }

    public void Refresh(UnitCell unitCell)                  // ���� �޾ƿͼ� �ʱ�ȭ
    {
        cell = unitCell;
        Refresh();
    }
}
