using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T90 : MonoBehaviour 
{
    GameObject explosionPrefab;
    GameObject bullet;

    [SerializeField] private float moveSpeed = 20f; // �̵� �ӵ�
    [SerializeField] private float turnSpeed = 900f; // ȸ�� �ӵ�
    [SerializeField] private float turretTurnSpeed = 70f; // ȸ�� �ӵ�
    [SerializeField] private float shootRange = 100f; // ��� ����
    [SerializeField] private float shootInterval = 4f; // ��� ����
    [SerializeField] private Transform turretTransform; // ��ž Transform ������Ʈ
    //[SerializeField] private Transform cannonTransform; // ���� Transform ������Ʈ
    //[SerializeField] private float cannonPitchRange = 45f; // ���� �� ���� ����

    private Transform playerTransform; // �÷��̾��� Transform ������Ʈ
    private Transform USbaseTransform; // �÷��̾� ������ Transform ������Ʈ
    private Rigidbody tankRigidbody; // ��ũ�� Rigidbody ������Ʈ
    private float lastShootTime; // ������ ��� �ð�

    [SerializeField] private Transform bulletPosition;


    private void Start()
    {
        bulletPosition = transform.GetChild(16).GetChild(0).GetChild(0).transform;
        bullet = Resources.Load<GameObject>("Prefabs/TankBullet");
        explosionPrefab = Resources.Load<GameObject>("Prefabs/BigExplosion");
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform; // �÷��̾��� Transform ������Ʈ ��������
        USbaseTransform = GameObject.FindGameObjectWithTag("USBase").transform;
        tankRigidbody = GetComponent<Rigidbody>(); // ��ũ�� Rigidbody ������Ʈ ��������
        turretTransform = GameObject.FindWithTag("Enemy").transform.GetChild(16).transform;
        //cannonTransform = GameObject.Find("T90LP ForrestWavyCamo/Cube.017/Cube.018").transform;
    }

    private void Update()
    {
        // �÷��̾���� �Ÿ� ���
        float distanceToPlayer = Vector3.Distance(transform.position, USbaseTransform.position);

        if (distanceToPlayer <= shootRange) // ��� ���� ���� ���� ��
        {
            moveSpeed = 0;
            Vector3 directionToPlayer = USbaseTransform.position - transform.position;
            // ��ž�� ������ �÷��̾� �������� ȸ��
            Quaternion targetTurretRotation = Quaternion.LookRotation(directionToPlayer);
            turretTransform.rotation = Quaternion.RotateTowards(turretTransform.rotation, targetTurretRotation, turnSpeed * Time.deltaTime);

            //Quaternion targetCannonRotation = Quaternion.LookRotation(directionToPlayer);
            //targetCannonRotation = Quaternion.Euler(new Vector3(Mathf.Clamp(-targetCannonRotation.eulerAngles.x, -cannonPitchRange, cannonPitchRange), targetCannonRotation.eulerAngles.y, targetCannonRotation.eulerAngles.z));
            //cannonTransform.rotation = Quaternion.RotateTowards(cannonTransform.rotation, targetCannonRotation, turnSpeed * Time.deltaTime);
            if (Time.time - lastShootTime >= shootInterval) // ��� ������ ���� ���
            {
                Shoot(); // ���
                lastShootTime = Time.time; // ������ ��� �ð� ����
            }
        }
        else // ��� ���� �ۿ� ���� ��
        {
            Vector3 directionToPlayer = USbaseTransform.position - transform.position;
            directionToPlayer.y = 0f;

            Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
            tankRigidbody.MoveRotation(Quaternion.RotateTowards(transform.rotation, targetRotation, turnSpeed * Time.deltaTime));

            // ��ũ�� �÷��̾� ������ �̵�
            Vector3 movement = transform.forward * moveSpeed * Time.deltaTime;
            tankRigidbody.MovePosition(tankRigidbody.position + movement);
        }
    }

    private void Shoot()
    {
        // ��� ���� ����
        //transform.GetChild(0).GetChild(2).gameObject.SetActive(true);
        GameObject go = Resources.Load<GameObject>("Prefabs/TankBullet");
        GameObject bullet = Instantiate(go, bulletPosition.position, Quaternion.identity);
        
        Destroy(bullet, 3);
        Debug.Log("Shoot!");
    }

    private void Dead()
    {
        Debug.Log("�׾���!");
    }
    
}
