using System;
using System.Collections;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms;

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
    public float accuracy;
    public float engageDistance;
    public float disengageDistance;
    public float cuddleDistance;

    public float chanceToCloseDistance;
    public float chanceToCyclePos;

    private float playerDistance;
    private Boolean engaged;
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
                Debug.Log("Shoot!");
            }
        }
        else if (Math.Round(Time.time, 0) % chanceToCyclePos == 0 && !moving) //walks between A and B every 10 seconds
        {
            if      (Vector3.Distance(enemy.position, posA) < 10f) { pTarget = posB; } //at A
            else if (Vector3.Distance(enemy.position, posB) < 10f) { pTarget = posA; } //at B
            else    { pTarget = posA; } //nowhere near A or B, and thus has to move to A
        
            enemy.LookAt(pTarget);
            StartCoroutine(Move(enemy.position, pTarget, 1.5f));
        }
    }

    //{
    //    distance = Vector3.Distance(enemy.position, player.position);
    //
    //    if (distance <= 30f) { engaged = true; }
    //    if (engaged && distance < 50f && !busy && health > 0)  //active movement
    //    {
    //        if (UnityEngine.Random.Range(0, 100) <= 0 && distance > 20f) //75% chance to walk towards the player, unless close enough already
    //        {
    //            enemyAnimator.SetBool("Running", true);
    //            StartCoroutine(RotateTo(new Vector3(player.position.x, player.position.y - 20, player.position.z)));
    //            StartCoroutine(Move(enemy.position, new Vector3(player.position.x, player.position.y - 5, player.position.z + 5), 3));
    //        }
    //        else //25% chance to stand still, aiming at the player
    //        {
    //            StartCoroutine(RotateTo(new Vector3(player.position.x, 0, player.position.z)));
    //            //shoot here
    //            StartCoroutine(Move(enemy.position, enemy.position, 3)); //walks towards himself, so he does not
    //        }
    //    }
    //    else if (!busy && health > 0)   //passive movement
    //    {
    //        engaged = false;
    //        if (UnityEngine.Random.Range(0, 100) <= 100) //20% chance to walk to A or B
    //        {
    //            if (Vector3.Distance(enemy.position, posA) < 10f) //at A
    //            {
    //                pTarget = posB;
    //            }
    //            else if (Vector3.Distance(enemy.position, posB) < 10f) //at B
    //            {
    //                pTarget = posA;
    //            }
    //            else
    //            {
    //                pTarget = posA;
    //            }
    //            enemyAnimator.SetBool("Running", true);
    //            StartCoroutine(RotateTo(pTarget));
    //            StartCoroutine(Move(enemy.position, pTarget, 3));
    //        }
    //        else //80% chance to stand still
    //        {
    //            StartCoroutine(Move(enemy.position, enemy.position, 3)); //Moves from A to A (yep)
    //        }
    //    }
    //}

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
        enemyAnimator.Play("Shoot");
        shooting = true;

        muzzleFlashLight.range = 25;
        muzzleFlashPng.localScale = new Vector3(UnityEngine.Random.Range(0.05f, 0.06f), UnityEngine.Random.Range(0.05f, 0.06f), UnityEngine.Random.Range(0.05f, 0.06f));
        yield return new WaitForSeconds(0.010f);
        muzzleFlashPng.localScale = new Vector3(UnityEngine.Random.Range(0.045f, 0.05f), UnityEngine.Random.Range(0.045f, 0.05f), UnityEngine.Random.Range(0.045f, 0.05f));
        yield return new WaitForSeconds(0.005f);

        if (UnityEngine.Random.Range(0, 100) <= accuracy) ///x% chance to hit player
        {
            player.GetComponent<PlayerHealth>().TakeDamage(damageMultiplier);
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