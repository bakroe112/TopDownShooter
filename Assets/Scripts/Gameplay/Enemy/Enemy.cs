using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName = "Inventory/New Enemy")]
public class Enemy : ScriptableObject
{
    public enum AttackMode
    {
        Normal,
        BlueFlower
    }
    public enum CritMode
    {
        Poison,
        Burn,
        Stun,
        Froze,
        None
    }
    public enum foreSwingMode
    {
        time
    }
    public AttackMode enemyFireMode;
    public CritMode enemyCritMode;
    public foreSwingMode enemyForeSwingMode;
    [TextArea]
    public string enemyName;
    [TextArea]
    public string enemyDescription;
    public Sprite enemyImage;
    public GameObject bullet;
    public int maxHP;
    public int bulletDamage;
    public int scoreValue = 10;
    public float foreSwingTime;
    public float attackGap;
    public float bulletNum;
    public float bulletRange;
}
