using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class PlayerHPMPShield : MonoBehaviour
{
    [SerializeField]
    private AudioClip hitSound;
    private AudioSource audioSource;

    public int HP;
    public int Shield;
    public int hurtDamage;
    public int constantDamage;
    public int constantMaxTime;
    public Player player;
    public GameObject nowDamageNum;
    public GameObject damageNum;
    public GameObject canvas;
    public GameObject HPWarning;
    public GameObject ShieldWarning;
    public GameObject stateIcon;
    public Slider HPSlider, ShieldSlider;
    public Text HPText, ShieldText;
    public Sprite playerDead;
    public SpriteRenderer playerSpriteRenderer;
    public float changeSpeed;
    public float hurtTime;
    public float damageNumX;
    public float damageNumY;
    public float maxAllowedHurtTime;
    public float maxAllowedCollisionIgnoreTime;
    public float recoverTimeMin;
    public float recoverTimeGap;
    public float constantDamageTimeGap;
    public bool allowHurt;
    private void Start()
    {
        HP = player.fullHP;
        Shield = player.fullShield;
        playerSpriteRenderer = GetComponent<SpriteRenderer>();
        HPSlider.maxValue = HP;
        HPSlider.value = HP;
        ShieldSlider.maxValue = Shield;
        ShieldSlider.value = Shield;
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
        if (HPSlider.value > HP + changeSpeed * Time.deltaTime)
        {
            HPSlider.value -= changeSpeed * Time.deltaTime;
        }
        else if (HPSlider.value < HP - changeSpeed * Time.deltaTime)
        {
            HPSlider.value += changeSpeed * Time.deltaTime;
        }
        else
        {
            HPSlider.value = HP;
        }
        if (ShieldSlider.value > Shield + changeSpeed * Time.deltaTime)
        {
            ShieldSlider.value -= changeSpeed * Time.deltaTime;
        }
        else if (ShieldSlider.value < Shield - changeSpeed * Time.deltaTime)
        {
            ShieldSlider.value += changeSpeed * Time.deltaTime;
        }
        else
        {
            ShieldSlider.value = Shield;
        }
        if (HPText.text != HP.ToString() + "/" + player.fullHP.ToString())
            HPText.text = HP.ToString() + "/" + player.fullHP.ToString();
        if (ShieldText.text != Shield.ToString() + "/" + player.fullShield.ToString())
            ShieldText.text = Shield.ToString() + "/" + player.fullShield.ToString();
        DisplayWarning();
    }
    void BeHit(int damage)
    {
        if (allowHurt)
        {
            allowHurt = false;
            GetHurt();
            Invoke(nameof(Recover), hurtTime);
            hurtDamage += damage;
            CancelInvoke(nameof(ClearDamageNum));
            Invoke(nameof(ClearDamageNum), maxAllowedHurtTime);
            //����Ʈ��
            if (hurtDamage > 0)
            {
                if (nowDamageNum == null)
                    nowDamageNum = Instantiate(damageNum, Camera.main.WorldToScreenPoint(transform.position) + new Vector3(damageNumX, damageNumY, 0), Quaternion.identity, canvas.transform);
                nowDamageNum.GetComponent<Text>().text = hurtDamage.ToString();
            }
            if (Shield > damage)
            {
                Shield -= damage;
            }
            else
            {
                if (HP > damage - Shield)
                    HP -= damage - Shield;
                else
                {
                    HP = 0;
                    Dead();
                }
                Shield = 0;
            }
            CancelInvoke(nameof(ShieldRecover));
            InvokeRepeating(nameof(ShieldRecover), recoverTimeMin, recoverTimeGap);
            Invoke(nameof(AllowHurt), maxAllowedCollisionIgnoreTime);
        }
    }
    void GetHurt()
    {
        audioSource.clip = hitSound;
        audioSource.Play();
        playerSpriteRenderer.color = new Color(1, 0.5f, 0.5f, 1);
        CancelInvoke(nameof(Recover));
    }
    void Recover()
    {
        playerSpriteRenderer.color = new Color(1, 1, 1, 1);
        Physics2D.IgnoreLayerCollision(6, 11, false);
    }
    void ClearDamageNum()
    {
        hurtDamage = 0;
    }
    void Dead()
    {
        GetComponent<SpriteRenderer>().sprite = playerDead;
        GetComponent<PlayerDead>().enabled = true;
        Destroy(GetComponent<PlayerMoveController>());
        Destroy(GetComponent<CapsuleCollider2D>());
        Invoke(nameof(DeadDestroy), 0.5f);
        CancelInvoke(nameof(Recover));
    }
    void ShieldRecover()
    {
        if (Shield < player.fullShield)
            Shield += 1;
        else
            CancelInvoke(nameof(ShieldRecover));
    }
    void DisplayWarning()
    {
        if (HP < 0.5 * player.fullHP)
        {
            HPWarning.SetActive(true);
        }
        else
        {
            HPWarning.SetActive(false);
        }
        if (Shield <= 2)
        {
            ShieldWarning.SetActive(true);
        }
        else
        {
            ShieldWarning.SetActive(false);
        }
    }
    void DeadDestroy()
    {
        Destroy(this);
        Destroy(nowDamageNum);
    }
    void AllowHurt()
    {
        allowHurt = true;
    }
    void UpdateDamage(int data)
    {
        constantDamage = data;
    }
    void UpdateTimeGap(float data)
    {
        constantDamageTimeGap = data;
    }
    void UpdateTimes(int data)
    {
        constantMaxTime = data;
    }
    //void GetPoisoned()
    //{
    //    for (int i = 0; i < constantMaxTime; i++)
    //    {
    //        if (i == constantMaxTime - 1)
    //            Invoke(nameof(HideIcon), (i + 1) * constantDamageTimeGap);
    //        Invoke(nameof(HurtByPoison), (i + 1) * constantDamageTimeGap);
    //    }
    //}
    //void HurtByPoison()
    //{
    //    BeHitByChemical(constantDamage);
    //}
    //void GetBurned()
    //{
    //    for (int i = 0; i < constantMaxTime; i++)
    //    {
    //        if (i == constantMaxTime - 1)
    //            Invoke(nameof(HideIcon), (i + 1) * constantDamageTimeGap);
    //        Invoke(nameof(HurtByBurned), (i + 1) * constantDamageTimeGap);
    //    }
    //}
    //void HurtByBurned()
    //{
    //    BeHitByChemical(constantDamage);
    //}
    //void ShowIcon(EnemyBullet.CritType type)
    //{
    //    stateIcon.SetActive(true);
    //    switch (type)
    //    {
    //        case EnemyBullet.CritType.poison:
    //            stateIcon.GetComponent<SpriteRenderer>().sprite = poison;
    //            break;
    //        case EnemyBullet.CritType.burn:
    //            stateIcon.GetComponent<SpriteRenderer>().sprite = burn;
    //            break;
    //    }
    //}
    void HideIcon()
    {
        stateIcon.SetActive(false);
    }
}
