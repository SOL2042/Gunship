using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UnitCellManager : MonoBehaviour
{
    private UnitCell cell;                                  // UnitCell을 받아오는 변수
    [SerializeField] private GameObject unitData;           // 하이어라키 내의 게임오브젝트
    [SerializeField] private Image unitImage;               // 유닛의 이미지
    [SerializeField] private TextMeshProUGUI unitName;      // 유닛의 이름
    [SerializeField] private TextMeshProUGUI unitCost;     // 유닛의 숫자

    private void Awake()
    {
        cell = new UnitCell();                              // UnitCell 첫 초기화
    }

    public void Refresh()                                   // UnitCell 초기화
    {
        if (cell.unit_Status.code == "")                    // cell의 유닛 코드가 없으면
        {
            unitData.SetActive(false);                      // 게임오브젝트 비활성화
        }
        else                                                // cell의 유닛 코드가 있으면
        {
            unitData.SetActive(true);                       // 게임오브젝트 활성화
            unitImage.sprite = cell.unit_Status.unitSprite; // 이미지는 cell 유닛 이미지 
            unitName.text = cell.unit_Status.unitName;      
            unitCost.text = cell.unitCost.ToString();
        }
    }

    public void Refresh(UnitCell unitCell)                  // 유닛 받아와서 초기화
    {
        cell = unitCell;
        Refresh();
    }
}
