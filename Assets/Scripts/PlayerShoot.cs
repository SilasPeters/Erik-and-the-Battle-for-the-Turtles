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
    public float maxAmmo;
    float ammo;
    bool reloading;

    float timeLastExec; //this is used to not execute the code unless the last time it was done was more than X seconds ago

    private void Start()
    {
        ammo = maxAmmo;
    }

    void Update()
    {
        if (Input.GetButton("Fire1") && Time.time - timeLastExec >= recoilDuration && ammo > 0) //fire
        {
            timeLastExec = Time.time; //sets timer
            ChangeAmmo(-1);
            animator.Play("Recoil"); //let's the recoil hit once
            StartCoroutine(MuzzleFlash());
            //all the background things related to shooting

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
            //the actual shooting
        }

        if (Input.GetKey(KeyCode.R) && !reloading)
        {
            reloading = true;
            
        }
    }

    private void ChangeAmmo(float ammoChange)
    {
        ammo += ammoChange;

        if (ammo < 0) { Debug.LogWarning("Ammo is less than 0 - Said by ChangeAmmo() in PlayerShoot.cs"); }
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
