using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WeaponController : MonoBehaviour
{
    Player player;
    // Weapon Inputs
    
    public GameObject missilePrefab;
    // Weapon Callbacks

    void Start()
    {
        player = GetComponent<Player>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            Fire();
        }

    }

    public void Fire()
    {
      
        
        LaunchMissile();
      
        
    }

    public void GunFire()
    {
       
    }


    public void SwitchWeapon()
    {
       
    }

    void LaunchMissile()
    {
        GameObject missile = Instantiate(missilePrefab, transform.position, transform.rotation);
        HellFire_Missile missileScript = missile.GetComponent<HellFire_Missile>();
        missileScript.Launch(null, player.speed + 15, gameObject.layer);
}
}