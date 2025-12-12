using UnityEngine;

public class HandKnife : MonoBehaviour
{
    public Fire playerFireInfo;
    public bool allowHandKnife, enemyNear;
    public float handKnifeTimeGap, deltaX, deltaY, enemyCheckDistance;
    public GameObject handKnife, player, playerHandKnife;
    void Start()
    {
        playerFireInfo = GetComponent<Fire>();
        allowHandKnife = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerFireInfo.MP < playerFireInfo.weaponHeld.MpNeed * playerFireInfo.weaponHeld.shootTimes)
        {
            RaycastHit2D enemyCheck = Physics2D.Raycast(transform.parent.parent.position, ((transform.parent.parent.localScale.x > 0) ? Vector2.right : Vector2.left), enemyCheckDistance, 1 << 8);
            if (enemyCheck.collider != null)
                enemyNear = true;
            else
                enemyNear = false;
            if (Input.GetMouseButtonDown(0) && (allowHandKnife || enemyNear))
            {
                allowHandKnife = false;
                CancelInvoke(nameof(Refresh));
                Invoke(nameof(Refresh), handKnifeTimeGap);
                if (player.transform.localScale.x > 0)
                {
                    playerHandKnife = Instantiate(handKnife, transform.position + new Vector3(deltaX, deltaY, 0), Quaternion.identity);
                }
                else
                {
                    playerHandKnife = Instantiate(handKnife, transform.position + new Vector3(-deltaX, deltaY, 0), Quaternion.identity);
                    playerHandKnife.transform.localScale = new Vector3(-playerHandKnife.transform.localScale.x, playerHandKnife.transform.localScale.y, playerHandKnife.transform.localScale.z);
                }
            }
        }
    }
    void Refresh()
    {
        allowHandKnife = true;
    }
}
