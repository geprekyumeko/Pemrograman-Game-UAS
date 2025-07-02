using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy_Raja_Iblis : MonoBehaviour
{
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

    // Start is called before the first frame update
    void Start()
    {
        facingLeft = true; // Initialize facing direction
        animator = this.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
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

    private void OnDrawGizmosSelected()
    {
        if (detectPoint == null)
        {
            return; // Exit if detectPoint is not assigned
        }
        Gizmos.color = Color.red; // Set the color for the Gizmo
        Gizmos.DrawRay(detectPoint.position, Vector2.down * Distance);
    }
}
