using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    [SerializeField]
    private GameObject player;                        // ī�޶�� �����ִ� ���

    private float finalAngleY;                        // ���������� ������ Y ����

    private float targetX;                            // ���� ȸ�� ����
    private float targetY;                            // �¿� ȸ�� ����
    private float distance = 30f;                     // �÷��̾�� ī�޶��� �Ÿ�
    float count = 0;

    float duration = 2f;


    [SerializeField]
    Camera cam;

    private void Start()
    {
        
    }
    void Update()
    {
        PlayerRotate();
        PlayerZoom();
        Debug.Log($"distance : {distance}");
        
        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (count == 0)
            {
                StopAllCoroutines();
                StartCoroutine(QuickPlayerZoomIn());
                count = -1;
            }
            else
            {
                StopAllCoroutines();
                StartCoroutine(QuickPlayerZoomOut());
                count = 0;
            }
        }
        
        
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
        distance = Mathf.Lerp(distance, distance *= zoomSensy, Time.deltaTime * 1000f);
        distance = Mathf.Clamp(distance, 1, 40);
    }

    IEnumerator QuickPlayerZoomIn()
    {
        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            float t = Mathf.Clamp01(timer / duration); // ������ ����� �ð� �� ���

            // ���� ó��
            distance = Mathf.Lerp(distance, 1f, t);

            yield return null;
        }
    }

    IEnumerator QuickPlayerZoomOut()
    {
        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            float t = Mathf.Clamp01(timer / duration); // ������ ����� �ð� �� ���

            // ���� ó��
            distance = Mathf.Lerp(distance, 30f, t);

            yield return null;
        }
    }

}