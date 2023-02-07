using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WeaponController : MonoBehaviour
{
    [SerializeField]
    Transform enemyPosition;

    [SerializeField]
    Transform rightMissilePosition;
    [SerializeField]
    Transform leftMissilePosition;

    private float missileCnt;
    public float missileCooldownTime;

    float rightMslCooldown;
    float leftMslCooldown;

    
    
    public int bulletCnt;
    public Transform gunTransform;
    public float gunRPM;
    float fireInterval;

    Player player;
    // Weapon Inputs
    
    public GameObject missilePrefab;
    // Weapon Callbacks

    void Start()
    {
        player = GetComponent<Player>();
        missileCnt = 8;
    }
    private void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.G))
        {
            Fire();
        }
        if(Input.GetKeyDown(KeyCode.R))
        {
            Reload();
        }
        MissileCooldown(ref rightMslCooldown);
        MissileCooldown(ref leftMslCooldown);
    }

    public void Fire()
    {
      
        
        LaunchMissile();
      
        
    }

    public void GunFire()
    {
        
    }


    void FireMachineGun()
    {
        if (bulletCnt <= 0)
        {
            // Beep sound
            CancelInvoke("FireMachineGun");
            return;
        }

        
    }


    private void LaunchMissile()
    {
        Vector3 missilePosition;
        if (missileCnt <= 0)
            return;

        if (leftMslCooldown > 0 && rightMslCooldown > 0)
        {
            // Beep sound
            return;
        }

        if (missileCnt % 2 == 1)
        {
            missilePosition = rightMissilePosition.position;
            rightMslCooldown = missileCooldownTime;
        }
        else
        {
            missilePosition = leftMissilePosition.position;
            leftMslCooldown = missileCooldownTime;
        }

        
        GameObject missile = Instantiate(missilePrefab, missilePosition, transform.rotation);
        HellFire_Missile missileScript = missile.GetComponent<HellFire_Missile>();
        missileScript.Launch(enemyPosition, player.speed + 15, gameObject.layer);

        missileCnt--;
        
       
    }


    void MissileCooldown(ref float cooldown)
    {
        if (cooldown > 0)
        {
            cooldown -= Time.deltaTime;
            if (cooldown < 0) cooldown = 0;
        }
        else return;
    }




    private void Reload()
    {
        
        
        if(missileCnt <= 0)
        {
            
           missileCnt = 8;
              
            
        }
    }


}