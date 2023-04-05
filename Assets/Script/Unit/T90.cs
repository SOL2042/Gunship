using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T90 : MonoBehaviour 
{
    GameObject explosionPrefab;

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

    private void Start()
    {
        explosionPrefab = Resources.Load<GameObject>("Prefabs/BigExplosion");
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform; // �÷��̾��� Transform ������Ʈ ��������
        USbaseTransform = GameObject.FindGameObjectWithTag("Player").transform;
        tankRigidbody = GetComponent<Rigidbody>(); // ��ũ�� Rigidbody ������Ʈ ��������
        turretTransform = GameObject.Find("T90LP ForrestWavyCamo/Cube.017").transform;
        //cannonTransform = GameObject.Find("T90LP ForrestWavyCamo/Cube.017/Cube.018").transform;
    }

    private void Update()
    {
        // �÷��̾���� �Ÿ� ���
        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

        if (distanceToPlayer <= shootRange) // ��� ���� ���� ���� ��
        {
            moveSpeed = 0;
            Vector3 directionToPlayer = playerTransform.position - transform.position;
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
            
            Vector3 directionToPlayer = playerTransform.position - transform.position;
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

        Debug.Log("Shoot!");
    }
}
