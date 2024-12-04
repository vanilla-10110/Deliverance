using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooter : MonoBehaviour
{
    public GameObject projectile;
    public Transform projectilePos;

    public float attackRange;
    public float attackSpeed;

    private float timer;
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {

        float distance = Vector2.Distance(transform.position, player.transform.position);


        if (distance < attackRange)
        {
            timer += Time.deltaTime;

            if (timer > attackSpeed)
            {
                timer = 0;
                shoot();
            }
        }
    }

    void shoot()
    {
        SoundManager.Instance.PlaySoundFX(gameObject.GetComponent<BaseEnemy>().mainAttackSound);
        Instantiate(projectile, projectilePos.position, Quaternion.identity);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
