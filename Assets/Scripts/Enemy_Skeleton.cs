using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 3f;
    public float jumpForce = 5f;
    private Rigidbody2D rb;
    public Animator animator;

    private bool facingRight = true;
    private bool isGrounded = true;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float move = 0f;

        // Gerakan horizontal (panah kiri/kanan)
        if (Input.GetKey(KeyCode.RightArrow)) move = 1f;
        else if (Input.GetKey(KeyCode.LeftArrow)) move = -1f;

        rb.velocity = new Vector2(move * speed, rb.velocity.y);

        // Set animasi "Walk"
        animator.SetFloat("Walk", Mathf.Abs(move));

        // Flip arah
        if (move > 0 && !facingRight)
            Flip();
        else if (move < 0 && facingRight)
            Flip();

        // Lompat (panah atas)
        if (Input.GetKeyDown(KeyCode.UpArrow) && isGrounded)
        {
            rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            animator.SetBool("Jump", true);
            isGrounded = false;
        }

        // Serang (tekan P)
        if (Input.GetKeyDown(KeyCode.P))
        {
            animator.SetTrigger("Attack");
        }

        // Terkena damage (tekan O)
        if (Input.GetKeyDown(KeyCode.O))
        {
            animator.SetTrigger("Hurt");
        }

        // Mati (tekan K)
        if (Input.GetKeyDown(KeyCode.K))
        {
            animator.SetTrigger("Die");
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1f;
        transform.localScale = scale;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Menandai bahwa musuh sudah menyentuh tanah
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            animator.SetBool("Jump", false);
        }
    }
}
