using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
    WeaponController weaponController;

    [SerializeField]
    Text textMesh;
    
    // Start is called before the first frame update
    void Start()
    {
        
        textMesh.text = "Hellfire Missile : 8";

    }

    // Update is called once per frame
    void Update()
    {
        //if(weaponController.GetComponent<WeaponController>())
    }
}
