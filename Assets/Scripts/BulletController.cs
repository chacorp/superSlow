using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float speed = 20f;
    float timer;
    float disableTime = 1.5f;
    public bool destroy;
    List<GameObject> bulletPool;

    public Material capMat;

    private void Start()
    {
        destroy = false;
        bulletPool = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerFire>().bulletPool;
    }
    
    void Update()
    {
        float moveSpeed = speed * TimeController.Instance.GetTimeScale;
        transform.position += transform.forward * moveSpeed * Time.deltaTime;
        timer += Time.deltaTime * TimeController.Instance.GetTimeScale;

        if(timer > disableTime || destroy)
        {
            timer = 0;
            bulletPool.Add(gameObject);
            destroy = false;
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            GameObject[] CutMesh = MeshCut.Cut(other.gameObject, transform.position, transform.right, capMat);
        }
    }
}
