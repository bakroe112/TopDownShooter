using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLayerChange : MonoBehaviour
{
    public GameObject player;
    //��̬��������
    void Start()
    {
        if (player == null)
            player = GameObject.Find("Player");
        InvokeRepeating("LayerChange", 0, 0.01f);
    }
    void LayerChange()
    {
        if (player == null)
            return;
            
        if(player.transform.position.y < transform.position.y)
        {
            GetComponent<SpriteRenderer>().sortingOrder = 0;
        }
        else
        {
            GetComponent<SpriteRenderer>().sortingOrder = 1;
        }
    }
}
