using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingBox : MonoBehaviour
{
    public float speed = 10f;
    Vector3 startPosition;
    Rigidbody rb;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        startPosition = transform.position;
    }
    void Update()
    {
        float moveSpeed = speed * TimeController.Instance.GetTimeScale;
        transform.position += Vector3.down * moveSpeed * Time.deltaTime;
        //rb.AddForce(Vector3.down * moveSpeed, ForceMode.Acceleration);
    }

    private void OnTriggerEnter(Collider collision)
    {
        transform.position = startPosition;
    }
}
