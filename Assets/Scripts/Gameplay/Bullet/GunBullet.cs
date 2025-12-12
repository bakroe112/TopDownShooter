using UnityEngine;

public class GunBullet : MonoBehaviour
{
    public float bulletSpeed;
    public int damageToken;
    public bool isCrit;
    public bool isDisappear;
    private void Start()
    {
        GetComponent<Rigidbody2D>().linearVelocity = bulletSpeed * transform.right;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log("Bullet hit: " + collision.gameObject.name);
        if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<EnemyBehaviour>().isCrit = isCrit;
            collision.gameObject.SendMessage("BeHit", damageToken, SendMessageOptions.DontRequireReceiver);
            if (!isDisappear)
                this.transform.SetParent(collision.transform);
        }
        GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
        if (isDisappear)
        {
            GetComponent<Animator>().SetBool("Hit", true);
            Destroy(GetComponent<BoxCollider2D>());
        }
    }
    public void Destroy()
    {
        Destroy(gameObject);
    }
}
