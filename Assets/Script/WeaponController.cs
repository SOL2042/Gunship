using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WeaponController : MonoBehaviour
{
    [SerializeField]
    Transform cameraTransform;


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

    Bullet bullet;
    float bulletTravelDistance = 25f;

    public int bulletCnt;
   
    public float gunRPM;
    

    Player player;
    // Weapon Inputs
    
    public GameObject missilePrefab;
    // Weapon Callbacks


    [SerializeField] Transform bulletPosition;

    private float fireTimer = 0;


    void Start()
    {
        bullet = GetComponent<Bullet>();
        cameraTransform = Camera.main.transform;
        player = GetComponent<Player>();
        missileCnt = 8;
    }
    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            fireTimer += Time.deltaTime;
            if (fireTimer >= 0.1f)
            {
                MainGunFire();
                fireTimer = 0f;

            }
        }
        else
        {
            CeaseFire();
            fireTimer = 0.09f;
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            LaunchMissile();
        }
        if(Input.GetKeyDown(KeyCode.R))
        {
            Reload();
        }
        MissileCooldown(ref rightMslCooldown);
        MissileCooldown(ref leftMslCooldown);
    }


    private void MainGunFire()
    {
        RaycastHit hit;
        
        
       
        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit, 9999))  //Raycast�� object�� ������
        {
            
            this.bullet.target = hit.point;    //Raycast�� ������ Target���� ����
            this.bullet.hit = true;
        }
        else
        {                                           //ī�޶� �ٶ󺸴� ���⿡�� �Ѿ˺���Ÿ���ŭ ������ ������ Target���� ����
            this.bullet.target = cameraTransform.position + cameraTransform.forward * bulletTravelDistance;
            this.bullet.hit = false;
        }


        player.transform.GetChild(0).GetChild(2).gameObject.SetActive(true);
        GameObject go = Resources.Load<GameObject>("Prefabs/Bullet");

        GameObject bullet = Instantiate(go, bulletPosition.position + new Vector3(Random.Range(-0.3f, 0.3f), Random.Range(-0.3f, 0.3f), 0), player.transform.rotation);
        bullet.layer = 6;
        Destroy(bullet, 2);
    }



    private void CeaseFire()
    {
        gameObject.transform.GetChild(0).GetChild(2).gameObject.SetActive(false);
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