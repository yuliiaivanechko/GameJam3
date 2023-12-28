using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] 
    private float startingHealth;
    public float CurrentHealth
    {
        get
        {
            return GetCurrentHealth();
        }
        private set
        {
            SetCurrentHealth(value);
        }
    }

    private Animator anim;
    private bool dead;

    [Header("iFrames")]
    [SerializeField]
    private float iFramesDuration;
    [SerializeField]
    private int numberOfFlashes;

    private SpriteRenderer spriteRend;

    [Header("Components")]
    [SerializeField] 
    private Behaviour[] components;

    private bool invulnerable;

    protected virtual void Awake()
    {
        anim = GetComponent<Animator>();
        spriteRend = GetComponent<SpriteRenderer>();
    }
    public void TakeDamage(float _damage)
    {
        if (invulnerable) return;
        CurrentHealth = Mathf.Clamp(CurrentHealth - _damage, 0, startingHealth);

        if (CurrentHealth > 0)
        {
            anim.SetTrigger("Damage");
            StartCoroutine(Invunerability());
        }
        else
        {
            if (!dead)
            {
                anim.SetTrigger("Kill");

                
                foreach (Behaviour component in components)
                    component.enabled = false;

                dead = true;
            }
        }
    }

    public bool IsDead
    {
        get
        {
            return dead;
        }
    }
    public void AddHealth(float _value)
    {
        CurrentHealth = Mathf.Clamp(CurrentHealth + _value, 0, startingHealth);
    }

    public void ResetHealth()
    {
        CurrentHealth = startingHealth;
    }

    protected virtual float GetCurrentHealth()
    {
        return 0;
    }

    protected virtual void SetCurrentHealth(float health)
    {
        return;
    }

    private IEnumerator Invunerability()
    {
        invulnerable = true;
        for (int i = 0; i < numberOfFlashes; i++)
        {
            spriteRend.color = new Color(1, 0, 0, 0.5f);
            yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes * 2));
            spriteRend.color = Color.white;
            yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes * 2));
        }
        invulnerable = false;
    }
}