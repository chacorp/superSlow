using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float speed = 30f;
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

        if (timer > disableTime || destroy)
        {
            timer = 0;
            bulletPool.Add(gameObject);
            destroy = false;
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);

        if (other.CompareTag("Enemy"))
        {
            // MeshCut으로 자른 녀석들 담아두기
            GameObject[] CutMesh = MeshCut.Cut(other.gameObject, transform.position, transform.right, capMat);

            // MESHCUT 적용한 녀석들에게 컴포넌트 붙이기
            for (int i = 0; i < CutMesh.Length; i++)
            {
                CutMesh[i].tag = ("Enemy");

                // Rigidbody            붙이기
                if (!CutMesh[i].GetComponent<Rigidbody>())
                    CutMesh[i].AddComponent<Rigidbody>();

                // Collider             붙이기
                if (!CutMesh[i].GetComponent<Collider>())
                    CutMesh[i].AddComponent<BoxCollider>();

                // SlicedMeshManager    붙이기
                if (!CutMesh[i].GetComponent<SlicedMeshManager>())
                {
                    CutMesh[i].AddComponent<SlicedMeshManager>();
                    CutMesh[i].GetComponent<SlicedMeshManager>().givenSpeed = speed / 10f;
                }
            }

            // 총알 되돌리기
            destroy = true;
        }
    }
}
