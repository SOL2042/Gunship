using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    [SerializeField]
    private GameObject player;                        // 카메라로 비춰주는 대상

    private float finalAngleY; // 최종적으로 도달할 Y 각도

    private float targetX; // 상하 회전 각도
    private float targetY; // 좌우 회전 각도
    private float distance = 30f; // 플레이어와 카메라의 거리

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
            cam.transform.position = player.transform.position - (Quaternion.Euler(targetX, targetY, 0) * Vector3.forward * hitDistance) + Vector3.up * 10;
        }
        else
        {
            cam.transform.position = player.transform.position - (Quaternion.Euler(targetX, targetY, 0) * Vector3.forward * distance) + Vector3.up * 10;
        }
        cam.transform.rotation = Quaternion.Euler(targetX, targetY, 0);
        player.transform.rotation = Quaternion.Euler(0, finalAngleY, 0);
        cam.farClipPlane = 100000f;
    }


    private void PlayerZoom()
    {
        float zoomSensy = 1 - Input.GetAxis("Mouse ScrollWheel");
        distance *= zoomSensy;

        distance = Mathf.Clamp(distance, 1, 40);
    }



}