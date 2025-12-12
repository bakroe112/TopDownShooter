using UnityEngine;
using UnityEngine.UI;

public class Fire : MonoBehaviour
{
    public GameObject player;
    public GameObject weaponCenter;
    public GameObject aimLineCenter;
    public GameObject aimLineUp;
    public GameObject aimLineDown;
    public GameObject MPWarning;

    public Player playerType;
    public Animator weaponAnimator;

    public Transform aimLineCenterTransform;
    public Transform aimLineUpTransform;
    public Transform aimLineDownTransform;
    public Transform weaponCenterTransform;
    public Transform playerTransform;
    public Transform centerTransform;

    public Weapon weaponHeld;
    public Slider MPSlider;
    public Text MPText;

    public int shootTimes;
    public int MP;

    public bool displayAimLine;
    public bool allowFire = true;
    public bool spin;
    public bool knifeSpin, up, down;

    public float shottingCounter;
    public float multipleShootingCounter;
    public float changeSpeed;

    public float initScatteringAngle;
    public float initCritChance;
    public float initDamage;

    public float angleZ;
    public float comboCounter;

    void Start()
    {
        weaponCenterTransform = weaponCenter.transform;
        playerTransform = player.transform;
        aimLineCenterTransform = aimLineCenter.transform;
        aimLineUpTransform = aimLineUp.transform;
        aimLineDownTransform = aimLineDown.transform;

        MP = playerType.fullMP;
        MPSlider.maxValue = MP;
        MPSlider.value = MP;

        initCritChance = weaponHeld.critChance;
        initScatteringAngle = weaponHeld.scatteringAngle;
        initDamage = weaponHeld.damage;
    }

    void Update()
    {
        if (Time.timeScale != 0)
        {
            if (shottingCounter >= Time.deltaTime)
                shottingCounter -= Time.deltaTime;
            else
                shottingCounter = 0;

            if (multipleShootingCounter >= Time.deltaTime)
                multipleShootingCounter -= Time.deltaTime;
            else
                multipleShootingCounter = 0;

            Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 delta = new Vector2(mouseWorldPos.x - centerTransform.position.x,
                                        mouseWorldPos.y - centerTransform.position.y);

            if (mouseWorldPos.x - centerTransform.position.x > 0)
                playerTransform.localScale = new Vector3(1, 1, 1);
            else
                playerTransform.localScale = new Vector3(-1, 1, 1);

            angleZ = Mathf.Atan(delta.y / delta.x) * 180 / Mathf.PI;

            if (!knifeSpin)
                weaponCenterTransform.eulerAngles = new Vector3(0, 0, angleZ);

            // hiển thị đường ngắm
            if (displayAimLine)
            {
                if (!aimLineCenter.activeSelf)
                    aimLineCenter.SetActive(true);

                aimLineCenterTransform.eulerAngles = new Vector3(0, 0, angleZ);

                if (aimLineUpTransform.localEulerAngles.z != weaponHeld.scatteringAngle)
                    aimLineUpTransform.localEulerAngles = new Vector3(0, 0, weaponHeld.scatteringAngle);

                if (aimLineDownTransform.localEulerAngles.z != weaponHeld.scatteringAngle)
                    aimLineDownTransform.localEulerAngles = new Vector3(0, 0, -weaponHeld.scatteringAngle);
            }
            else
            {
                aimLineCenter.SetActive(false);
            }

            // bắn
            if (Input.GetMouseButton(0) && MP >= weaponHeld.MpNeed && allowFire && shootTimes == 0)
            {
                if (shottingCounter == 0)
                {
                    OpenFire(angleZ);
                    shottingCounter = weaponHeld.shootingGap;
                    multipleShootingCounter = weaponHeld.continuousShootingGap;

                    if (weaponHeld.type == Weapon.WeaponType.gun)
                        MP -= weaponHeld.MpNeed;
                }
                spin = true;
            }
            else
            {
                StopFire();
                spin = false;
            }

            // burst nhiều lần theo shootTimes
            if (shootTimes != 0 && shootTimes < weaponHeld.shootTimes)
            {
                if (multipleShootingCounter == 0)
                {
                    OpenFire(angleZ);
                    multipleShootingCounter = weaponHeld.continuousShootingGap;
                }
            }
            else if (shootTimes == weaponHeld.shootTimes)
            {
                shootTimes = 0;
            }

            comboCounter += Time.deltaTime;
        }

        // MP bar mượt
        if (MPSlider.value > MP + changeSpeed * Time.deltaTime)
            MPSlider.value -= changeSpeed * Time.deltaTime;
        else if (MPSlider.value < MP - changeSpeed * Time.deltaTime)
            MPSlider.value += changeSpeed * Time.deltaTime;
        else
            MPSlider.value = MP;

        if (MPText.text != MP.ToString() + "/" + playerType.fullMP.ToString())
            MPText.text = MP.ToString() + "/" + playerType.fullMP.ToString();

        DisplayWarning();
        KnifeSpin();
    }

