using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossHairController : MonoBehaviour
{
    public enum CursorType
    {
        none,
        cross,
        circle,
        grab
    }
    public CursorType cType;

    public GameObject CrossHair;
    public GameObject CircleCursor;
    public GameObject GrabCursor;

    public float rotateSpeed = 180f;
    float moveSpeed;
    public bool Onfire;
    public bool detect;

    void Start()
    {
        cType = CursorType.none;
        CrossHair.SetActive(false);
        CircleCursor.SetActive(false);
        GrabCursor.SetActive(false);
    }

    void Update()
    {
        moveSpeed = rotateSpeed * TimeController.Instance.GetTimeScale;

        switch (cType)
        {
            case CursorType.none:
                CrossHair.SetActive(false);
                CircleCursor.SetActive(false);
                GrabCursor.SetActive(false);
                break;

            case CursorType.circle:
                // 무기가 없거나 칼, 몽둥이가 있을때
                CircleCursor.SetActive(true);

                CrossHair.SetActive(false);
                GrabCursor.SetActive(false);

                // 한 방 날릴때
                OnFireCursor();
                break;

            case CursorType.cross:
                // 총이 있을때
                CrossHair.SetActive(true);

                CircleCursor.SetActive(false);
                GrabCursor.SetActive(false);

                // 한 방 날릴때
                OnFireCursor();
                break;

            case CursorType.grab:
                // 아무 것도 없을때 
                GrabCursor.SetActive(true);

                CrossHair.SetActive(false);
                CircleCursor.SetActive(false);
                break;

            default:
                break;
        }
    }

    void OnFireCursor()
    {
        if (Onfire)
        {
            RectTransform rt = CrossHair.GetComponent<RectTransform>();
            rt.localEulerAngles += Vector3.forward * moveSpeed * Time.deltaTime;
            if (rt.localEulerAngles.z >= 90)
            {
                rt.localEulerAngles = Vector3.zero;
                Onfire = false;
            }
        }
    }
}
