using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingFollower : MonoBehaviour
{
    public float speed;
    public float lineOfSight;
    public float personalSpace;
    public float smoothDuration = 0.3f;

    private float currentSpeed = 0f;
    private float speedVelocity;

    private Transform player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        float distanceFromPlayer = Vector2.Distance(player.position, transform.position);

        if (distanceFromPlayer < lineOfSight)
        {
            if (distanceFromPlayer > personalSpace + 0.25)
            {
                currentSpeed = Mathf.SmoothDamp(currentSpeed, speed, ref speedVelocity, smoothDuration);
            }
            else if (distanceFromPlayer < personalSpace - 0.1)
            {
                currentSpeed = Mathf.SmoothDamp(currentSpeed, -speed * (float)2, ref speedVelocity, smoothDuration * (float)0.5);
            }
            else
            {
                currentSpeed = Mathf.SmoothDamp(currentSpeed, 0f, ref speedVelocity, smoothDuration);
            }

            transform.position = Vector2.MoveTowards(transform.position, player.position, currentSpeed * Time.deltaTime);
        }
        else
        {
            currentSpeed = Mathf.SmoothDamp(currentSpeed, 0f, ref speedVelocity, smoothDuration);
            //transform.position = Vector2.MoveTowards(transform.position, player.position, currentSpeed * Time.deltaTime);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, lineOfSight);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, personalSpace);
    }
}
