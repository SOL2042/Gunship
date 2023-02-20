using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    [SerializeField]
    private GameObject player;                        // ī�޶�� �����ִ� ���

    private float finalAngleY; // ���������� ������ Y ����

    private float targetX; // ���� ȸ�� ����
    private float targetY; // �¿� ȸ�� ����
    private float distance = 30f; // �÷��̾�� ī�޶��� �Ÿ�

  
    [SerializeField]
    Camera cam;
    // Start is called before the first frame update
    void Start()
    {
        
        
    }

    // Update is called once per frame
    void Update()
    {
        PlayerRotate();
        PlayerZoom();
    }
    private void PlayerRotate()
    {
        float axisX = Input.GetAxis("Mouse Y");
        float axisY = Input.GetAxis("Mouse X");

        targetX -= axisX;
        targetY += axisY;

        targetX = Mathf.Clamp(targetX, -90, 90);

        finalAngleY = Mathf.Lerp(finalAngleY, targetY, Time.deltaTime * 20f);

        if (Physics.Raycast(player.transform.position, Quaternion.Euler(targetX, targetY, 0) * -Vector3.forward, out RaycastHit hit, distance, ~(1 << LayerMask.NameToLayer("Player"))))
        {
            float hitDistance = hit.distance;
            cam.transform.position = player.transform.position - (Quaternion.Euler(targetX, targetY, 0) * Vector3.forward * hitDistance);
        }
        else
        {
            cam.transform.position = player.transform.position - (Quaternion.Euler(targetX, targetY, 0) * Vector3.forward * distance);
        }
        cam.transform.rotation = Quaternion.Euler(targetX, targetY, 0);
        player.transform.rotation = Quaternion.Euler(0, finalAngleY, 0);
    }


    private void PlayerZoom()
    {
        float zoomSensy = 1 - Input.GetAxis("Mouse ScrollWheel");
        distance *= zoomSensy;
    }

   

}
