using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T90 : MonoBehaviour 
{
    GameObject explosionPrefab;
    GameObject bullet;

    [SerializeField] private float moveSpeed = 20f; // 이동 속도
    [SerializeField] private float turnSpeed = 900f; // 회전 속도
    [SerializeField] private float turretTurnSpeed = 70f; // 회전 속도
    [SerializeField] private float shootRange = 100f; // 사격 범위
    [SerializeField] private float shootInterval = 4f; // 사격 간격
    [SerializeField] private Transform turretTransform; // 포탑 Transform 컴포넌트
    //[SerializeField] private Transform cannonTransform; // 포신 Transform 컴포넌트
    //[SerializeField] private float cannonPitchRange = 45f; // 포신 고도 제한 각도

    private Transform playerTransform; // 플레이어의 Transform 컴포넌트
    private Transform USbaseTransform; // 플레이어 기지의 Transform 컴포넌트
    private Rigidbody tankRigidbody; // 탱크의 Rigidbody 컴포넌트
    private float lastShootTime; // 마지막 사격 시간

    [SerializeField] private Transform bulletPosition;


    private void Start()
    {
        bulletPosition = transform.GetChild(16).GetChild(0).GetChild(0).transform;
        bullet = Resources.Load<GameObject>("Prefabs/TankBullet");
        explosionPrefab = Resources.Load<GameObject>("Prefabs/BigExplosion");
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform; // 플레이어의 Transform 컴포넌트 가져오기
        USbaseTransform = GameObject.FindGameObjectWithTag("USBase").transform;
        tankRigidbody = GetComponent<Rigidbody>(); // 탱크의 Rigidbody 컴포넌트 가져오기
        turretTransform = GameObject.FindWithTag("Enemy").transform.GetChild(16).transform;
        //cannonTransform = GameObject.Find("T90LP ForrestWavyCamo/Cube.017/Cube.018").transform;
    }

    private void Update()
    {
        // 플레이어와의 거리 계산
        float distanceToPlayer = Vector3.Distance(transform.position, USbaseTransform.position);

        if (distanceToPlayer <= shootRange) // 사격 범위 내에 있을 때
        {
            moveSpeed = 0;
            Vector3 directionToPlayer = USbaseTransform.position - transform.position;
            // 포탑과 포신을 플레이어 방향으로 회전
            Quaternion targetTurretRotation = Quaternion.LookRotation(directionToPlayer);
            turretTransform.rotation = Quaternion.RotateTowards(turretTransform.rotation, targetTurretRotation, turnSpeed * Time.deltaTime);

            //Quaternion targetCannonRotation = Quaternion.LookRotation(directionToPlayer);
            //targetCannonRotation = Quaternion.Euler(new Vector3(Mathf.Clamp(-targetCannonRotation.eulerAngles.x, -cannonPitchRange, cannonPitchRange), targetCannonRotation.eulerAngles.y, targetCannonRotation.eulerAngles.z));
            //cannonTransform.rotation = Quaternion.RotateTowards(cannonTransform.rotation, targetCannonRotation, turnSpeed * Time.deltaTime);
            if (Time.time - lastShootTime >= shootInterval) // 사격 간격이 지난 경우
            {
                Shoot(); // 사격
                lastShootTime = Time.time; // 마지막 사격 시간 갱신
            }
        }
        else // 사격 범위 밖에 있을 때
        {
            Vector3 directionToPlayer = USbaseTransform.position - transform.position;
            directionToPlayer.y = 0f;

            Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
            tankRigidbody.MoveRotation(Quaternion.RotateTowards(transform.rotation, targetRotation, turnSpeed * Time.deltaTime));

            // 탱크를 플레이어 쪽으로 이동
            Vector3 movement = transform.forward * moveSpeed * Time.deltaTime;
            tankRigidbody.MovePosition(tankRigidbody.position + movement);
        }
    }

    private void Shoot()
    {
        // 사격 로직 구현
        //transform.GetChild(0).GetChild(2).gameObject.SetActive(true);
        GameObject go = Resources.Load<GameObject>("Prefabs/TankBullet");
        GameObject bullet = Instantiate(go, bulletPosition.position, Quaternion.identity);
        
        Destroy(bullet, 3);
        Debug.Log("Shoot!");
    }

    private void Dead()
    {
        Debug.Log("죽었다!");
    }
    
}
