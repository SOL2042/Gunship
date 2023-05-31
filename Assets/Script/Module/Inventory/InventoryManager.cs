using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddItemInfo
{
    public Unit_Status data;
    public int count;

    public AddItemInfo(Unit_Status data, int count)
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
    Inventory inventory;

    public Inventory GetInventory() => inventory;

    public void AddItem(Unit_Status data, int count)
    {
        inventory.AddItem(data, count);
        Refresh();
    }

    [SerializeField]
    private Transform unitCellParent;
    private GameObject unitCellPrefab;

    private UnitCellManager[] unitCells;

    private void Awake()
    {
        inventory = new Inventory();
        unitCellPrefab = Resources.Load<GameObject>("Prefabs/ItemCell");
        unitCells = new UnitCellManager[inventory.unitCell.Length];
        for (int i = 0; i < inventory.unitCell.Length; i++)
        {
            unitCells[i] = Instantiate(unitCellPrefab, unitCellParent).GetComponent<UnitCellManager>();
        }
        Refresh();

        EventManager.instance.AddListener("Inventory.AddItem", (p) =>
        {
            AddItemInfo info = (AddItemInfo)p;
            inventory.AddItem(info.data, info.count);
            EventManager.instance.PostEvent("GetItem", null);
            Refresh();
        });

        EventManager.instance.AddListener("Inventory.UseItem", (p) =>
        {
            UseItem((int)p);
            Refresh();
        });
    }

    private void Refresh()
    {
        for (int i = 0; i < unitCells.Length; i++)
        {
            unitCells[i].Refresh(inventory.unitCell[i]);
        }
    }

    public void UseItem(int index)
    {
        inventory.unitCell[index].unit_Status.UseItem();
        inventory.unitCell[index].unitCount--;
        Refresh();
    }
}
