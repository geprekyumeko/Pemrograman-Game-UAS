using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy_Skeleton : MonoBehaviour
{
    [Header("Health")]
    public int maxHealth = 3; // Maximum health of the skeleton
    public GameObject explosionEffectPrefab; // Prefab for explosion effect


    public Transform player;
    public float patrolSpeed = 2.5f; // Speed at which the skeleton patrols
    public float attackRange = 5f; // Range within which the skeleton can attack
    public float chaseSpeed = 3f; // Speed at which the skeleton chases the player
    public float retrieveDistance = 2.5f;

    private bool facingLeft;
    public Transform detectPoint; // Point to detect obstacles
    public float Distance = 0.4f; // Distance to check for obstacles
    public LayerMask whatIsGround; // Layer mask to identify ground and obstacles

    private Animator animator;

    [Header("Attack")]
    public Transform attackPosition; // Position where the attack will be performed
    public float attackRadius = 1f; // Radius of the attack area
    public LayerMask attackLayer;
    // Start is called before the first frame update
    void Start()
    {
        facingLeft = true; // Initialize facing direction
        animator = this.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (maxHealth <= 0)
        {   
            Die();
        }

        if (Vector2.Distance(transform.position, player.position) <= attackRange)
            {
                if (transform.position.x < player.position.x && facingLeft == true)
                {
                    transform.eulerAngles = new Vector3(0f, -180f, 0f);
                    facingLeft = false;
                }
                else if (player.position.x < transform.position.x)
                {
                    transform.eulerAngles = new Vector3(0f, 0f, 0f);
                    facingLeft = true;
                }
                if (Vector2.Distance(transform.position, player.position) > retrieveDistance)
                {
                    animator.SetBool("Attack", false);
                    transform.position = Vector2.MoveTowards(transform.position, player.position, chaseSpeed * Time.deltaTime);
                }
                else
                {
                    animator.SetBool("Attack", true);
                }
            }
            else
            {
                transform.Translate(Vector2.left * Time.deltaTime * patrolSpeed);

                RaycastHit2D hit = Physics2D.Raycast(detectPoint.position, Vector2.down, Distance, whatIsGround);
                if (hit == false)
                {
                    if (facingLeft == true)
                    {
                        transform.eulerAngles = new Vector3(0f, -180f, 0f); // Flip the skeleton to face right
                        facingLeft = false; // Update facing direction
                    }
                    else
                    {
                        transform.eulerAngles = new Vector3(0f, -0f, 0f); // Flip the skeleton to face right
                        facingLeft = true; // Update facing direction
                    }
                }
            }
    }

    public void Attack()
    {
        Collider2D callInfo = Physics2D.OverlapCircle(attackPosition.position, attackRadius, attackLayer);
        if (callInfo)
        {
            if (callInfo.gameObject.GetComponent<Player>() != null)
            {
                callInfo.gameObject.GetComponent<Player>().TakeDamage(1);
            }
        }
    }

    public void TakeDamage(int damage)
    {
        if (maxHealth <= 0)
        {
            Debug.Log("Skeleton is already dead.");
            return; // Exit if the skeleton is already dead
        }
        maxHealth -= damage;
    }

    private void OnDrawGizmosSelected()
    {
        if (detectPoint == null)
        {
            return; // Exit if detectPoint is not assigned
        }
        Gizmos.color = Color.yellow; // Set the color for the Gizmo
        Gizmos.DrawRay(detectPoint.position, Vector2.down * Distance);

        if (attackPosition == null)
        {
            return; // Exit if attackPosition is not assigned
        }
        Gizmos.color = Color.red; // Set the color for the attack radius
        Gizmos.DrawWireSphere(attackPosition.position, attackRadius); // Draw the attack radius
    }

    void Die()
    {
        Debug.Log("Skeleton has died.");
        GameObject tempExplosionEffectPrefab = Instantiate(explosionEffectPrefab, transform.position, quaternion.identity); // Instantiate explosion effect at the enemy position
        Destroy(tempExplosionEffectPrefab, .8f);
        Destroy(this.gameObject); // Destroy the Enemy game object
    }
}
