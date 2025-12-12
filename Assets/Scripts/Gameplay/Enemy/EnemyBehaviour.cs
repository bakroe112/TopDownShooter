using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class EnemyBehaviour : MonoBehaviour
{
    [SerializeField]
    private AudioClip hitSound;
    private AudioSource audioSource;

    public GameObject damageNum;
    public GameObject critText;
    public GameObject nowDamageNum;
    public GameObject nowCritText;
    public GameObject bullet;
    public GameObject canvas;
    public Animator enemyAnimator;
    public Sprite deadImage;
    public SpriteRenderer enemySpriteRenderer;
    public Enemy enemy;
    public bool isCrit;
    public int hurtDamage;
    public int bulletNum;
    public int HP;
    public float deadSpeed;
    private bool hasAddedScore = false;
    public float hurtTime;
    public float critTime;
    public float maxAllowedHurtTime;
    public float damageNumX;
    public float damageNumY;
    public float critTextX;
    public float critTextY;

    void Start()
    {
        enemySpriteRenderer = GetComponent<SpriteRenderer>();
        enemyAnimator = GetComponent<Animator>();
        canvas = GameObject.Find("Canvas");
        HP = enemy.maxHP;

        audioSource = GetComponent<AudioSource>();
    }
    private void Update()
    {
        if (nowDamageNum != null)
        {
            if (hurtDamage <= 0)
                Destroy(nowDamageNum);
            else if (hurtDamage > 0)
                nowDamageNum.transform.position = Camera.main.WorldToScreenPoint(transform.position) + new Vector3(damageNumX, damageNumY, 0);
        }
        if (nowCritText != null)
            nowCritText.transform.position = Camera.main.WorldToScreenPoint(transform.position) + new Vector3(critTextX, critTextY, 0);
        if (HP <= 0)
        {
                if (!hasAddedScore)
                {
                    GameObject scoreManagerObj = GameObject.Find("ScoreManager");
                    if (scoreManagerObj != null)
                    {
                        scoreManagerObj.SendMessage("AddScore", enemy.scoreValue, SendMessageOptions.DontRequireReceiver);
                    }
                    hasAddedScore = true;
                }
  
            var behaviorAgent = GetComponent<Unity.Behavior.BehaviorGraphAgent>();
            if (behaviorAgent != null)
            {
                behaviorAgent.enabled = false;
            }
            GetComponent<SpriteRenderer>().sprite = deadImage;
            Destroy(GetComponent<Animator>());
            Destroy(GetComponent<BoxCollider2D>());
            if (this.GetComponent<EnemyDeadMove>() != null)
                this.GetComponent<EnemyDeadMove>().enabled = true;
            if (transform.childCount != 0)
                Destroy(transform.GetChild(0).gameObject);
            Invoke(nameof(DestroyThis), 0.5f);
        }
    }
    void BeHit(int damage)
    {
        GetHurt();
        HP -= damage;
        Invoke(nameof(Recover), hurtTime);
        hurtDamage += damage;
        CancelInvoke(nameof(ClearDamageNum));
        Invoke(nameof(ClearDamageNum), maxAllowedHurtTime);
        //受伤飘字
        if (hurtDamage > 0)
        {
            if (nowDamageNum == null)
                nowDamageNum = Instantiate(damageNum, Camera.main.WorldToScreenPoint(transform.position) + new Vector3(damageNumX, damageNumY, 0), Quaternion.identity, canvas.transform);
            nowDamageNum.GetComponent<Text>().text = hurtDamage.ToString();
            if (isCrit)
            {
                nowDamageNum.GetComponent<Text>().color = new Color(1, 0, 0, 1);
                if (nowCritText == null)
                    nowCritText = Instantiate(critText, Camera.main.WorldToScreenPoint(transform.position) + new Vector3(critTextX, critTextY, 0), Quaternion.identity, canvas.transform);
                CancelInvoke(nameof(ClearCritInfo));
                Invoke(nameof(ClearCritInfo), critTime);
            }
        }
    }
    void GetHurt()
    {
        audioSource.clip = hitSound;
        audioSource.Play();
        enemySpriteRenderer.color = new Color(1, 0.5f, 0.5f, 1);
        CancelInvoke(nameof(Recover));
    }
    void Recover()
    {
        enemySpriteRenderer.color = new Color(1, 1, 1, 1);
    }
    void ClearDamageNum()
    {
        hurtDamage = 0;
    }
    void ClearCritInfo()
    {
        if (nowCritText != null)
            Destroy(nowCritText);
        if (nowDamageNum != null)
            nowDamageNum.GetComponent<Text>().color = new Color(1, 1, 1, 1);
    }
    void DestroyThis()
    {
        if (nowCritText != null)
            Destroy(nowCritText);
        if (nowDamageNum != null)
            Destroy(nowDamageNum);
        Destroy(GetComponent<EnemyAttack>());
        Destroy(gameObject);
    }
}
