using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f; // Speed of the player
    public float jumpHeight = 5f; // Height of the jump
    // You can adjust the speed and jump height as needed
    private float movement; // Variable to store horizontal movement input
    private bool facingRight = true; // To track the player's facing direction
    private bool isGrounded = true; // To check if the player is on the ground
    public Rigidbody2D body; // Reference to the Rigidbody2D component

    void Start()
    {
        facingRight = true;
        isGrounded = true;
    }

    void Update()
    {
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
        }
    }

    void Jump()
    {
        body.AddForce(new Vector2(0f, jumpHeight), ForceMode2D.Impulse);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the player collides with the ground
        if (collision.gameObject.tag == "Ground")
        {
            isGrounded = true; // Set isGrounded to true when touching the ground
        }
    }
}
// menit 47:18