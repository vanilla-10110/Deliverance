using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class ProjectileSpawner : MonoBehaviour
{

    [SerializeField] GameObject projectilePrefab;
    [SerializeField] float projectileSpeed = 5f;
    [SerializeField] float timeBeweetShots = 2f;
    [SerializeField] float destroyProjectileTime = 5;
    
    enum DIRECTION {
        LEFT, RIGHT, UP, DOWN,
    }

    Dictionary<DIRECTION, Vector3> directionDict = new Dictionary<DIRECTION, Vector3>() {
        {DIRECTION.UP, Vector2.up},
        {DIRECTION.DOWN, Vector2.down},
        {DIRECTION.LEFT, Vector2.left},
        {DIRECTION.RIGHT, Vector2.right},
    };
    
    [SerializeField] DIRECTION directionOfProjectile = DIRECTION.UP;

    void Start(){
        StartCoroutine(ShootRoutine());
    }

    IEnumerator ShootRoutine(){
        // wait for time between shots
        yield return new WaitForSeconds(timeBeweetShots);

        ShootProjectile();
    }

    void ShootProjectile(){
        GameObject newProjectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);

        Rigidbody2D rb = newProjectile.GetComponent<Rigidbody2D>();

        rb.velocity = directionDict[directionOfProjectile] * projectileSpeed;

        StartCoroutine(DestroyProjectileRoutine(newProjectile));
        StartCoroutine(ShootRoutine());
    }

    IEnumerator DestroyProjectileRoutine(GameObject projectile){
        yield return new WaitForSeconds(destroyProjectileTime);

        Destroy(projectile);
    }
}
