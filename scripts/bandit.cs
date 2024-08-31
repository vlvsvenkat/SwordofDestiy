using System.Collections;
using System.Collections.Generic;
using UnityEditor.Callbacks;
using UnityEngine;
[RequireComponent(typeof(Rigidbody2D), typeof(airsimulation), typeof(damagable))]

public class Bandit : MonoBehaviour
{
    public detectionzone attackZone;
    private Animator animator;
    damagable damagable;
    Rigidbody2D rb;
    public float walkSpeed = 3f; // Speed at which the bandit walks

    private airsimulation airSim; // Reference to the AirSimulation script attached to this GameObject
    private bool hasTarget = false;
    public float walkStopRate = 0.22f;

    public bool HasTarget
    {
        get { return hasTarget; }
        set
        {
            hasTarget = value;
            if (animator != null)
            {
                animator.SetBool("hasTarget", value);
            }
        }
    }
    public bool canMove
    {
        get
        {
            return animator.GetBool("canMove");
        }
    }

    private void Awake()
    {
        airSim = GetComponent<airsimulation>();
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        damagable = GetComponent<damagable>();
    }

    private void Update()
    {
        if (attackZone != null)
        {
            HasTarget = attackZone.detectedcolliders.Count > 0;
        }
        else
        {
            HasTarget = false;
        }


        // If the bandit is on a wall, flip its direction
        if (airSim != null && airSim.IsOnWall)
        {
            // Flip the bandit's direction
            FlipDirection();
        }
        if (!damagable.LockVelocity)
        {
            if (canMove)
            {
                Vector3 movement = new Vector3(walkSpeed * Time.deltaTime * (transform.localScale.x > 0 ? 1 : -1), 0f, 0f);
                transform.position += movement;

            }
            else
            {
                rb.velocity = new Vector2(Mathf.Lerp(rb.velocity.x, 0, walkStopRate), rb.velocity.y);
            }

        }



    }

    // Flip the bandit's direction
    private void FlipDirection()
    {
        // Reverse the scale of the bandit along the X-axis to flip its direction
        Vector3 newScale = transform.localScale;
        newScale.x *= -1;
        transform.localScale = newScale;
    }
    public void OnHt(int damage, Vector2 knockback)
    {
        rb.velocity = new Vector2(knockback.x, rb.velocity.y);
    }
}
