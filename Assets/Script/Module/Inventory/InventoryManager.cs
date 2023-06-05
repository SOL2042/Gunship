using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddUnitInfo                                    // 유닛 정보 추가 클래스
{
    public Unit_Status data;                                // 유닛 데이터
    public int count;                                       // 유닛 숫자

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
    Inventory inventory;                                    // 인벤토리 변수

    public Inventory GetInventory() => inventory;           

    public void AddUnit(Unit_Status data, int count)        // 인벤토리에 유닛 추가
    {
        inventory.AddUnit(data, count);
        Refresh();
    }

    [SerializeField]
    private Transform unitCellParent;                       // unitCell의 부모 위치
    private GameObject unitCellPrefab;                      // unitCell 프리팹

    private UnitCellManager[] unitCells;                    // UnitCellManager 배열 unitCells

    private void Awake()
    {
        inventory = new Inventory();                                                                        // 인벤토리 초기화
        unitCellPrefab = Resources.Load<GameObject>("Prefabs/UnitCell");                                    // 유닛 프리팹 가져오기
        unitCells = new UnitCellManager[inventory.unitCell.Length];                                         // unitCells에 인벤토리의 유닛칸의 길이만큼 새 UnitCellManager 추가
        for (int i = 0; i < inventory.unitCell.Length; i++)                                                 //  유닛칸의 길이만큼 반복
        {
            unitCells[i] = Instantiate(unitCellPrefab, unitCellParent).GetComponent<UnitCellManager>();     // unitCells에 게임 오브젝트 생성
        }
        Refresh();                                                                                          // 초기화

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
