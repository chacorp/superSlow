using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeController : MonoBehaviour
{
    public static TimeController Instance;
    private void Awake()
    {
        Instance = this;

    }
    float timeScale { get; set; }
    public float GetTimeScale
    {
        get
        {
            return timeScale;
        }
    }

    [Header("TimeControl")]
    public float decreasingAmount = 1.5f;
    public bool fireTime;
    void Update()
    {
        // 시간의 움직임
        TimeControl();
    }


    public void TimeControl()
    {
        // 움직임의 절댓값 가져오기
        float moveX = Input.GetAxis("Horizontal"); // -1 0 1
        float moveY = Input.GetAxis("Vertical");   // -1 0 1

        float magni = (new Vector2(moveX, moveY) * Mathf.Sqrt(2)).magnitude;

        if (fireTime)
        {
            timeScale = 1;
            //timeScale = 0.1f;
            fireTime = false;
        }
        if(magni <= 0.1f)
        {
            magni = 0.1f;
        }
        // print(moveX); // 값이 0.1 아니면 1이 나오느 이유는 normalized 때문인듯

        //움직임이 있을 때의 시간 흐름
        if (magni != 0.1f)
        {
            //timeScale = 1 / (magni * 10);
            timeScale = magni;
        }

        //움직임이 없을 때의 시간 흐름
        else
        {
            timeScale -= decreasingAmount * Time.deltaTime;
            if (timeScale <= 0.1f)
            {
                timeScale = 0.1f;
            }
            //timeScale += decreasingAmount * Time.deltaTime;
            //if (timeScale >= 1)
            //{
            //    timeScale = 1f;
            //}
        }

        //Time.timeScale = timeScale;

    }
}
