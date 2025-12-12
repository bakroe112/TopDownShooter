using UnityEngine;

public class WeaponOnGround : MonoBehaviour
{

    public bool allowSwitch;
    public Weapon thisWeapon;
    public GameObject UIDoSomethingFilling;
    public GameObject playerWeapon;
    public GameObject weaponStatuIndicator;
    private void Start()
    {
        GetComponent<SpriteRenderer>().sprite = thisWeapon.weaponImage;
        UIDoSomethingFilling = GameObject.Find("DoSomethingFill");
        playerWeapon = GameObject.Find("Weapon");
        weaponStatuIndicator = GameObject.Find("WeaponStatuIndicator");
    }
    private void Update()
    {
        if (allowSwitch)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (playerWeapon.GetComponent<SwitchWeapon>().playerWeapon[playerWeapon.GetComponent<SwitchWeapon>().nextWeaponID] == null)
                {
                    playerWeapon.GetComponent<SwitchWeapon>().playerWeapon[playerWeapon.GetComponent<SwitchWeapon>().nextWeaponID] = thisWeapon;
                    playerWeapon.GetComponent<SwitchWeapon>().PlayerSwitchOwnedWeapon(thisWeapon);
                }
                else
                    playerWeapon.GetComponent<SwitchWeapon>().PlayerSwitchWeapon(thisWeapon);
                Destroy(gameObject);
                playerWeapon.GetComponent<Fire>().allowFire = false;
            }
        }
        else if (Input.GetMouseButtonUp(0) && !playerWeapon.GetComponent<Fire>().allowFire)
        {
            playerWeapon.GetComponent<Fire>().allowFire = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            UIDoSomethingFilling.GetComponent<DoSomethingUIChange>().ChangeToInteract();
            playerWeapon.GetComponent<Fire>().allowFire = false;
            allowSwitch = true;
            weaponStatuIndicator.GetComponent<WeaponStatu>().Show(thisWeapon);
            CancelInvoke(nameof(weaponInfoHide));
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            UIDoSomethingFilling.GetComponent<DoSomethingUIChange>().ChangeToFire();
            playerWeapon.GetComponent<Fire>().allowFire = true;
            allowSwitch = false;
            Invoke(nameof(weaponInfoHide), 0.1f);
        }
    }
    void weaponInfoHide()
    {
        weaponStatuIndicator.GetComponent<WeaponStatu>().Hide();
    }
}
