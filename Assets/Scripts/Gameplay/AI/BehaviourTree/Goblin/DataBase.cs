using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Behavior;  

public class DataBase : MonoBehaviour
{
    public bool isPlayerDead;
    public bool isBulletAhead;
    public float playerDistance;
    public GameObject rayTarget;

    public GameObject player;
    public float maxBulletCheckDistance;
    public float bulletCheckDeltaDistance;
    public float bulletCheckFrontDistance;

    private BehaviorGraphAgent goblinAgent;

    private void Awake()
    {
        // nếu quên kéo trong Inspector thì tự lấy trên cùng GameObject
        if (goblinAgent == null)
            goblinAgent = GetComponent<BehaviorGraphAgent>();
    }

    private void Start()
    {
        if (player == null)
            player = GameObject.Find("Player");

        // gán player vào blackboard của Behavior Graph
        if (goblinAgent != null)
        {
            goblinAgent.SetVariableValue("player", player);
        }
    }

    private void Update()
    {
        if (player == null || goblinAgent == null)
            return;

        Vector3 forward, left, right;
        forward = (player.transform.position - transform.position).normalized;
        left = new Vector3(-forward.y, forward.x).normalized;
        right = -left;

        // check player sống/chết
        var hp = player.GetComponent<PlayerHPMPShield>();
        if (hp != null && hp.HP > 0)
            isPlayerDead = false;
        else
            isPlayerDead = true;

        // check bullet ở 2 bên
        RaycastHit2D rayForBulletLeft, rayForBulletRight;
        rayForBulletLeft = Physics2D.Raycast(
            transform.position + bulletCheckDeltaDistance * left + bulletCheckFrontDistance * forward,
            left,
            maxBulletCheckDistance,
            1 << 7
        );
        Debug.DrawRay(
            transform.position + bulletCheckDeltaDistance * left + bulletCheckFrontDistance * forward,
            left * maxBulletCheckDistance
        );

        rayForBulletRight = Physics2D.Raycast(
            transform.position + bulletCheckDeltaDistance * right + bulletCheckFrontDistance * forward,
            right,
            maxBulletCheckDistance,
            1 << 7
        );
        Debug.DrawRay(
            transform.position + bulletCheckDeltaDistance * right + bulletCheckFrontDistance * forward,
            right * maxBulletCheckDistance
        );

        if ((rayForBulletLeft.collider != null && rayForBulletLeft.collider.CompareTag("Bullet")) ||
            (rayForBulletRight.collider != null && rayForBulletRight.collider.CompareTag("Bullet")))
        {
            isBulletAhead = true;
        }
        else
        {
            isBulletAhead = false;
        }

        // khoảng cách tới player
        playerDistance = (player.transform.position - transform.position).magnitude;

        // raycast tới player để lấy rayTarget
        RaycastHit2D rayForPlayer;
        rayForPlayer = Physics2D.Raycast(
            transform.position,
            forward,
            int.MaxValue,
            ~(1 << 8 | 1 << 2 | 1 << 9)
        );
        if (rayForPlayer.collider != null)
        {
            rayTarget = rayForPlayer.collider.gameObject;
        }
        else
        {
            rayTarget = null;
        }

        // cập nhật blackboard của Behavior Graph
        goblinAgent.SetVariableValue("isPlayerDead", isPlayerDead);
        goblinAgent.SetVariableValue("isBulletAhead", isBulletAhead);
        goblinAgent.SetVariableValue("playerDistance", playerDistance);
        goblinAgent.SetVariableValue("rayTarget", rayTarget);
    }
}
