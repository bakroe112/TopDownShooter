using UnityEngine;

public class PlayerDead : MonoBehaviour
{
    [SerializeField]
    private AudioClip deadSound;
    private AudioSource audioSource;

    [SerializeField]
    private float deadSpeed;
    [SerializeField]
    private float minSpeed;
    [SerializeField]
    private float slowDown;
    [SerializeField]
    private GameObject deadMenu;
    [SerializeField]
    private bool test;

    private Rigidbody2D rb;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    void OnEnable()
    {
        audioSource.clip = deadSound;
        audioSource.Play();

        Destroy(transform.GetChild(0).gameObject);
        Destroy(transform.GetChild(1).gameObject);
        Destroy(transform.GetChild(2).gameObject);
        Destroy(GetComponent<Animator>());
        GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.5f, 0.5f);
        rb = GetComponent<Rigidbody2D>();
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        float angle = Random.Range(0, 2 * Mathf.PI);
        rb.linearVelocity = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * deadSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        rb.linearVelocity *= slowDown;
        if (rb.linearVelocity.magnitude < minSpeed)
        {
            rb.linearVelocity = Vector2.zero;
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
            Invoke(nameof(Destroy), 2);
        }
    }
    void Destroy()
    {
        Destroy(this);
        //deadMenu.SetActive(true);
    }
}
