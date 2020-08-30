using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFire : MonoBehaviour
{
    public static PlayerFire Instance;
    private void Awake()
    {
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
    // 무기 리스트
    public List<GameObject> WeaponContain = new List<GameObject>();

    // 총알 리스트
    public List<GameObject> bulletPool = new List<GameObject>();

    // 소지할 총알 갯수
    public int bulletAmount = 10;

    // 총알
    public GameObject bulletPrefab;
    // 격발 위치
    public Transform firePoint;
    // 무기 위치
    public Transform weaponPoint;
    // 조준점 가늠쇠
    public CrossHairController CHC;
    // 총알이 날아갈 방향
    Vector3 direction;

    // 마우스 조준지점
    Vector3 aimedPosition;
    GameObject aimedObject;


    void Start()
    {
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
                    CHC.cType = CrossHairController.CursorType.none;
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
                        CHC.cType = CrossHairController.CursorType.circle;

                        //펀치
                        Punch();
                    }
                    // 무기를 겨냥했을때
                    if (aimedObject.CompareTag("Weapon"))
                    {
                        // 커서
                        CHC.cType = CrossHairController.CursorType.grab;
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
                CHC.cType = CrossHairController.CursorType.cross;
                if (!CHC.Onfire)
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
                CHC.cType = CrossHairController.CursorType.circle;
                break;

            default:
                break;
        }
    }

    void Aim()
    {
        // 레이를 쏜다
        // 마우스 클릭 위치로 레이 쏘기(마우스 잠김이라 화면 정 중앙에서 나가는 것같이 됨)
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // 히트다 히트!
        RaycastHit hit;

        // 플레이어가 있는 레이어
        int layer = LayerMask.NameToLayer("Player");

        // 1을 플레이어가 있는 레이어의 값만큼 비트 옮기기
        layer = 1 << layer;

        // 플레이어 있는 레이어를 제외한 레이어에 레이를 쏴서 hit 한 곳 검출하기
        if (Physics.Raycast(ray, out hit, 100f, ~layer))
        {
            // 부딪힌 곳은 aimedPosition, 부딪힌 오브젝트는 aimedObject 로 저장하기
            aimedPosition = hit.point;
            aimedObject = hit.transform.gameObject;
            print(aimedObject);
            //CHC.detect = true;
        }
        // 검출한 hit 없으면 걍 암것도 저장 안 함
        else
        {
            aimedPosition = Vector3.zero;
            aimedObject = null;
            //CHC.detect = false;
        }
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
        // 크로스헤어 회전하기
        CHC.Onfire = true;

        // 시간 살짝 빨라지게하기
        TimeController.Instance.fireTime = true;

        // 리스트에서 총알 가져오기
        GameObject bullet = bulletPool[0];

        // 총알 위치 각도 맞추기
        bullet.transform.position = firePoint.position;

        // 총알이 나아갈 방향 설정하기
        if (aimedPosition != Vector3.zero)
        {
            direction = aimedPosition - firePoint.position;
        }
        else
        {
            direction = Camera.main.transform.forward;
        }

        // 총알의 정면 설정하기
        bullet.transform.forward = direction;

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
        CHC.cType = CrossHairController.CursorType.none;
    }
}
