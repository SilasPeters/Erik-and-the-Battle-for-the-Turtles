using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.SocialPlatforms;

/// <summary>
/// Somehow the animation takes twice as long as intended (0.1s in stead of 0.05s)
/// </summary>

public class PlayerShoot : MonoBehaviour
{
    public Animator animator;
    public Transform cam;
    public Transform muzzleFlashPng;
    public Light muzzleFlashLight;

    public Transform EDParent;
    public GameObject ground; ///particle system which hits ground
    public GameObject building;
    public GameObject blood;
    public LayerMask raycastTarget;

    public float recoilDuration; //dit moet gelijk zijn aan de animation duration
    public float range = Mathf.Infinity;
    public float damage;

    private float timeLastExec; //this is used to not execute the code unless the last time it was done was more than X seconds ago


    void Update()
    {
        if (Input.GetButton("Fire1") && Time.time - timeLastExec >= recoilDuration)
        {
            timeLastExec = Time.time; //sets timer
            animator.Play("Recoil"); //let's the recoil hit once
            StartCoroutine(MuzzleFlash());

            RaycastHit hit;
            if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, range, raycastTarget))
            {
                if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Enemy"))
                {
                    hit.transform.GetComponentInParent<EnemyBehavior>().TakeDamage(damage);
                    GameObject hitParticle = Instantiate(blood, hit.point, Quaternion.LookRotation(hit.normal), EDParent);
                    Destroy(hitParticle, 2f);
                }
                else if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Ground"))
                {
                    GameObject hitParticle = Instantiate(ground, hit.point, Quaternion.LookRotation(hit.normal), EDParent);
                    Destroy(hitParticle, 2f);
                }
                else if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Building"))
                {
                    GameObject hitParticle = Instantiate(building, hit.point, Quaternion.LookRotation(hit.normal), EDParent);
                    Destroy(hitParticle, 2f);
                }
                else
                {
                    Debug.LogWarning("Could not determine layer type of raycast hit");
                }
                
            }
        }
        //else if (Time.time - timeLastExec >= 0.5f && Time.time - timeLastExec <= 0.6f)
        //{
        //    smoke.Play();
        //}
    }

    IEnumerator MuzzleFlash()
    {
        muzzleFlashLight.range = 25;
        muzzleFlashPng.localScale = new Vector3(UnityEngine.Random.Range(0.03f, 0.04f), UnityEngine.Random.Range(0.03f, 0.04f), UnityEngine.Random.Range(0.03f, 0.04f));
        yield return new WaitForSeconds(0.010f);
        muzzleFlashPng.localScale = new Vector3(UnityEngine.Random.Range(0.025f, 0.03f), UnityEngine.Random.Range(0.025f, 0.03f), UnityEngine.Random.Range(0.025f, 0.03f));
        yield return new WaitForSeconds(0.005f);

        muzzleFlashLight.range = 0;
        muzzleFlashPng.localScale = new Vector3(0.00f, 0.00f, 0.00f);
    }
}
