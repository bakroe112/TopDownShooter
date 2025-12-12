using UnityEngine;
using Unity.Behavior; // có Behavior Agent thì bạn có package này

public class DoorActive : MonoBehaviour
{
    [SerializeField] private GameObject door;
    [SerializeField] private Transform enemyPlace; // kéo EnemyPlace vào đây
    [SerializeField] private string playerTag = "Player";
    [SerializeField] private bool disableThisAfterTriggered = true;

    [SerializeField] private float delay = 0.3f;
    [SerializeField] private bool includeInactive = true;          // vẫn tìm được agent dù đang disabled
    [SerializeField] private bool forceEnableEnemyGameObject = false; // nếu Goblin đang SetActive(false) thì bật lên
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag(playerTag)) return;
        Invoke(nameof(ActivateDoor), delay);
    }

    private void ActivateDoor()
    {
        if (door != null) door.SetActive(true);

        // Bật Behavior Agent cho tất cả Goblin con của EnemyPlace
        if (enemyPlace != null)
        {
            var agents = enemyPlace.GetComponentsInChildren<BehaviorGraphAgent>(includeInactive);
            for (int i = 0; i < agents.Length; i++)
            {
                agents[i].gameObject.SetActive(true);
                agents[i].enabled = true;
            }
        }

        if (disableThisAfterTriggered) gameObject.SetActive(false);
    }
}
