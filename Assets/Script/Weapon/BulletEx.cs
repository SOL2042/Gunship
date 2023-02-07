using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletEx : MonoBehaviour
{
    Transform parent;
    TrailRenderer trailRenderer;

    public float speed;
    public float lifetime;

    public void Fire(float launchSpeed, int layer)
    {
        speed += launchSpeed;
        gameObject.layer = layer;
    }

    void OnTriggerEnter(Collider other)
    {
        CancelInvoke("DisableBullet");
        DisableBullet();
    }

    void DisableBullet()
    {
        gameObject.SetActive(false);
        transform.parent = parent;
    }

    void OnEnable()
    {
        trailRenderer.Clear();
        Invoke("DisableBullet", lifetime);
    }

    void Awake()
    {
        trailRenderer = GetComponent<TrailRenderer>();
        parent = transform.parent;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(new Vector3(0, 0, speed * Time.deltaTime));
    }
}
