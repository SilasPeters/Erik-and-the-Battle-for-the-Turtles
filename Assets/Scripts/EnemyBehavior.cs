using System;
using System.Collections;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    /// <summary>
    /// Move at a constant speed, not a constant duration
    /// Make every value here public
    /// Let him tell others he is engaged, after a short delay
    /// Move met time.deltatime?
    /// </summary>
    
    public Transform enemy;
    public Animator enemyAnimator;
    public Transform player;

    public float health;
    public float damageMultiplier;
    public float flinch;
    public float accuracy;
    public float hitIndicatorOpacity;
    //public float hitIndicatorLifetime;

    public float engageDistance;
    public float disengageDistance;
    public float cuddleDistance;

    public float chanceToCloseDistance;
    public float chanceToCyclePos;

    private float playerDistance;
    private bool engaged;
    private bool moving; //prevents the coroutines from being spammed
    //private bool rotating; //prevents the coroutines from being spammed
    private bool shooting; //prevents the coroutines from being spammed

    private Vector3 pTarget;
    public Vector3 posA;
    public Vector3 posB;

    public Transform muzzleFlashPng;
    public Light muzzleFlashLight;

    void Update()
    {
        if (health > 0) //only execute script if the enemy is still alive
        {

            if (moving)
                enemyAnimator.SetBool("Running", true);
            else
                enemyAnimator.SetBool("Running", false);


            playerDistance = Vector3.Distance(enemy.position, player.position);
            if (playerDistance <= engageDistance)
            {
                engaged = true;
            }
            else if (playerDistance > disengageDistance)
            {
                engaged = false;
            }


            if (engaged && !moving)
            {
                enemy.LookAt(player.position + new Vector3(0, -10, 0));
                if (playerDistance > cuddleDistance && Math.Round(Time.time, 0) % chanceToCloseDistance == 0) //walks towards a position around the player every 6 seconds
                {
                    StartCoroutine(Move(enemy.position, player.position + new Vector3(UnityEngine.Random.Range(-cuddleDistance, cuddleDistance), -6.8f, UnityEngine.Random.Range(-cuddleDistance, cuddleDistance)), 2f));
                }
                else if (UnityEngine.Random.Range(0, 100) <= 1 && !shooting)
                {
                    StartCoroutine(Shoot());
                    //Debug.Log("Shoot!");
                }
            }
            else if (Math.Round(Time.time, 0) % chanceToCyclePos == 0 && !moving) //walks between A and B every 10 seconds
            {
                if (Vector3.Distance(enemy.position, posA) < 10f) { pTarget = posB; } //at A
                else if (Vector3.Distance(enemy.position, posB) < 10f) { pTarget = posA; } //at B
                else { pTarget = posA; } //nowhere near A or B, and thus has to move to A

                enemy.LookAt(pTarget);
                StartCoroutine(Move(enemy.position, pTarget, 1.5f));
            }
        }
    }

    private IEnumerator Move(Vector3 pStart, Vector3 pEnd, float moveDuration)
    {
        moving = true;

        float timeStarted = Time.time;

        while (Time.time - timeStarted < moveDuration)
        {
            enemy.position = Vector3.Lerp(pStart, pEnd, (Time.time - timeStarted) / moveDuration);
            yield return null;
        }

        enemyAnimator.SetBool("Running", false);
        moving = false;
    }

    private IEnumerator RotateTo(Vector3 direction, float duration)
    {
        //rotating = true;
        float timeStarted = Time.time;
        while (Time.time - timeStarted <= duration)
        {
            enemy.rotation = Quaternion.Lerp(enemy.rotation, Quaternion.LookRotation(direction), (Time.time - timeStarted) / duration);
            yield return null;
        }
    }

    private IEnumerator Shoot()
    {
        shooting = true;
        enemyAnimator.Play("Shoot");

        muzzleFlashLight.range = 40;
        muzzleFlashPng.localScale = new Vector3(UnityEngine.Random.Range(0.05f * 2, 0.06f * 2), UnityEngine.Random.Range(0.05f * 2, 0.06f * 2), UnityEngine.Random.Range(0.05f * 2, 0.06f * 2));
        yield return new WaitForSeconds(0.010f);
        muzzleFlashPng.localScale = new Vector3(UnityEngine.Random.Range(0.045f * 2, 0.05f * 2), UnityEngine.Random.Range(0.045f * 2, 0.05f * 2), UnityEngine.Random.Range(0.045f * 2, 0.05f * 2));
        yield return new WaitForSeconds(0.005f);

        if (UnityEngine.Random.Range(0, 100) <= accuracy) ///x% chance to hit player
        {
            player.GetComponent<PlayerHealth>().TakeDamage(damageMultiplier, flinch, transform.position, hitIndicatorOpacity);
        }

        muzzleFlashLight.range = 0;
        muzzleFlashPng.localScale = new Vector3(0.00f, 0.00f, 0.00f);
        shooting = false;
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            enemy.GetComponent<Animator>().enabled = false;
        }
    }
}