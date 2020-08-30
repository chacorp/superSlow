using System;
using UnityEngine;

// 먼저 플레이어의 카메라로부터 레이를 쏴서 Ray
// 레이가 맞은 물체가 어떤 것인지 가져온다 GameObject
// 다른 스크립트에게 레이 맞은 물체를 알려준다 public
public class RayManager : MonoBehaviour
{
    public GameObject hitObject;
    public Vector3 hitPosition;
    public float rayLength;

    CrossHairController CHC;

    private void Start()
    {
        CHC = GetComponent<CrossHairController>();
    }

    private void ShootRay()
    {
        // MainCamera의 위치에서 정면 방향으로 나가는 Ray 만들기
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        RaycastHit hit;

        // 검출 제외할 레이어 설정하기
        int layerP = LayerMask.NameToLayer("Player");
        int layerG = LayerMask.NameToLayer("Ground");

        if(CHC.crossHair == CrossHairController.CursorType.cross)
        {
            rayLength = 20f;
        }
        else
        {
            rayLength = 5f;
        }
        // Ray쏘기!! 길이는 rayLength 만큼 
        if (Physics.Raycast(ray, out hit, rayLength, ~layerP))
        {
            hitPosition = hit.point;

            // 만약 땅에 맞았다면 오브젝트는 null
            if (hit.transform.gameObject.layer == layerG)
                hitObject = null;

            else
                hitObject = hit.transform.gameObject;
        }
        else
        {
            // Ray 끝단의 위치
            hitPosition = ray.GetPoint(rayLength);
            hitObject = null;
        }
    }

    public GameObject GetHitObject()
    {
        return hitObject;
    }

    public Vector3 GetHitPosition()
    {
        return hitPosition;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * rayLength);
    }

    private void Update()
    {
        ShootRay();
    }
}
