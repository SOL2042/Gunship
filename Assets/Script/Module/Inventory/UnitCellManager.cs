using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UnitCellManager : MonoBehaviour
{
    private UnitCell cell;
    [SerializeField] private GameObject unitData;
    [SerializeField] private Image unitImage;
    [SerializeField] private TextMeshProUGUI unitName;
    [SerializeField] private TextMeshProUGUI unitCount;

    private void Awake()
    {
        cell = new UnitCell();
    }

    public void Refresh()
    {
        if (cell.unitCount <= 0)
        {
            cell.unit_Status = new Unit_Status_NoUnit();
        }

        if (cell.unit_Status.code == "")
        {
            unitData.SetActive(false);
        }
        else
        {
            unitData.SetActive(true);
            unitImage.sprite = cell.unit_Status.unitSprite;
            unitName.text = cell.unit_Status.unitName;
            unitCount.text = cell.unitCount.ToString();
        }
    }

    public void Refresh(UnitCell itemCell)
    {
        cell = itemCell;
        Refresh();
    }
}
