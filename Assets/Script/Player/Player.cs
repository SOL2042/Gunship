using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private enum Mode
    {
        normal,
        Hover
    }

    private static Player _instance;

    public static Player instance
    {
        get
        {
            if (_instance == null) _instance = FindObjectOfType<Player>();
            return _instance;
        }
    }

    private Rigidbody rgb;

    [SerializeField] Transform bulletPosition;
    [SerializeField] Transform enemyPosition;

    [SerializeField] private float responsiveness = 100000f;
    [SerializeField] private float throttleAmt = 25f;
    public float throttle;
    
    private float roll;
    private float pitch;
    private float yaw;

    float count = 0;
    public float speed;

    //public float maxSpeed = 30.0f; // 최대 속력
    //public float accelAmount; // 가속 

    //float speedReciprocal; // maxSpeed의 역수
    //float accelValue; // 컨트롤러로 얻어오는 값

    void Start()
    {
        speed = rgb.velocity.z;
        //rb = GetComponent<Rigidbody>();
        //rb.drag = drag;
        //rb.angularDrag = angularDrag;
    }

    private void Awake()
    {
        rgb = GetComponent<Rigidbody>();
    }


    void Update()
    {
        HandleInputs();
        //Debug.Log(throttle);
        //Move();
    }
    float originalYRotation;
    private void FixedUpdate()
    {
        rgb.AddForce(transform.up * throttle * 0.07f, ForceMode.Impulse);
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S))
        {
            rgb.AddTorque(transform.right * pitch * responsiveness * 10f);
            rgb.AddForce(transform.forward * pitch * 100f);
        }
        else
        {

        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            rgb.AddTorque(transform.forward * -roll * responsiveness);
            rgb.AddForce(transform.right * -roll);
        }
        else
        {

        }
        rgb.AddTorque(transform.up * yaw * responsiveness * 4f);
        
        Vector3 currentPosition = transform.position;
        
        float PositionY = Mathf.Clamp(currentPosition.y, -10, 200);
   
        
        transform.position = new Vector3(transform.position.x, PositionY, transform.position.z);
        
    }

    private void HandleInputs()
    {
        roll = Input.GetAxis("Roll");
        pitch = Input.GetAxis("Pitch");
        yaw = Input.GetAxis("Yaw");
        if(Input.GetKey(KeyCode.LeftShift))
        {
            throttle += Time.deltaTime * throttleAmt;
        }
        else if (Input.GetKey(KeyCode.LeftControl))
        {
            throttle -= Time.deltaTime * throttleAmt;
        }
        //호버링 모드
        if (count == 0)
        {
            if (Input.GetKeyDown(KeyCode.H))
            {
                rgb.velocity = Vector3.zero;
                rgb.MoveRotation(Quaternion.Slerp(rgb.rotation, Quaternion.identity, Time.deltaTime * 10f));
                rgb.rotation = Quaternion.Slerp(rgb.rotation, Quaternion.identity, Time.deltaTime * 10f);
                count += 1;
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.H))
            {
                count = 0;
            }
        }

        throttle = Mathf.Clamp(throttle, 0f, 100f);
    }

    //public float maxTiltAngle = 30f;
    //public float smooth = 1f;
    //public float engineForce = 100f;
    //public float drag = 0.1f;
    //public float angularDrag = 0.1f;
    //public float gravity = 9.81f;
    //public float maxSpeed = 10f;

    //private Rigidbody rb;

    
    //public float Speed
    //{
    //    get
    //    {
    //        return speed;
    //    }
    //}

    
    //float rotSpeed = 100f;
    //float resetSpeed = 5f;

   
    
    //private void Move()
    //{
       
    //    // 키 입력값 받기
    //    float tiltAngle = Input.GetAxis("Horizontal") * maxTiltAngle;
    //    float thrust = Input.GetAxis("Vertical") * maxTiltAngle;

    //    // 목표 회전각 구하기
    //    Quaternion HorRotation = Quaternion.Euler(0f, 0f, -tiltAngle);
    //    Quaternion VerRotation = Quaternion.Euler(thrust, 0f, 0f);

    //    rgb.rotation = Quaternion.Slerp(rgb.rotation, Quaternion.identity , Time.deltaTime);
    //    //rgb.velocity = Vector3.Lerp(rgb.velocity, Vector3.zero, Time.deltaTime);


    //    if (Input.GetKey(KeyCode.A))
    //    {
    //        //rgb.AddRelativeForce(new Vector3(-speed, 0, 0));
    //        rgb.AddRelativeTorque(new Vector3(0 , 0, Mathf.Lerp(0, 30, Time.deltaTime * smooth)));
    //        //transform.rotation = Quaternion.Lerp(transform.rotation, HorRotation, Time.deltaTime * smooth);
    //    }
    //    if (Input.GetKey(KeyCode.D))
    //    {
    //        //rgb.AddRelativeForce(new Vector3(speed, 0, 0));
    //        rgb.AddRelativeTorque(new Vector3(0, 0, Mathf.Lerp(0, -30, Time.deltaTime * smooth)));
    //        //transform.rotation = Quaternion.Lerp(transform.rotation, HorRotation, Time.deltaTime * smooth);
    //    }
    //    if (Input.GetKey(KeyCode.W))
    //    {
    //        //rgb.AddRelativeForce(new Vector3(0, 0, speed));
    //        transform.rotation = Quaternion.Lerp(transform.rotation, VerRotation, Time.deltaTime * smooth);

    //    }
    //    if (Input.GetKey(KeyCode.S))
    //    {
    //        //rgb.AddRelativeForce(new Vector3(0, 0, -speed));
    //        transform.rotation = Quaternion.Lerp(transform.rotation, VerRotation, Time.deltaTime * smooth);
    //    }
    //    if (transform.position.y < 100)
    //        if (Input.GetKey(KeyCode.Space))
    //        {
    //            rgb.AddRelativeForce(new Vector3(0, speed, 0));
    //        }
    //    if (Input.GetKey(KeyCode.LeftControl))
    //    {
    //        rgb.AddForce(new Vector3(0, -speed, 0));
    //    }
    //    if (Input.GetKey(KeyCode.C))
    //    {
    //        rgb.AddForce(new Vector3(0, -speed, 0));
    //    }
    //    if (Input.GetKey(KeyCode.Q))
    //    {
    //        gameObject.transform.Rotate(Vector3.Lerp(new Vector3(0, -rotSpeed * Time.deltaTime, 0), Vector3.zero, Time.deltaTime));
    //    }
    //    if (Input.GetKey(KeyCode.E))
    //    {
    //        gameObject.transform.Rotate(Vector3.Lerp(new Vector3(0, rotSpeed * Time.deltaTime, 0), Vector3.zero, Time.deltaTime));
    //    }
    //    //호버링 모드
    //    if (count == 0)
    //    {
    //        if (Input.GetKeyDown(KeyCode.H))
    //        {
    //            rgb.velocity = Vector3.Lerp(rgb.velocity, Vector3.zero, Time.deltaTime);
    //            rgb.rotation = Quaternion.Slerp(rgb.rotation, Quaternion.identity, Time.deltaTime);
    //            rgb.useGravity = false;
    //            rgb.freezeRotation = true;
    //            count += 1;
    //        }
    //    }
    //    else
    //    {
    //        if (Input.GetKeyDown(KeyCode.H))
    //        {
    //            rgb.useGravity = true;
    //            rgb.freezeRotation = false;
    //            count = 0;
    //        }
    //    }

    //}
    
}