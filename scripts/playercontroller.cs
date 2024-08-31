using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D), typeof(airsimulation), typeof(damagable))]
public class PlayerController : MonoBehaviour
{
    public float walkSpeed = 5f;
    public float jumpImpulse = 5f;
    public float airspeed = 3f;
    Vector2 moveInput;
    bool isMoving = false;

    Rigidbody2D rb;
    Animator animator;
    airsimulation airsim;
    damagable damagable;
    public float CurrentMoveSpeed
    {
        get
        {
            if (canMove)
            {
                if (isMoving && !airsim.IsOnWall)
                {
                    if (airsim.IsGrounded)
                    {
                        return walkSpeed;
                    }
                    else
                    {
                        return airspeed;
                    }
                }
                else
                {
                    return 0;
                }

            }
            else
            {
                return 0;
            }

        }
    }

    public bool _isFacingRight = true;

    public bool IsFacingRight
    {
        get { return _isFacingRight; }
        private set
        {
            if (_isFacingRight != value)
            {
                transform.localScale *= new Vector2(-1, 1);
            }
            _isFacingRight = value;
        }
    }
    public bool canMove
    {
        get
        {
            return animator.GetBool("canMove");
        }
    }
    public bool IsAlive
    {
        get
        {
            return animator.GetBool("isAlive");
        }
    }



    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        airsim = GetComponent<airsimulation>();
        damagable = GetComponent<damagable>();
    }

    void FixedUpdate()
    {
        if (airsim.IsOnWall && !airsim.IsGrounded)
        {
            // Player is on a wall but not grounded, make them fall
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y);
        }
        else
        {
            // Normal movement behavior
            if (!damagable.LockVelocity)
            {
                rb.velocity = new Vector2(moveInput.x * CurrentMoveSpeed, rb.velocity.y);
            }
        }

        animator.SetFloat("yVelocity", rb.velocity.y);
        UpdateAnimation();
    }

    void UpdateAnimation()
    {
        bool wasMoving = isMoving;
        isMoving = moveInput.magnitude > 0;

        if (wasMoving != isMoving)
        {
            animator.SetBool("isMoving", isMoving);
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        if (IsAlive)
        {
            SetFacingDirection(moveInput);
        }


    }

    private void SetFacingDirection(Vector2 moveInput)
    {
        if (moveInput.x > 0 && !IsFacingRight)
        {
            // Face right
            IsFacingRight = true;
        }
        else if (moveInput.x < 0 && IsFacingRight)
        {
            // Face left
            IsFacingRight = false;
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started && airsim.IsGrounded && canMove)
        {
            animator.SetTrigger("jump");
            rb.velocity = new Vector2(rb.velocity.x, jumpImpulse);
        }
    }
    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            animator.SetTrigger("attack");
        }
    }
    public void OnHt(int damage, Vector2 knockback)
    {
        rb.velocity = new Vector2(knockback.x, rb.velocity.y);

    }
    
    

}
