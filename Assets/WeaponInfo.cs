using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponInfo : MonoBehaviour
{
    public enum WeaponType
    {
        Pistol,
        ShotGun,
        Sword
    }
    public WeaponType wType;

    public bool isGrabed;
    public bool isTossed;
    public Transform weaponTransform;

    float speed = 10f;

    Rigidbody rb;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = true;
    }
    void Update()
    {
        if (isGrabed)
        {
            rb.isKinematic = true;
            Grabed();
        }

        if (isTossed)
        {
            rb.isKinematic = false;
            Tossed();
        }
    }

    private void Tossed()
    { // 질문
        transform.position += transform.forward * speed * Time.deltaTime;
        // rb.AddForce(transform.forward * (speed / 2) * Time.deltaTime, ForceMode.Impulse);
    }

    private void Grabed()
    {
        Vector3 direction = weaponTransform.position - transform.position;
        direction.Normalize();
        transform.position += direction * speed * Time.deltaTime;
        transform.rotation = Quaternion.Lerp(transform.rotation, weaponTransform.rotation, speed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
        {
            isTossed = false;
        }
    }
}
