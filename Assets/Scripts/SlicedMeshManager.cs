using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof (Rigidbody))]
public class SlicedMeshManager : MonoBehaviour
{
    Rigidbody rb;
    Vector3 timeVelocity;
    public float givenSpeed;
    BoxCollider bc;
    void Start()
    {
        // 중력 따로 만들어서 적용하기!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        rb = GetComponent<Rigidbody>();

        // 트리거용 콜라이더 따로 붙이기
        gameObject.AddComponent<BoxCollider>();
        bc = GetComponent<BoxCollider>();
        bc.isTrigger = true;
        bc.size = Vector3.one * 1.1f;
    }

    void Update()
    {
        // 주어진 속도가 0이 아니면 점차 줄어들기!
        if (givenSpeed > 0)
            givenSpeed -= Time.deltaTime * TimeController.Instance.GetTimeScale;

        // 주어진 속도가 0이면 그냥 0
        else
            givenSpeed = 0;

        // TimeController의 시간 속도에 따라 rb의 속도 변화시키기
        timeVelocity = Vector3.one * givenSpeed * TimeController.Instance.GetTimeScale;
        rb.velocity = timeVelocity;
    }
}