    private void OpenFire(float angle)
    {
        if (weaponHeld.type == Weapon.WeaponType.gun)
        {
            float randomAngle;
            shootTimes++;

            // nhiều viên / 1 lần bắn
            if (weaponHeld.bulletCount > 1)
            {
                for (int i = 0; i < weaponHeld.bulletCount; i++)
                {
                    int bulletDamage = (int)(((UnityEngine.Random.Range(0f, 1f) < weaponHeld.critChance)
                        ? weaponHeld.critDamage : 1) * weaponHeld.damage);

                    if (weaponHeld.weightedRandomScheduling)
                    {
                        if (UnityEngine.Random.Range(0f, 1f) > 0.3f)
                            randomAngle = UnityEngine.Random.Range(-weaponHeld.scatteringAngle * 0.3f, weaponHeld.scatteringAngle * 0.3f);
                        else
                            randomAngle = UnityEngine.Random.Range(-weaponHeld.scatteringAngle, weaponHeld.scatteringAngle);
                    }
                    else
                    {
                        randomAngle = UnityEngine.Random.Range(-weaponHeld.scatteringAngle, weaponHeld.scatteringAngle);
                    }

                    GameObject bullet = Instantiate(
                        weaponHeld.bullet,
                        weaponCenterTransform.position,
                        Quaternion.Euler(0, 0,
                            (playerTransform.localScale.x > 0) ? angle + randomAngle : 180 + angle + randomAngle));

                    bullet.GetComponent<GunBullet>().damageToken = bulletDamage;
                    weaponAnimator.SetTrigger("Fire");

                    if (bulletDamage == weaponHeld.critDamage * weaponHeld.damage)
                        bullet.GetComponent<GunBullet>().isCrit = true;
                }
            }
            // 1 viên / 1 lần bắn
            else
            {
                if (weaponHeld.weightedRandomScheduling)
                {
                    if (UnityEngine.Random.Range(0f, 1f) > 0.3f)
                        randomAngle = UnityEngine.Random.Range(-weaponHeld.scatteringAngle * 0.3f, weaponHeld.scatteringAngle * 0.3f);
                    else
                        randomAngle = UnityEngine.Random.Range(-weaponHeld.scatteringAngle, weaponHeld.scatteringAngle);
                }
                else
                {
                    randomAngle = UnityEngine.Random.Range(-weaponHeld.scatteringAngle, weaponHeld.scatteringAngle);
                }

                int bulletDamage = (int)(((UnityEngine.Random.Range(0f, 1f) < weaponHeld.critChance)
                    ? weaponHeld.critDamage : 1) * weaponHeld.damage);

                GameObject bullet = Instantiate(
                    weaponHeld.bullet,
                    weaponCenterTransform.position,
                    Quaternion.Euler(0, 0,
                        (playerTransform.localScale.x > 0) ? angle + randomAngle : 180 + angle + randomAngle));

                bullet.GetComponent<GunBullet>().damageToken = bulletDamage;
                weaponAnimator.SetTrigger("Fire");

                if (bulletDamage == weaponHeld.critDamage * weaponHeld.damage)
                    bullet.GetComponent<GunBullet>().isCrit = true;
            }
        }
        else if (weaponHeld.type == Weapon.WeaponType.knife)
        {
            GameObject playerHandKnife;

            if (comboCounter >= weaponHeld.shootingGap + weaponHeld.comboDelta)
            {
                down = true;
                up = false;
            }
            else
            {
                if (down)
                {
                    down = false;
                    up = true;
                }
                else if (up)
                {
                    down = true;
                    up = false;
                }
                else
                {
                    down = true;
                    up = false;
                }
            }

            if (player.transform.localScale.x > 0)
            {
                playerHandKnife = Instantiate(
                    weaponHeld.knife,
                    transform.position + new Vector3(
                        weaponHeld.knifeDeltaX * Mathf.Cos(transform.parent.eulerAngles.z / 180 * Mathf.PI),
                        weaponHeld.knifeDeltaX * Mathf.Sin(transform.parent.eulerAngles.z / 180 * Mathf.PI),
                        0),
                    transform.rotation);

                playerHandKnife.transform.localScale = weaponHeld.knifeScale;
            }
            else
            {
                playerHandKnife = Instantiate(
                    weaponHeld.knife,
                    transform.position + new Vector3(
                        -weaponHeld.knifeDeltaX * Mathf.Cos(transform.parent.eulerAngles.z / 180 * Mathf.PI),
                        -weaponHeld.knifeDeltaX * Mathf.Sin(transform.parent.eulerAngles.z / 180 * Mathf.PI),
                        0),
                    transform.rotation);

                playerHandKnife.transform.localScale = weaponHeld.knifeScale;
                playerHandKnife.transform.localScale = new Vector3(
                    -playerHandKnife.transform.localScale.x,
                    -playerHandKnife.transform.localScale.y,
                    playerHandKnife.transform.localScale.z);
            }

            if (up)
            {
                playerHandKnife.transform.localScale = new Vector3(
                    playerHandKnife.transform.localScale.x,
                    -playerHandKnife.transform.localScale.y,
                    playerHandKnife.transform.localScale.z);
            }

            knifeSpin = true;

            if (up)
            {
                weaponCenterTransform.eulerAngles = new Vector3(0, 0, 320);
                knifeSpin = true;
            }
            else if (down)
            {
                weaponCenterTransform.eulerAngles = new Vector3(0, 0, 60);
                knifeSpin = true;
            }

            comboCounter = 0;
        }
    }

    void DisplayWarning()
    {
        if (MP <= 0.3f * playerType.fullMP)
            MPWarning.SetActive(true);
        else
            MPWarning.SetActive(false);
    }

    public void StopFire()
    {
        if (weaponAnimator.runtimeAnimatorController != null)
            weaponAnimator.SetBool("Fire", false);
    }

    public void KnifeSpin()
    {
        if (knifeSpin && up)
        {
            if (weaponCenterTransform.eulerAngles.z >= 320 || weaponCenterTransform.eulerAngles.z <= 60)
                weaponCenterTransform.eulerAngles += new Vector3(0f, 0f, weaponHeld.knifeSpeed * Time.deltaTime);
            else
                knifeSpin = false;
        }
        else if (knifeSpin && down)
        {
            if (weaponCenterTransform.eulerAngles.z >= 320 || weaponCenterTransform.eulerAngles.z <= 61)
                weaponCenterTransform.eulerAngles -= new Vector3(0f, 0f, weaponHeld.knifeSpeed * Time.deltaTime);
            else
                knifeSpin = false;
        }
    }
}
