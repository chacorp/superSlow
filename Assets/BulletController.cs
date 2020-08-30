using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float speed = 20f;
    float timer;
    float disableTime = 2f;

    List<GameObject> bulletPool;

    private void Start()
    {
        bulletPool = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerFire>().bulletPool;
    }
    
    void Update()
    {
        float moveSpeed = speed * TimeController.Instance.GetTimeScale;
        transform.position += transform.forward * moveSpeed * Time.deltaTime;
        timer += Time.deltaTime * TimeController.Instance.GetTimeScale;

        if(timer > disableTime)
        {
            timer = 0;
            bulletPool.Add(gameObject);
            gameObject.SetActive(false);
        }
    }
}
