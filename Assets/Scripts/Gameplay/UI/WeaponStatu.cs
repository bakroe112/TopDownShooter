using UnityEngine;
using UnityEngine.UI;

public class WeaponStatu : MonoBehaviour
{
    public Text attack, mp, crit, scattering, weaponName;
    public bool show;
    public float speed;

    public void Show(Weapon weapon)
    {
        attack.text = weapon.damage.ToString();
        mp.text = weapon.MpNeed.ToString();
        crit.text = weapon.critChance.ToString();

        // Không còn bow nữa, dùng luôn scatteringAngle
        scattering.text = weapon.scatteringAngle.ToString();

        weaponName.text = weapon.weaponName;
        show = true;
    }

    public void Hide()
    {
        show = false;
    }

    private void Update()
    {
        // -80 -> 60
        if (show)
        {
            if (transform.position.y <= 60 - speed * Time.deltaTime)
            {
                transform.position += new Vector3(0, speed * Time.deltaTime, 0);
            }
            else
            {
                transform.position = new Vector3(transform.position.x, 60, 0);
            }
        }
        else
        {
            if (transform.position.y >= -80 + speed * Time.deltaTime)
            {
                transform.position -= new Vector3(0, speed * Time.deltaTime, 0);
            }
            else
            {
                transform.position = new Vector3(transform.position.x, -80, 0);
            }
        }
    }

}
