using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Skeleton : MonoBehaviour
{
    public Transform player;
    public float patrolSpeed = 1.5f; // Speed at which the skeleton patrols
    public float attackRange = 5f; // Range within which the skeleton can attack

    private bool facingLeft;
    public Transform detectPoint; // Point to detect obstacles
    public float Distance = 0.4f; // Distance to check for obstacles
    public LayerMask whatIsGround; // Layer mask to identify ground and obstacles
    
    // Start is called before the first frame update
    void Start()
    {
        facingLeft = true; // Initialize facing direction
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(transform.position, player.position) <= attackRange)
        {

        }
        else
        {
            transform.Translate(Vector2.left* Time.deltaTime * patrolSpeed);

            //Physics2D.Raycast()
        }
    }
}
