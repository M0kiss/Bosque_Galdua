using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public Animator MyAni;
    public LayerMask EnemyLayers;
    public KeyCode attackButton;
    public Transform AttackPoint, JumpingAttackPoint;
    public float AttackRate = 2f;
    public float AttackRange = 0.5f, JumpingAttackRange;
    public int AttackDamage = 5;
    public float NextAttackTime = 0f;
    Moving moving;
    Jumping jumping;
    PlayerHighAttack playerHighAttack;
    //Sound
    AudioSource swordSounds;
    AudioClip attackSound;

    private void Start()
    {
        moving = GetComponent<Moving>();
        jumping = GetComponent<Jumping>();
        AudioSource[] audios = GetComponentsInChildren<AudioSource>();
        swordSounds = audios[0];
        playerHighAttack = GetComponent<PlayerHighAttack>();
        attackSound = Resources.Load("sword_swing_1") as AudioClip;
    }

    private void Update()
    {
        if (Time.time >= NextAttackTime && Time.time >= playerHighAttack.NextAttackTime)
        {
            if (Input.GetKeyDown(attackButton) && jumping.IsGrounded == false)
            {
                AttackSoundAndDelay();
                MyAni.SetTrigger("Attack");
                InvokeRepeating(nameof(Attack), .2f, .016f);
                Invoke(nameof(AttackCooldown), .5f);
            }
            else if (Input.GetKeyDown(attackButton) && moving.isMoving == true)
            {
                AttackSoundAndDelay();
                MyAni.SetTrigger("Attack");
                InvokeRepeating(nameof(Attack), .216f, .016f);
                Invoke(nameof(AttackCooldown), .599f);
            }
            else if (Input.GetKeyDown(attackButton))
            {
                AttackSoundAndDelay();
                MyAni.SetTrigger("Attack");
                InvokeRepeating(nameof(Attack), .183f, .016f);
                Invoke(nameof(AttackCooldown), .483f);
            }
        }

    }

    void Attack()
    {
        if (jumping.IsGrounded == false)
        {
            Collider2D[] HitEnemies = Physics2D.OverlapCircleAll(JumpingAttackPoint.position, JumpingAttackRange, EnemyLayers);

            foreach (Collider2D enemy in HitEnemies)
            {
                Debug.Log("Inimigo tomou dano por ataque pulando.");
                enemy.GetComponent<EnemiesScript>().TakeDamage(AttackDamage);
                AttackCooldown();
            }
        }
        else
        {
            Collider2D[] HitEnemies = Physics2D.OverlapCircleAll(AttackPoint.position, AttackRange, EnemyLayers);

            foreach (Collider2D enemy in HitEnemies)
            {
                Debug.Log("Inimigo tomou dano.");
                enemy.GetComponent<EnemiesScript>().TakeDamage(AttackDamage);
                AttackCooldown();
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (AttackPoint == null)
        {
            return;
        }
        Gizmos.DrawWireSphere(AttackPoint.position, AttackRange);
        Gizmos.DrawWireSphere(JumpingAttackPoint.position, JumpingAttackRange);
    }
    public void AttackSoundAndDelay()
    {
        //Sound settings
        int i = Random.Range(1, 3);
        float randomPitch = Random.Range(.9f, 1.1f);
        swordSounds.pitch = randomPitch;
        attackSound = Resources.Load("sword_swing_" + i) as AudioClip;
        swordSounds.PlayOneShot(attackSound);
        NextAttackTime = Time.time + 1f / AttackRate;
    }
    void AttackCooldown()
    {
        CancelInvoke(nameof(Attack));
    }
}