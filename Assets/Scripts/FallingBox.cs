using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingBox : MonoBehaviour
{
    public float speed = 10f;
    Vector3 startPosition;
    Rigidbody rb;
    int start = 0;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        startPosition = transform.position;
    }
    void Update()
    {
        float moveSpeed = speed * TimeController.Instance.GetTimeScale;
        //rb.AddForce(Vector3.down * moveSpeed / 100f, ForceMode.VelocityChange);

        // 직접 속력을 건드려야 시간이 느려져도 일정하게 움직인다!
        rb.velocity = Vector3.down * moveSpeed;
        //transform.position += Vector3.down * moveSpeed * Time.deltaTime;

    }

    private void OnTriggerEnter(Collider collision)
    {
        transform.position = startPosition;

        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        BulletController BC = collision.GetComponent<BulletController>();
        if (BC) BC.destroy = true;
    }
}
