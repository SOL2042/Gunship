using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitCellManager : MonoBehaviour
{
    private UnitCell cell;
    [SerializeField] private GameObject unitCell;
    [SerializeField] private Image unitImage;
    [SerializeField] private Text unitName;
    [SerializeField] private Text unitCount;

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
            unitCell.SetActive(false);
        }
        else
        {
            unitCell.SetActive(true);
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
