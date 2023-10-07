using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private static Player _instance;

    public static Player instance
    {
        get
        {
            if (_instance == null) _instance = FindObjectOfType<Player>();
            return _instance;
        }
    }

    public Rigidbody rgb;

    [SerializeField] Transform bulletPosition;
    [SerializeField] Transform enemyPosition;

    [SerializeField] private float responsiveness = 100000f;
    [SerializeField] private float throttleAmt = 25f;
    public float throttle;
    
    private float roll;
    private float pitch;
    private float yaw;

    public float speed;
   
    Vector3 rotateValue;
    private void Awake()
    {
        rgb = GetComponent<Rigidbody>();
    }
    void Start()
    {
        speed = rgb.velocity.z;
        Mathf.Clamp(gameObject.transform.position.y, -1, 200);


    }
    void Update()
    {
        
    }
    private void FixedUpdate()
    {
        HandleInputs();
        Hover();
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
        throttle = Mathf.Clamp(throttle, 0f, 100f);

        if (gameObject.GetComponent<WeaponController>().totalData.flyMode == FlyMode.Default)
        {
            rgb.AddForce(transform.up * throttle * 0.07f, ForceMode.Impulse);
        }
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S))
        {
            rgb.AddTorque(transform.right * pitch * responsiveness * 10f);
            rgb.AddForce(transform.forward * pitch * 100f);
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            rgb.AddTorque(transform.forward * -roll * responsiveness);
            rgb.AddForce(transform.right * -roll);
        }
        rgb.AddTorque(transform.up * yaw * responsiveness * 7f);
    }
    
    //호버링 모드
    private void Hover()
    {
        if (gameObject.GetComponent<WeaponController>().totalData.flyMode == FlyMode.Default)
        {
            if (Input.GetKeyDown(KeyCode.H))
            {
                gameObject.GetComponent<WeaponController>().totalData.flyMode = FlyMode.Hover;
                rgb.useGravity = false;
                rgb.velocity = Vector3.Slerp(rgb.velocity, Vector3.zero, Time.deltaTime);
                gameObject.transform.rotation = Quaternion.RotateTowards(gameObject.transform.rotation, Quaternion.Euler(0, 0, 0), Time.deltaTime);
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.H))
            {
                gameObject.GetComponent<WeaponController>().totalData.flyMode = FlyMode.Default;
                rgb.useGravity = true;
            }
        }
    }
}