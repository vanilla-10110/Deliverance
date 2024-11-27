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

        var speedMod = (distanceFromPlayer < personalSpace - 0.1 && distanceFromPlayer < lineOfSight ? -2f :
            distanceFromPlayer > personalSpace + 0.25 && distanceFromPlayer < lineOfSight ? 1f :
            0f);
        var durationMod = (distanceFromPlayer < personalSpace - 0.1 && distanceFromPlayer < lineOfSight ? .5f : 1f);

        currentSpeed = Mathf.SmoothDamp(currentSpeed, speed * speedMod, ref speedVelocity, smoothDuration * durationMod);

        transform.position = Vector2.MoveTowards(transform.position, player.position, currentSpeed * Time.deltaTime);

        transform.localScale = new Vector2((transform.InverseTransformPoint(player.transform.position).x < 0 ? transform.localScale.x : -transform.localScale.x), transform.localScale.y);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, lineOfSight);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, personalSpace);
    }
}
