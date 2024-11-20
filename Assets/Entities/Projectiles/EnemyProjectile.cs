using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;

public class EnemyProjectile : MonoBehaviour
{
    public GameObject player;
    private Rigidbody2D rb;
    public float force;
    public float timer;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");

        //The direction from the projectile to the player's last position
        Vector3 direction = player.transform.position - transform.position;

        //Move the projectile at the specified direction with the speed mentioned earlier
        rb.velocity = new Vector2 (direction.x, direction.y).normalized * force;

        //Rotate the projectile depending on the direction it was fired at
        float rot = Mathf.Atan2(-direction.y, -direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0,0, rot);
    }

    // Update is called once per frame
    void Update()
    {
        //Start counting the time by adding the difference in time between each frame
        timer += Time.deltaTime;

        //Delete the projectile after 10 seconds
        if (timer > 10)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player") && collider.gameObject.name == "Body")
        {
            //Damage the player by 1
            collider.GetComponentInParent<Player>().ChangeHealth(-1);
            //Delete projectile when it touches the player (we can add effects here)
            Destroy(gameObject);
        }
    }
}
