using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDetecter : MonoBehaviour
{
    WeaponInfo Winfo;
    int DetectedWeapon;
    public Collider col;
    private void Start()
    {
        col = GetComponent<Collider>();
        col.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        Winfo = other.gameObject.GetComponent<WeaponInfo>();
        if (Winfo)
        {
            other.transform.position = transform.position;
            other.transform.rotation = transform.rotation;
            other.transform.SetParent(transform);

            DetectedWeapon = (int)Winfo.wType;
            PlayerFire.Instance.WeaponGrabed(DetectedWeapon);
            Winfo.isGrabed = false;
        }
    }
}
