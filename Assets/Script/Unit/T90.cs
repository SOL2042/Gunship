using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T90 : MonoBehaviour 
{
    //Player target;
    //GameObject turret;
    //GameObject mainGun;
    //Rigidbody rgb;
    //float rotSpeed = 30;
    //float timer = 0;
    //void Start()
    //{
    //    rgb = GetComponent<Rigidbody>();
    //    target = FindObjectOfType<Player>();
    //    turret = GameObject.Find("T90LP ForrestWavyCamo/Cube.017");
    //    mainGun = GameObject.Find("T90LP ForrestWavyCamo/Cube.017/Cube.018");
    //}

    //void Update()
    //{
    //    rgb.velocity = Vector3.Lerp(rgb.velocity, Vector3.zero, Time.deltaTime);
    //    timer += Time.deltaTime;
    //    if (timer < 10)
    //    {
    //        rgb.AddRelativeForce(Vector3.forward * Time.deltaTime * 1000);
    //    }
    //    else if (timer > 10 && timer <= 13.1)
    //    {
    //        gameObject.transform.Rotate(Vector3.Lerp(new Vector3(0, -rotSpeed * Time.deltaTime, 0), Vector3.zero, Time.deltaTime));
    //        if(timer >= 13)
    //        {
    //            timer = 0;
    //        }
    //    }




    //    Aim();
    //}

    //private void Aim()
    //{
    //    //��ž ȸ��
    //    //turret.transform.rotation = Quaternion.LookRotation(new Vector3(target.transform.position.x, 0 , target.transform.position.z));

    //    turret.transform.rotation = Quaternion.Euler(0, turret.transform.rotation.y, 0);
    //    turret.transform.LookAt(target.gameObject.transform);




    //    //������
    //    //mainGun.transform.rotation = Quaternion.LookRotation(new Vector3(0, target.transform.position.y, 0));

    //   // mainGun.transform.rotation = Quaternion.Euler(mainGun.transform.rotation.x, 0, 0);
    //   // mainGun.transform.LookAt(target.gameObject.transform);


    //}

    GameObject turret;
    GameObject mainGun;

    [SerializeField] private float moveSpeed = 20f; // �̵� �ӵ�
    [SerializeField] private float turnSpeed = 60f; // ȸ�� �ӵ�
    [SerializeField] private float shootRange = 30f; // ��� ����
    [SerializeField] private float shootInterval = 2f; // ��� ����
    [SerializeField] private Transform turretTransform; // ��ž Transform ������Ʈ
    [SerializeField] private Transform cannonTransform; // ���� Transform ������Ʈ
    [SerializeField] private float cannonPitchRange = 45f; // ���� �� ���� ����

    private Transform playerTransform; // �÷��̾��� Transform ������Ʈ
    private Rigidbody tankRigidbody; // ��ũ�� Rigidbody ������Ʈ
    private float lastShootTime; // ������ ��� �ð�

    private void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform; // �÷��̾��� Transform ������Ʈ ��������
        tankRigidbody = GetComponent<Rigidbody>(); // ��ũ�� Rigidbody ������Ʈ ��������
        turretTransform = GameObject.Find("T90LP ForrestWavyCamo/Cube.017").transform;
        cannonTransform = GameObject.Find("T90LP ForrestWavyCamo/Cube.017/Cube.018").transform;
    }

    private void Update()
    {
        // �÷��̾���� �Ÿ� ���
        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

        if (distanceToPlayer <= shootRange) // ��� ���� ���� ���� ��
        {
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

            // ��ž�� ������ �÷��̾� �������� ȸ��
            Quaternion targetTurretRotation = Quaternion.LookRotation(directionToPlayer);

            turretTransform.rotation = Quaternion.RotateTowards(turretTransform.rotation, targetTurretRotation, turnSpeed * Time.deltaTime);

            Quaternion targetCannonRotation = Quaternion.LookRotation(directionToPlayer);
            targetCannonRotation = Quaternion.Euler(new Vector3(Mathf.Clamp(-targetCannonRotation.eulerAngles.x, -cannonPitchRange, cannonPitchRange), targetCannonRotation.eulerAngles.y, targetCannonRotation.eulerAngles.z));
            cannonTransform.rotation = Quaternion.RotateTowards(cannonTransform.rotation, targetCannonRotation, turnSpeed * Time.deltaTime);

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
