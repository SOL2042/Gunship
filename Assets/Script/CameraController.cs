using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    [SerializeField]
    private GameObject player;

    private float angleX = 90f, angleY;

    private float offsetX = 0f;
    private float offsetY = 20f;
    private float offsetZ = -20f;

    Camera cam;
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        cam.transform.rotation = Quaternion.Euler(30, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
        angleX -= Input.GetAxis("Mouse Y");
        angleY += Input.GetAxis("Mouse X");
        transform.RotateAround(transform.position, Vector3.right, -angleX);
        transform.RotateAround(transform.position, Vector3.up, angleY);

        cam.transform.LookAt(transform.position);
        

        

        Vector3 camPosition = new Vector3(player.transform.position.x + offsetX, player.transform.position.y + offsetY, player.transform.position.z + offsetZ);



        cam.transform.position = camPosition;
        
        

    }
}
