using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Events;

public class damagable : MonoBehaviour
{
    public UnityEvent<int,Vector2>damagableHit;
    Animator animator;
    [SerializeField]
    private int _maxHealth = 100;

    public int MaxHealth
    {
        get { return _maxHealth; }
        set
        {
            _maxHealth = value;
        }
    }
    [SerializeField]
    private int _health = 100;
    public int Health
    {
        get { return _health; }
        set
        {
            _health = value;
            if (_health <= 0)
            {
                IsAlive = false;
                playerDied();
            }
        }
    }
    [SerializeField]
    private bool _isAlive = true;
    [SerializeField]
    private bool isInvincible= false;

    private float timeSinceHit = 0;
    public float invinsibilityTime = 0.25f;

    public bool IsAlive
    {
        get
        {
            return _isAlive;
        }
        set
        {
            _isAlive = value;
            animator.SetBool("isAlive",value);
        }
    }
    public bool LockVelocity { get{
        return animator.GetBool("lockVelocity");
    }set{
        animator.SetBool("lockVelocity",value);
    } }
    private void Awake(){
        animator=GetComponent<Animator>();
    }
    private void Update(){
        if(isInvincible){
            if(timeSinceHit > invinsibilityTime){
                isInvincible=false;
                timeSinceHit=0;
            }
            timeSinceHit += Time.deltaTime;
        }
    }

    public bool Hit(int damage,Vector2 knockback)
    {
        if(IsAlive && !isInvincible){
            Health -= damage;
            isInvincible=true;
            animator.SetTrigger("hit");
            LockVelocity= true;
            damagableHit?.Invoke(damage,knockback);
            return true;
        }
        return false;
    }
    private void playerDied(){
        LevelManager.instance.GameOver();
        gameObject.SetActive(false);
    }

}
