using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddUnitInfo                                    // ���� ���� �߰� Ŭ����
{
    public Unit_Status data;                                // ���� ������
    public int count;                                       // ���� ����

    public AddUnitInfo(Unit_Status data, int count)         
    {
        this.data = data;
        this.count = count;
    }
}

public class InventoryManager : MonoBehaviour
{
    #region
    private static InventoryManager _instance;
    public static InventoryManager instance
    {
        get
        {
            if (_instance == null) _instance = FindObjectOfType<InventoryManager>();
            return _instance;
        }
    }
    #endregion
    Inventory inventory;                                    // �κ��丮 ����

    public Inventory GetInventory() => inventory;           

    public void AddUnit(Unit_Status data, int count)        // �κ��丮�� ���� �߰�
    {
        inventory.AddUnit(data, count);
        Refresh();
    }

    [SerializeField]
    private Transform unitCellParent;                       // unitCell�� �θ� ��ġ
    private GameObject unitCellPrefab;                      // unitCell ������

    private UnitCellManager[] unitCells;                    // UnitCellManager �迭 unitCells

    private void Awake()
    {
        inventory = new Inventory();                                                                        // �κ��丮 �ʱ�ȭ
        unitCellPrefab = Resources.Load<GameObject>("Prefabs/UnitCell");                                    // ���� ������ ��������
        unitCells = new UnitCellManager[inventory.unitCell.Length];                                         // unitCells�� �κ��丮�� ����ĭ�� ���̸�ŭ �� UnitCellManager �߰�
        for (int i = 0; i < inventory.unitCell.Length; i++)                                                 //  ����ĭ�� ���̸�ŭ �ݺ�
        {
            unitCells[i] = Instantiate(unitCellPrefab, unitCellParent).GetComponent<UnitCellManager>();     // unitCells�� ���� ������Ʈ ����
        }
        Refresh();                                                                                          // �ʱ�ȭ

        EventManager.instance.AddListener("Inventory.AddUnit", (p) =>
        {
            AddUnitInfo info = (AddUnitInfo)p;
            inventory.AddUnit(info.data, info.count);
            EventManager.instance.PostEvent("GetUnit", null);
            Refresh();
        });

        EventManager.instance.AddListener("Inventory.UseUnit", (p) =>
        {
            UseUnit((int)p);
            Refresh();
        });
    }
    private void Update()
    {
        

    }
    private void Refresh()
    {
        for (int i = 0; i < unitCells.Length; i++)
        {
            unitCells[i].Refresh(inventory.unitCell[i]);
        }
    }

    public void UseUnit(int index)
    {
        inventory.unitCell[index].unit_Status.UseUnit();
        inventory.unitCell[index].unitCount--;
        Refresh();
    }
}
