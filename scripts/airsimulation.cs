using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class airsimulation : MonoBehaviour
{
    // Start is called before the first frame update
    Rigidbody2D rb;
    public ContactFilter2D castfilter;
    RaycastHit2D[] groundhits = new RaycastHit2D[5];
    RaycastHit2D[] wallHits = new RaycastHit2D[5];
    RaycastHit2D[] celingHits = new RaycastHit2D[5];
    public float grounddist = 0.05f;
    public float wallDistance = 0.2f;
    public float celingDistance = 0.05f;
    CapsuleCollider2D touchingcol;
    private Vector2 wallCheckDirection => gameObject.transform.localScale.x > 0 ?  Vector2.right : Vector2.left;
    
    Animator animator;

    [SerializeField]
    private bool _isgrounded;
    public bool IsGrounded { get{
        return _isgrounded;
    } private set{
        _isgrounded = value;
        animator.SetBool("isGrounded",value);
    } }
    [SerializeField]
    private bool _isOnWall;
    public bool IsOnWall { get{
        return _isOnWall;
    } private set{
        _isOnWall = value;
        animator.SetBool("isOnWall",value);
    } }
    [SerializeField]
    private bool _isOnCeling;
    

    public bool IsOnCeling { get{
        return _isOnCeling;
    } private set{
        _isOnCeling = value;
        animator.SetBool("isOnCeling",value);
    } }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        touchingcol = GetComponent<CapsuleCollider2D>();
        animator=GetComponentInChildren<Animator>();
    }
    void FixedUpdate(){
        IsGrounded = touchingcol.Cast(Vector2.down, castfilter, groundhits, grounddist)>0;
        IsOnWall= touchingcol.Cast(wallCheckDirection,castfilter,wallHits,wallDistance)>0;
        IsOnCeling= touchingcol.Cast(Vector2.up,castfilter,celingHits,celingDistance)>0;
    }
}
