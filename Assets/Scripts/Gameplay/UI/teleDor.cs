using UnityEngine;

public class teleDor : MonoBehaviour
{
    public GameObject canvas;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && canvas != null)
        {
            canvas.SetActive(true);
            Debug.Log("Player entered the teleDor area, canvas enabled.");
        }
    }

}
