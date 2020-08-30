using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFire : MonoBehaviour
{
    public static PlayerFire Instance;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    public enum WeaponHold
    {
        none = -1,
        Pistol,
        ShotGun,
        Sword
    }
    public WeaponHold weaponH;

    [Header("GameObject List")]
    // 무기 리스트
    public List<GameObject> WeaponContain = new List<GameObject>();

    // 총알 리스트
    public List<GameObject> bulletPool = new List<GameObject>();

    // 소지할 총알 갯수
    public int bulletAmount = 10;



    [Header("Prefabs and Transforms")]
    // 총알
    public GameObject bulletPrefab;
    // 격발 위치
    public Transform firePoint;
    // 무기 위치
    public Transform weaponPoint;



    [Header("CrossHairConroller Script")]
    public GameObject CH_and_ray;
    // 조준점 가늠쇠
    CrossHairController crossHairC;
    // Ray!
    RayManager rayManager;



    // 총알이 날아갈 방향
    public Vector3 direction;

    // 마우스 조준지점
    Vector3 aimedPosition;
    GameObject aimedObject;



    void Start()
    {
        crossHairC = CH_and_ray.GetComponent<CrossHairController>();
        rayManager = CH_and_ray.GetComponent<RayManager>();

        weaponH = WeaponHold.none;
        // 총알 만들기
        for (int i = 0; i < bulletAmount; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab);
            bullet.SetActive(false);
            bulletPool.Add(bullet);
        }
    }

    void Update()
    {
        // 겨냥하고 
        Aim();

        // 소지하고 있는 무기에 따라 기능바꾸기
        switch (weaponH)
        {
            // 아무 무기가 없을때
            case WeaponHold.none:
                //아무것도 겨냥하고 있지 않을때 가늠쇠 모양
                if (!aimedObject)
                {
                    //print("1");
                    crossHairC.crossHair = CrossHairController.CursorType.none;
                    //아무 것도 안함
                }
                // 뭔가 있을때
                if (aimedObject)
                {
                    //CHC.detect = true;
                    //print("2");
                    // 적을 겨냥했을때
                    if (aimedObject.CompareTag("Enemy"))
                    {
                        // 커서
                        crossHairC.crossHair = CrossHairController.CursorType.circle;

                        //펀치
                        Punch();
                    }
                    // 무기를 겨냥했을때
                    if (aimedObject.CompareTag("Weapon"))
                    {
                        // 커서
                        crossHairC.crossHair = CrossHairController.CursorType.grab;
                        //
                        if (Input.GetButtonDown("Fire1"))
                        {
                            WeaponInfo wi = aimedObject.GetComponent<WeaponInfo>();
                            if (wi)
                            {
                                weaponPoint.GetComponent<Collider>().enabled = true;
                                wi.weaponTransform = weaponPoint;
                                wi.isGrabed = true;
                            }
                        }
                    }
                }

                break;

            // 총이 있을때
            case WeaponHold.Pistol:
            case WeaponHold.ShotGun:
                // 가늠쇠 모양
                crossHairC.crossHair = CrossHairController.CursorType.cross;
                if (!crossHairC.Onfire)
                {
                    if (Input.GetButtonDown("Fire1"))
                    {
                        GunFire();
                    }
                    if (Input.GetButtonDown("Fire2"))
                    {
                        TossWeapon();
                    }
                }
                break;

            // 칼이 있을때
            case WeaponHold.Sword:
                // 가늠쇠 모양
                crossHairC.crossHair = CrossHairController.CursorType.circle;
                break;

            default:
                break;
        }
    }

    void Aim()
    {
        // rayManager로부터 데이커 가져오기
        aimedObject = rayManager.GetHitObject();
        aimedPosition = rayManager.GetHitPosition();
    }

    void Punch()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            print("Punched");
        }
    }

    void GunFire()
    {
        // 1. 크로스헤어 회전하기
        crossHairC.Onfire = true;


        // 2. 시간 살짝 빨라지게하기
        TimeController.Instance.fireTime = true;


        // 3. 총알 쏘기

        // 리스트에서 총알 가져오기
        GameObject bullet = bulletPool[0];

        // 총알 위치 맞추기
        bullet.transform.position = firePoint.position;

        // 총알이 나아갈 방향 설정하기
        Vector3 Ndirection = aimedPosition - firePoint.position;


        // 총알의 정면 설정하기
        bullet.transform.forward = Ndirection;

        // 활성화하기
        bullet.SetActive(true);

        // 리스트에서 총알 제외하기
        bulletPool.Remove(bullet);

    }


    // 무기 바꾸기
    public void WeaponGrabed(int weapon)
    {
        // 소지하는 무기 = enum WeaponHold 의 weapon 번째 무기로 바꾸기
        weaponH = (WeaponHold)weapon;
        print(weaponH);
    }

    // 무기 던지기
    void TossWeapon()
    {
        weaponPoint.GetComponent<Collider>().enabled = false;
        // 던질 무기 가져오기
        GameObject weapon = weaponPoint.GetChild(0).gameObject;

        // 던질 무기 독립시키기
        weapon.transform.SetParent(null);

        // 던질 무기한테 던져버린다고 말하기
        WeaponInfo wi = weapon.GetComponent<WeaponInfo>();
        if (wi)
        {
            wi.isTossed = true;
        }
        // 시간 살짝 빨라지게하기
        TimeController.Instance.fireTime = true;

        // 이제 갖고 있는 무기는 없음
        weaponH = WeaponHold.none;
        crossHairC.crossHair = CrossHairController.CursorType.none;
    }
}
