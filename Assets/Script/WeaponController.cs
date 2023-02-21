using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WeaponController : MonoBehaviour
{
    Bullet bullet;

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
       
        cameraTransform = Camera.main.transform;
        player = GetComponent<Player>();
        missileCnt = 8;
    }
    private void Update()
    {
        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * 1000.0f, Color.green);

        RaycastHit temp;

        if (Physics.Raycast(cameraTransform.transform.position, cameraTransform.transform.forward, out temp, 1000.0f, (-1) - (1 << 6))) // ī�޶��� ��ġ���� ī�޶� �ٶ󺸴� �������� ���̸� ���� �浹Ȯ��
        {
            // �浹�� ����Ǹ� �Ѿ��� ����������Ʈ(firePos)�� �浹�� �߻�����ġ�� �ٶ󺸰� �����. 
            // �� ���¿��� �߻��Է��� ������ �Ѿ��� �浹������ ���ư��� �ȴ�.
            
            //Debug.Log(temp.point);
            bulletPosition.LookAt(temp.point);
            bullet = new Bullet(temp.point, false);
            Debug.DrawRay(bulletPosition.position, bulletPosition.forward * 1000.0f, Color.red); // �� ���̴� �ռ� ������ ����׿� ���̿� �浹������ �����Ѵ�
        }
        else
        {
            Vector3 screenCenter = new Vector3(Camera.main.pixelWidth / 2, Camera.main.pixelHeight / 2, 0);
            Ray ray = Camera.main.ScreenPointToRay(screenCenter);
            
            bulletPosition.LookAt(ray.direction);
            Debug.Log(ray.direction);
            Debug.DrawRay(ray.origin,ray.direction * 1000.0f, Color.blue); 
        }

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
            if (missileCnt % 2 == 1)
            {
                if(missileCnt == 7)
                gameObject.transform.GetChild(0).GetChild(7).gameObject.SetActive(false);
                if(missileCnt == 5)
                gameObject.transform.GetChild(0).GetChild(8).gameObject.SetActive(false);
                if(missileCnt == 3)
                gameObject.transform.GetChild(0).GetChild(9).gameObject.SetActive(false);
                if(missileCnt == 1)
                gameObject.transform.GetChild(0).GetChild(10).gameObject.SetActive(false);

            }
            else
            {
                if (missileCnt == 6)
                    gameObject.transform.GetChild(0).GetChild(11).gameObject.SetActive(false);
                if (missileCnt == 4)
                    gameObject.transform.GetChild(0).GetChild(12).gameObject.SetActive(false);
                if (missileCnt == 2)
                    gameObject.transform.GetChild(0).GetChild(13).gameObject.SetActive(false);
                if (missileCnt == 0)
                    gameObject.transform.GetChild(0).GetChild(14).gameObject.SetActive(false);
            }
           
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
        player.transform.GetChild(0).GetChild(2).gameObject.SetActive(true);
        GameObject go = Resources.Load<GameObject>("Prefabs/Bullet");
        GameObject bullet = Instantiate(go, bulletPosition.position + new Vector3(Random.Range(-0.3f, 0.3f), Random.Range(-0.3f, 0.3f), 0), Quaternion.identity);
        bullet.layer = 6;

        Destroy(bullet, 3);
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

           for (int i = 7; i < 15; i++)
           {
               gameObject.transform.GetChild(0).GetChild(i).gameObject.SetActive(true);
           }

        }
    }
}