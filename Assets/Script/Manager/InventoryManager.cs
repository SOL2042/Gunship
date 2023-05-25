using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
