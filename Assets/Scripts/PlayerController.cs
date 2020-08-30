using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 가만히 있으면 시간이 흘러가고
// 움직이면 시간이 멈추는 게임을 만들어 보자

public class PlayerController : MonoBehaviour
{
    // 1. 키보드 입력에 따라 플레이어를 움직이게 하기
    // 2. 키보드 입력에 따라 점프 가능하게 하기
    // 3. 마우스 입력에 따라 시선 움직이기

    [Header("PlayerCamera")]
    public float cameraSpeed = 200f;

    [Header("PlayerMovement")]
    float moveSpeed;
    public float speed = 10f;
    public float jumpForce = 10f;
    public float gravity = -20f;
    float velocityY;

    

    CharacterController playerCC;
    void Start()
    {
        playerCC = GetComponent<CharacterController>();
    }

    void Update()
    {
        moveSpeed = speed * TimeController.Instance.GetTimeScale;
        // 플레이어 움직임
        PlayerMovement();

        // 플레이어 카메라 움직임
        PlayerCamera();
    }

    void PlayerMovement()
    {
        // 1-1. 키보드 입력을 받아서,
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

        // 1-2. 인풋으로 방향을 만들고,
        Vector3 moveDirection = (transform.right * moveX + transform.forward * moveY).normalized;

        // 중력을 만들기
        velocityY += gravity * Time.deltaTime;

        // 2. 플레이어가 땅을 밟고 있다면, 키보드 입력을 받아서, 점프 힘을 더하고, 중력의 힘으로 다시 바닥으로 내리기
        if (playerCC.isGrounded)
        {
            velocityY = -1;
            if (Input.GetButtonDown("Jump"))
            {
                velocityY = jumpForce;
            }
        }

        // 플레이어 방향에 더해주기
        moveDirection.y = velocityY;

        // 1-3. 플레이어 방향으로 플레이어를 움직이게 하기
        playerCC.Move(moveDirection * moveSpeed * Time.deltaTime);
    }

    float angleX;
    void PlayerCamera()
    {
        // 키보드 입력을 받아서, 방향을 만들어서 적용한다.
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        mouseX *= cameraSpeed * Time.deltaTime;
        
        angleX -= mouseY * cameraSpeed * Time.deltaTime;
        transform.localEulerAngles += new Vector3(0, mouseX, 0);

        angleX = Mathf.Clamp(angleX, -60, 60);
        Camera.main.transform.localEulerAngles = new Vector3(angleX, Camera.main.transform.localEulerAngles.y, Camera.main.transform.localEulerAngles.z);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
   
}
