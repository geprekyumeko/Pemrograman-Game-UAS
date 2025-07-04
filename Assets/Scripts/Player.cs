using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI; // Importing Unity UI namespace for Text component
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [Header("Player Health")]
    public int maxHealth = 5; // Maximum health of the player
    public GameObject explosionEffectPrefab; // Prefab for explosion effect
    public Transform explosionPosition;
    public Text healthText; // Reference to the UI Text component to display health
    private int currentCoin;
    public Text coinText; // Reference to the UI Text component to display coins
    public GameObject gameOverUI; // Reference to the Game Over UI panel

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
        currentCoin = 0; // Initialize current coin count
    }

    void Update()
    {
        if (maxHealth <= 0)
        {
            Die(); // Call the Die method if health is zero or less
        }

        healthText.text = maxHealth.ToString(); // Update the health text UI
        coinText.text = currentCoin.ToString(); // Update the coin text UI

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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Coin")
        {
            currentCoin++;
            collision.gameObject.transform.GetChild(0).GetComponent<Animator>().SetTrigger("Collected"); // Trigger coin collection animation
            Destroy(collision.gameObject, 1f); // Destroy the coin after 1 second
        }
        if (collision.gameObject.tag == "Checkpoint")
        {
            SceneManager.LoadScene("Victory"); // Load the Victory scene when reaching a checkpoint
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
        gameOverUI.SetActive(true); // Show the Game Over UI panel
        FindAnyObjectByType<GameManager>().isGameActive = false; // Set the game to inactive
        GameObject tempExplosionEffectPrefab = Instantiate(explosionEffectPrefab, explosionPosition.position, quaternion.identity); // Instantiate explosion effect at the enemy position
        Destroy(tempExplosionEffectPrefab, .8f);
        Destroy(this.gameObject);
    }
}
