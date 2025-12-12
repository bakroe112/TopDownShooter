using System.Collections.Generic;
using UnityEngine;

public class WeaponChargeTip : MonoBehaviour
{
    public GameObject player;
    public GameObject fatherTips;
    public float deltaX, deltaY;

    void Update()
    {
        // Chỉ cho cụm UI chạy theo vị trí player
        if (player != null && fatherTips != null)
        {
            fatherTips.transform.position =
                Camera.main.WorldToScreenPoint(player.transform.position)
                + new Vector3(deltaX, deltaY, 0);
        }
    }
}
