using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Player Health")]
    public int maxHealth = 5; // Maximum health of the player

    public float speed = 5f; // Speed of the player
    public float jumpHeight = 5f; // Height of the jump
                                  // You can adjust the speed and jump height as needed
    public Animator animator; // Reference to the Animator component
    private float movement; // Variable to store horizontal movement input
    private bool facingRight = true; // To track the player's facing direction
    private bool isGrounded = true; // To check if the player is on the ground
    public Rigidbody2D body; // Reference to the Rigidbody2D component

    [Header("Attack")]
    public Transform attackPosition; // Position where the attack will be performed
    public float attackRadius = 1f; // Radius of the attack area
    public LayerMask attackLayer; // Layer mask to identify attackable objects
    void Start()
    {
        facingRight = true;
        isGrounded = true;
    }

    void Update()
    {
        if (maxHealth <= 0)
        {
            Die(); // Call the Die method if health is zero or less
        }

        movement = Input.GetAxis("Horizontal");
        transform.position += new Vector3(movement, 0f, 0f) * speed * Time.deltaTime;

        // Kontrol arah player (kanan atau kiri)
        if (movement < 0f && facingRight == true)
        {
            transform.eulerAngles = new Vector3(0f, 180f, 0f);
            facingRight = false;
        }
        else if (movement > 0f && facingRight == false)
        {
            transform.eulerAngles = new Vector3(0f, 0f, 0f);
            facingRight = true;
        }

        // Kontrol lompat
        if (Input.GetKeyDown(KeyCode.W) && isGrounded == true)
        {
            Jump();
            isGrounded = false; // Set isGrounded to false when jumping
            animator.SetBool("Jump", true); // Set jump animation
        }

        // Set animator parameters walk
        if (Mathf.Abs(movement) > .1f)
        {
            animator.SetFloat("Walk", 1f);
        }
        else if (movement < .1f)
        {
            animator.SetFloat("Walk", 0f);
        }

        // Set animator parameters attack
        if (Input.GetKeyDown(KeyCode.Space))
        {
            animator.SetTrigger("Attack");
        }
    }

    // Method to handle jumping
    // This method applies an upward force to the player's Rigidbody2D component
    void Jump()
    {
        body.AddForce(new Vector2(0f, jumpHeight), ForceMode2D.Impulse);
    }

    // Versi yang sudah diperbaiki, tapi tetap tidak ideal
    public void Attack()
    {
        Collider2D hitInfo = Physics2D.OverlapCircle(attackPosition.position, attackRadius, attackLayer);
        if (hitInfo != null)
        {
            // Cek Skeleton
            Enemy_Skeleton enemy = hitInfo.gameObject.GetComponent<Enemy_Skeleton>();
            if (enemy != null)
            {
                enemy.TakeDamage(1);
                Debug.Log(hitInfo.gameObject.name + " has been attacked!");
                return; // Keluar dari fungsi setelah musuh ditemukan
            }

            // Cek Golden Skeleton
            Enemy_Skeleton_Gold goldEnemy = hitInfo.gameObject.GetComponent<Enemy_Skeleton_Gold>();
            if (goldEnemy != null)
            {
                goldEnemy.TakeDamage(1);
                Debug.Log(hitInfo.gameObject.name + " has been attacked!");
                return;
            }

            // Cek Raja Iblis
            Enemy_Raja_Iblis rajaIblis = hitInfo.gameObject.GetComponent<Enemy_Raja_Iblis>();
            if (rajaIblis != null)
            {
                rajaIblis.TakeDamage(1);
                Debug.Log(hitInfo.gameObject.name + " has been attacked!");
                return;
            }
        }
    }

    public void TakeDamage(int amount)
    {
        if (maxHealth <= 0)
        {
            Debug.Log("Player is already dead.");
            return; // Exit if the player is already dead
        }
        maxHealth -= amount; // Reduce the player's health by the specified amount
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the player collides with the ground
        if (collision.gameObject.tag == "Ground")
        {
            isGrounded = true; // Set isGrounded to true when touching the ground
            animator.SetBool("Jump", false); // Reset jump animation
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPosition == null)
        {
            return; // Exit if attackPosition is not assigned
        }
        Gizmos.color = Color.red; // Set the color for the attack radius
        Gizmos.DrawWireSphere(attackPosition.position, attackRadius); // Draw the attack radius
    }

    void Die()
    {
        Debug.Log(this.gameObject.name + " Bowo has died.");
    }
}
