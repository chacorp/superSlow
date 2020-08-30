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
    public CursorType crossHair;

    [Header("CrossHair Sprites")]
    public GameObject AttackCursor;
    public GameObject CircleCursor;
    public GameObject GrabCursor;

    [Header("Properties")]
    public float rotateSpeed = 180f;
    float moveSpeed;
    public bool Onfire;
    public bool detect;

    void Start()
    {
        crossHair = CursorType.none;
        AttackCursor.SetActive(false);
        CircleCursor.SetActive(false);
        GrabCursor.SetActive(false);
    }
    void SpinCursor()
    {
        if (Onfire)
        {
            RectTransform rt = AttackCursor.GetComponent<RectTransform>();
            rt.localEulerAngles += Vector3.forward * moveSpeed * Time.deltaTime;
            if (rt.localEulerAngles.z >= 90)
            {
                rt.localEulerAngles = Vector3.zero;
                Onfire = false;
            }
        }
    }

    void Update()
    {
        moveSpeed = rotateSpeed * TimeController.Instance.GetTimeScale;

        switch (crossHair)
        {
            case CursorType.none:
                AttackCursor.SetActive(false);
                CircleCursor.SetActive(false);
                GrabCursor.SetActive(false);
                break;

            case CursorType.circle:
                // 무기가 없거나 칼, 몽둥이가 있을때
                CircleCursor.SetActive(true);

                AttackCursor.SetActive(false);
                GrabCursor.SetActive(false);

                // 한 방 날릴때
                SpinCursor();
                break;

            case CursorType.cross:
                // 총이 있을때
                AttackCursor.SetActive(true);

                CircleCursor.SetActive(false);
                GrabCursor.SetActive(false);

                // 한 방 날릴때
                SpinCursor();
                break;

            case CursorType.grab:
                // 아무 것도 없을때 
                GrabCursor.SetActive(true);

                AttackCursor.SetActive(false);
                CircleCursor.SetActive(false);
                break;

            default:
                break;
        }
    }

}
