using System.Collections.Generic;
using UnityEngine;

public class Knife : MonoBehaviour
{
    public int damage;
    public float critChance;
    public float critDamage;
    public bool isCrit;
    public List<GameObject> targetEnemy = new List<GameObject>();
    public void Destroy()
    {
        CheckAttack();
        Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (!targetEnemy.Contains(collision.gameObject))
            {
                targetEnemy.Add(collision.gameObject);
            }
        }
        if (collision.gameObject.CompareTag("EnemyBullet"))
        {
            collision.GetComponent<Animator>().SetBool("Hit", true);
            Destroy(collision.GetComponent<BoxCollider2D>());
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (targetEnemy.Contains(collision.gameObject))
            {
                targetEnemy.Remove(collision.gameObject);
            }
        }
    }
    public void CheckAttack()
    {
        for (int i = 0; i < targetEnemy.Count; i++)
        {
            int bulletDamage = (int)(((UnityEngine.Random.Range(0f, 1f) < critChance) ? critDamage : 1) * damage);
            if (bulletDamage == critDamage * damage)
                isCrit = true;
            if (targetEnemy[i] != null)
            {
                targetEnemy[i].GetComponent<EnemyBehaviour>().isCrit = isCrit;
                targetEnemy[i].SendMessage("BeHit", bulletDamage, SendMessageOptions.DontRequireReceiver);
            }
            isCrit = false;
        }
    }
}
