using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchWeapon : MonoBehaviour
{
    public Image weaponUIImage;
    public Image secondWeaponUIImage;
    public Text weaponUIText;
    public Transform weaponFather;
    public Weapon initialWeapon;
    public List<Weapon> playerWeapon = new List<Weapon>();
    public GameObject weaponName;
    public GameObject nowWeaponName;
    public GameObject weaponOnGround;
    public GameObject nowWeaponOnGround;
    public GameObject canvas;
    public float deltaXForName, deltaYForName;
    public float deltaXForWeapon, deltaYForWeapon;
    public float weaponTextLastTime;
    public int nextWeaponID;

    private void Start()
    {
        weaponUIImage.sprite = GetComponent<Fire>().weaponHeld.weaponImage;
        weaponUIText.text = GetComponent<Fire>().weaponHeld.MpNeed.ToString();
        weaponUIImage.SetNativeSize();

        playerWeapon.Add(null);
        playerWeapon.Add(null);

        PlayerSwitchOwnedWeapon(initialWeapon);
        playerWeapon[0] = initialWeapon;
        nextWeaponID = 1;
    }

    private void Update()
    {
        if (nowWeaponName != null)
            nowWeaponName.transform.position =
                Camera.main.WorldToScreenPoint(transform.parent.position) +
                new Vector3(deltaXForName, deltaYForName, 0);

        if (Input.GetAxisRaw("Mouse ScrollWheel") != 0 && playerWeapon[nextWeaponID] != null)
        {
            PlayerSwitchOwnedWeapon(playerWeapon[nextWeaponID]);
        }
    }

    public void PlayerSwitchWeapon(Weapon weapon)
    {
        nowWeaponOnGround = Instantiate(
            weaponOnGround,
            transform.position + new Vector3(deltaXForWeapon, deltaYForWeapon, 0),
            Quaternion.identity,
            weaponFather);

        nowWeaponOnGround.GetComponent<WeaponOnGround>().thisWeapon =
            this.GetComponent<Fire>().weaponHeld;

        nowWeaponOnGround.GetComponent<SpriteRenderer>().sprite =
            this.GetComponent<Fire>().weaponHeld.weaponImage;

        this.GetComponent<Fire>().weaponHeld = weapon;
        this.GetComponent<Animator>().runtimeAnimatorController = weapon.weaponAnimator;
        this.GetComponent<SpriteRenderer>().sprite = weapon.weaponImage;

        weaponUIImage.sprite = weapon.weaponImage;
        weaponUIText.text = weapon.MpNeed.ToString();
        weaponUIImage.SetNativeSize();

        if (nowWeaponName == null)
        {
            nowWeaponName = Instantiate(
                weaponName,
                Camera.main.WorldToScreenPoint(transform.parent.position) +
                new Vector3(deltaXForName, deltaYForName, 0),
                Quaternion.identity,
                canvas.transform);
        }

        nowWeaponName.GetComponent<Text>().text = weapon.weaponName;
        CancelInvoke(nameof(DestroyText));
        Invoke(nameof(DestroyText), weaponTextLastTime);

        if (nextWeaponID == 0)
            playerWeapon[1] = weapon;
        else if (nextWeaponID == 1)
            playerWeapon[0] = weapon;

        this.GetComponent<Fire>().StopFire();
        this.GetComponent<Fire>().shootTimes = 0;
    }

    public void PlayerSwitchOwnedWeapon(Weapon weapon)
    {
        // bỏ CancleFullShoot + bow + allowFireArrow, chỉ reset knifeSpin
        this.GetComponent<Fire>().knifeSpin = false;

        this.GetComponent<Fire>().weaponHeld = weapon;
        this.GetComponent<Animator>().runtimeAnimatorController = weapon.weaponAnimator;
        this.GetComponent<SpriteRenderer>().sprite = weapon.weaponImage;

        weaponUIImage.sprite = weapon.weaponImage;
        weaponUIText.text = weapon.MpNeed.ToString();
        weaponUIImage.SetNativeSize();

        if (nowWeaponName == null)
        {
            nowWeaponName = Instantiate(
                weaponName,
                Camera.main.WorldToScreenPoint(transform.parent.position) +
                new Vector3(deltaXForName, deltaYForName, 0),
                Quaternion.identity,
                canvas.transform);
        }

        nowWeaponName.GetComponent<Text>().text = weapon.weaponName;
        CancelInvoke(nameof(DestroyText));
        Invoke(nameof(DestroyText), weaponTextLastTime);

        if (nextWeaponID == 0)
            nextWeaponID = 1;
        else if (nextWeaponID == 1)
            nextWeaponID = 0;

        if (playerWeapon[nextWeaponID] != null)
        {
            if (!secondWeaponUIImage.gameObject.activeSelf)
                secondWeaponUIImage.gameObject.SetActive(true);

            secondWeaponUIImage.sprite = playerWeapon[nextWeaponID].weaponImage;
            secondWeaponUIImage.SetNativeSize();
        }
        else
        {
            secondWeaponUIImage.gameObject.SetActive(false);
        }

        this.GetComponent<Fire>().StopFire();
        this.GetComponent<Fire>().shootTimes = 0;
    }

    public void DestroyText()
    {
        if (nowWeaponName != null)
            Destroy(nowWeaponName);
    }
}
