using UnityEngine;

public class EnemyPlaceCounter : MonoBehaviour
{
    [SerializeField] private GameObject door;
    [SerializeField] private GameObject teleDoor;
    [SerializeField] private float startDelay = 0.3f;
    [SerializeField] private float checkInterval = 0.2f;

    private void OnEnable()
    {
        // bắt đầu tự canh
        CancelInvoke(nameof(CheckEmpty));
        InvokeRepeating(nameof(CheckEmpty), startDelay, checkInterval);
    }

    private void OnDisable()
    {
        CancelInvoke(nameof(CheckEmpty));
    }

    private void CheckEmpty()
    {
        if (transform.childCount != 0) return;

        door?.SetActive(false);
        teleDoor?.SetActive(true);

        // dừng hẳn script luôn
        CancelInvoke(nameof(CheckEmpty));
        enabled = false;
    }
}
