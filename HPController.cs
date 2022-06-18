using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPController : MonoBehaviour, IDamgeable
{
    public float myStartingHealth = 100.0f;
    private float health;
    protected bool isHitted;
    protected bool dead;
    public float dealtDamage;

    public float getHealth()
    {
        return this.health;
    }

    protected virtual void Start()
    {
        health = myStartingHealth;
        dead = false;
        isHitted = false;
    }
    public virtual void TakeHit(float damage)
    {
        isHitted = true;
        health -= damage;
        dealtDamage = Mathf.Round(damage * 10) * 0.1f;
        if (health <= 0 && !dead)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        dead = true;
    }
}
