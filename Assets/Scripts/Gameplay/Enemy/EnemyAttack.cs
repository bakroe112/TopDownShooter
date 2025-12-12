using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    private Animator enemyAnimator;
    public GameObject fireWarning;
    public GameObject player;
    public Enemy nowEnemy;
    public bool aimAtPlayer;
    public float bulletSpeed, minBulletSpeed, destroyTime;
    public float targetAngle;
    private void Start()
    {
        enemyAnimator = GetComponent<Animator>();
    }
    void Attack()
    {
        float angle = Mathf.Atan((player.transform.position.y - transform.position.y) / (player.transform.position.x - transform.position.x)) * 180 / Mathf.PI;
        if (player.transform.position.x < transform.position.x)
            angle += 180;
        if (fireWarning != null && fireWarning.activeSelf)
            fireWarning.SetActive(false);
        if (nowEnemy.enemyFireMode == Enemy.AttackMode.BlueFlower)
            for (int i = 1; i <= nowEnemy.bulletNum; i++)
            {
                GameObject bullet = Instantiate(nowEnemy.bullet, this.transform.position, Quaternion.Euler(0, 0, 360 * i / nowEnemy.bulletNum + UnityEngine.Random.Range(-180 / nowEnemy.bulletNum, 180 / nowEnemy.bulletNum)));
                bullet.GetComponent<EnemyBullet>().bulletSpeed = bulletSpeed;
                bullet.GetComponent<EnemyBullet>().minBulletSpeed = minBulletSpeed;
                bullet.GetComponent<EnemyBullet>().destroyTime = destroyTime;
            }
        else
        {
            if (nowEnemy.bulletNum > 1)
                for (int i = 0; i < nowEnemy.bulletNum; i++)
                {
                    GameObject bullet;
                    if (aimAtPlayer)
                        bullet = Instantiate(nowEnemy.bullet, transform.position, Quaternion.Euler(0, 0, (nowEnemy.bulletRange * 2f / (nowEnemy.bulletNum - 1) * i) + angle - nowEnemy.bulletRange));
                    else
                        bullet = Instantiate(nowEnemy.bullet, transform.position, Quaternion.Euler(0, 0, 360 * i / nowEnemy.bulletNum + targetAngle));
                    bullet.GetComponent<EnemyBullet>().bulletSpeed = bulletSpeed;
                    bullet.GetComponent<EnemyBullet>().minBulletSpeed = minBulletSpeed;
                    bullet.GetComponent<EnemyBullet>().destroyTime = destroyTime;
                    if (bullet.GetComponent<EnemyBullet>() != null)
                        bullet.GetComponent<EnemyBullet>().damageToken = nowEnemy.bulletDamage;
                }
            else
            {
                GameObject bullet;
                if (aimAtPlayer)
                    bullet = Instantiate(nowEnemy.bullet, transform.position, Quaternion.Euler(0, 0, angle));
                else
                    bullet = Instantiate(nowEnemy.bullet, transform.position, Quaternion.Euler(0, 0, targetAngle));
                bullet.GetComponent<EnemyBullet>().bulletSpeed = bulletSpeed;
                bullet.GetComponent<EnemyBullet>().minBulletSpeed = minBulletSpeed;
                bullet.GetComponent<EnemyBullet>().destroyTime = destroyTime;
                if (bullet.GetComponent<EnemyBullet>() != null)
                    bullet.GetComponent<EnemyBullet>().damageToken = nowEnemy.bulletDamage;
            }
        }
    }
    public void StartAttack()
    {
        if (fireWarning != null)
            fireWarning.SetActive(true);
        //if (enemyAnimator != null && enemyAnimator.runtimeAnimatorController != null)
        //    enemyAnimator.SetTrigger("Attack");
        if (nowEnemy.enemyForeSwingMode == Enemy.foreSwingMode.time)
        {
            Invoke(nameof(Attack), nowEnemy.foreSwingTime);
        }
    }
}
