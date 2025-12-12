using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public enum CritType
    {
        poison,
        burn,
        freeze,
        slowDown,
        None
    }
    public CritType bulletCritType;
    public Rigidbody2D rb;
    public GameObject bullet;
    public GameObject player;
    public Vector3 up;
    public Vector3 right;
    public float bulletSpeed;
    public float minBulletSpeed;
    public float targetTimeGap;
    public float destroyTime;
    public float splitTime;
    public float splitAngle;
    public float instantiateTimeGap;
    public float spinSpeed;
    public float selfDestroyTime;
    public float dirChangeSpeed;
    private float sinCosSpeed;
    private float sinCosChangeSpeed;
    private float sinCosAngle;
    private float stopTime;
    private float sonBulletSpeed;
    private float sonBulletMinSpeed;
    private float sonBulletDestroyTime;
    private float constantDamageTimeGap;
    public int damageToken;
    public int splitNum;
    public int sonBulletDamage;
    public int constantTimes;
    public int constantDamage;
    public bool isSon;
    public bool isCrit;
    public bool isSpin;
    public bool selfDestroy;
    public bool trackPlayer;
    public bool autoStop;
    public enum enemyBulletType
    {
        normal,
        slowdown,
        split,
        constantInstantiate,
        constantInstantiateLeftAndRight,
        graphicSinCos
    }
    public enemyBulletType thisEnemyBulletType;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (!isSon)
            rb.linearVelocity = bulletSpeed * transform.right;
        if (thisEnemyBulletType == enemyBulletType.slowdown)
            InvokeRepeating(nameof(SlowDown), targetTimeGap, targetTimeGap);
        else if (thisEnemyBulletType == enemyBulletType.split)
            Invoke(nameof(Split), splitTime);
        else if (thisEnemyBulletType == enemyBulletType.constantInstantiate)
            InvokeRepeating(nameof(InstantiateOnce), 0, instantiateTimeGap);
        else if (thisEnemyBulletType == enemyBulletType.constantInstantiateLeftAndRight)
            InvokeRepeating(nameof(InstantiateOnceLeftAndRight), 0, instantiateTimeGap);
        if (selfDestroy)
            Invoke(nameof(Destroy), selfDestroyTime);
        if (trackPlayer)
            player = GameObject.Find("Player");
        if (autoStop)
            Invoke(nameof(FreezePosition), stopTime);
        up = transform.up;
        right = transform.right;
    }
    private void Update()
    {
        if (isSpin)
            transform.eulerAngles -= new Vector3(0, 0, spinSpeed * Time.deltaTime);
        if (trackPlayer && Time.frameCount % 10 == 0 && Time.frameCount > 0.5 / Time.deltaTime)
        {
            float angle = Mathf.Atan((player.transform.position.y - transform.position.y) / (player.transform.position.x - transform.position.x)) * 180 / Mathf.PI;
            if (player.transform.position.x < transform.position.x)
                angle += 180;
            if (angle < 0)
                angle += 360;
            float deltaUp, deltaDown;
            deltaUp = (transform.eulerAngles.z < angle) ? (angle - transform.eulerAngles.z) : (angle + (360 - transform.eulerAngles.z));//��ʱ��
            deltaDown = 360 - deltaUp;//˳ʱ��
            if (deltaUp < dirChangeSpeed * 10 * Time.deltaTime || deltaDown < dirChangeSpeed * 10 * Time.deltaTime)
            {
                transform.eulerAngles = new Vector3(0, 0, angle);
            }
            else if (deltaUp < deltaDown)
            {
                transform.eulerAngles += new Vector3(0, 0, dirChangeSpeed * 10 * Time.deltaTime);
            }
            else
            {
                transform.eulerAngles -= new Vector3(0, 0, dirChangeSpeed * 10 * Time.deltaTime);
            }
            rb.linearVelocity = bulletSpeed * transform.right;
        }
        if(thisEnemyBulletType == enemyBulletType.graphicSinCos)
        {
            sinCosAngle += Time.deltaTime * sinCosChangeSpeed;
            rb.linearVelocity = (bulletSpeed * right + Mathf.Cos(sinCosAngle) * up) * sinCosSpeed;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(rb != null)
            rb.linearVelocity = Vector2.zero;
        else
        {
            rb = this.gameObject.AddComponent<Rigidbody2D>();
            rb.linearVelocity = Vector2.zero;
        }
        GetComponent<Animator>().SetBool("Hit", true);
        if (GetComponent<BoxCollider2D>() != null)
            Destroy(GetComponent<BoxCollider2D>());
        if (GetComponent<CircleCollider2D>() != null)
            Destroy(GetComponent<CircleCollider2D>());
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.SendMessage("BeHit", damageToken, SendMessageOptions.DontRequireReceiver);
            switch(bulletCritType)
            {
                case CritType.poison: 
                    collision.gameObject.SendMessage("UpdateDamage", constantDamage, SendMessageOptions.DontRequireReceiver);
                    collision.gameObject.SendMessage("UpdateTimes", constantTimes, SendMessageOptions.DontRequireReceiver);
                    collision.gameObject.SendMessage("UpdateTimeGap", constantDamageTimeGap, SendMessageOptions.DontRequireReceiver);
                    collision.gameObject.SendMessage("ShowIcon", bulletCritType, SendMessageOptions.DontRequireReceiver);
                    collision.gameObject.SendMessage("GetPoisoned", SendMessageOptions.DontRequireReceiver);
                    break;
                case CritType.burn:
                    collision.gameObject.SendMessage("UpdateDamage", constantDamage, SendMessageOptions.DontRequireReceiver);
                    collision.gameObject.SendMessage("UpdateTimes", constantTimes, SendMessageOptions.DontRequireReceiver);
                    collision.gameObject.SendMessage("UpdateTimeGap", constantDamageTimeGap, SendMessageOptions.DontRequireReceiver);
                    collision.gameObject.SendMessage("ShowIcon", bulletCritType, SendMessageOptions.DontRequireReceiver);
                    collision.gameObject.SendMessage("GetBurned", SendMessageOptions.DontRequireReceiver);
                    break;
            }
        }
    }
    public void Destroy()
    {
        Destroy(gameObject);
    }
    private void Disappear()
    {
        GetComponent<Animator>().SetBool("Hit", true);
    }
    private void SlowDown()
    {
        if (rb.linearVelocity.magnitude > minBulletSpeed)
            rb.linearVelocity *= 0.95f;
        else
            Invoke(nameof(Disappear), destroyTime);
    }
    private void Split()
    {
        for (int i = 0; i < splitNum; i++)
        {
            GameObject splitBullet = Instantiate(bullet, transform.position, Quaternion.Euler(0, 0, (splitAngle * 2f / (splitNum - 1) * i) - splitAngle + transform.eulerAngles.z));
            splitBullet.GetComponent<EnemyBullet>().damageToken = sonBulletDamage;
            splitBullet.GetComponent<EnemyBullet>().bulletSpeed = sonBulletSpeed;
        }
        Destroy(this.gameObject);
    }
    private void InstantiateOnce()
    {
        GameObject splitBullet = Instantiate(bullet, transform.position, Quaternion.Euler(0, 0, transform.eulerAngles.z));
        splitBullet.GetComponent<EnemyBullet>().damageToken = sonBulletDamage;
        splitBullet.GetComponent<EnemyBullet>().bulletSpeed = sonBulletSpeed;
        splitBullet.GetComponent<EnemyBullet>().selfDestroy = true;
        splitBullet.GetComponent<EnemyBullet>().selfDestroyTime = 3;
    }
    private void InstantiateOnceLeftAndRight()
    {
        GameObject splitBulletLeft = Instantiate(bullet, transform.position, Quaternion.Euler(0, 0, transform.eulerAngles.z + 90));
        splitBulletLeft.GetComponent<EnemyBullet>().damageToken = sonBulletDamage;
        splitBulletLeft.GetComponent<EnemyBullet>().bulletSpeed = sonBulletSpeed;
        splitBulletLeft.GetComponent<EnemyBullet>().minBulletSpeed = sonBulletMinSpeed;
        splitBulletLeft.GetComponent<EnemyBullet>().destroyTime = sonBulletDestroyTime;
        GameObject splitBulletRight = Instantiate(bullet, transform.position, Quaternion.Euler(0, 0, transform.eulerAngles.z - 90));
        splitBulletRight.GetComponent<EnemyBullet>().damageToken = sonBulletDamage;
        splitBulletRight.GetComponent<EnemyBullet>().bulletSpeed = sonBulletSpeed;
        splitBulletRight.GetComponent<EnemyBullet>().minBulletSpeed = sonBulletMinSpeed;
        splitBulletRight.GetComponent<EnemyBullet>().destroyTime = sonBulletDestroyTime;
    }
    private void FreezePosition()
    {
        GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
    }
}
