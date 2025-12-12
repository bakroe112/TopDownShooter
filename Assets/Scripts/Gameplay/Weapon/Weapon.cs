using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Inventory/New Weapon")]
public class Weapon : ScriptableObject
{
    public enum WeaponType
    {
        gun,
        knife,
        others
    }
    public enum WeaponMPmode
    {
        single
    }

    public WeaponType type;
    public WeaponMPmode MpMode;
    public bool weightedRandomScheduling;

    public GameObject bullet;

    public GameObject knife;
    public Sprite weaponImage;
    public RuntimeAnimatorController weaponAnimator;
    [TextArea]
    public string weaponName;
    [TextArea]
    public string weaponDescription;
    public float critChance;
    public float critDamage;
    public float damage;
    public float scatteringAngle;
    public float shootingGap;
    public int shootTimes;
    public float continuousShootingGap;
    public int bulletCount;
    public int MpNeed;

    public float minCritChance;
    public float minDamage;
    public float minScatteringAngle;
    public float fullChargeTime;
    public float minBulletSpeed;
    public float maxBulletSpeed;
    public float knifeDeltaX;
    public float comboDelta;
    public float knifeSpeed;
    public Vector3 knifeScale;
}
