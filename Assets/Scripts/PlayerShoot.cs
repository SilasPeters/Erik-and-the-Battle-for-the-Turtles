using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Somehow the animation takes twice as long as intended (0.1s in stead of 0.05s)
/// 
/// The ammo count is not perfectly aligned with adjustable screen sizes
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
    public GameObject electrical;
    public LayerMask raycastTarget;
    public Transform ammoCount;

    public float recoilDuration; //dit moet gelijk zijn aan de animation duration
    public float range = Mathf.Infinity;
    public float damage;
    public float maxAmmo;
    private float ammo;
    public float Ammo
    {
        get { return ammo; }
        set
        {
            ammo = value;
            ammoCount.GetComponent<Text>().text = $"{ammo} / {maxAmmo}";
            
            if (ammo < maxAmmo * 0.2)
            {
                ammoCount.GetComponent<Text>().color = Color.red;
            }
            else
            {
                ammoCount.GetComponent<Text>().color = Color.white;
            }
            if (ammo < 0) { Debug.LogWarning("Ammo is less than 0 - Said by AmmoSet() in PlayerShoot.cs"); }
        }
    }
    bool reloading;
    float timeLastExec; //this is used to not execute the code unless the last time it was done was more than X seconds ago


    private void Start()
    {
        Ammo = maxAmmo;
    }

    void Update()
    {
        if (Input.GetButton("Fire1") && Time.time - timeLastExec >= recoilDuration && Ammo > 0 && !reloading) //fire if capable
        {
            timeLastExec = Time.time; //sets timer
            Ammo -= 1;
            animator.Play("Recoil"); //let's the recoil hit once
            StartCoroutine(MuzzleFlash());
            //all the visable things related to shooting

            if (Physics.Raycast(cam.transform.position, cam.transform.forward, out RaycastHit hit, range, raycastTarget))
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
                    //Debug.Log("Hit Building");
                    //Debug.Log(hit.transform.gameObject.name);
                    GameObject hitParticle = Instantiate(building, hit.point, Quaternion.LookRotation(hit.normal), EDParent);
                    Destroy(hitParticle, 2f);
                }
                else if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Dial Pad"))   //EntranceRoom_Door.cs
                {
                    GameObject hitParticle = Instantiate(electrical, hit.point, Quaternion.LookRotation(hit.normal), EDParent);
                    Destroy(hitParticle, 2f);
                    FindObjectOfType<EntranceRoom_Door>().Unlocked = true;
                }
                else if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Hallway Lock ID")) //HallwayDoor.cs : the buttons 6 and 9 on the lock have this layer
                {
                    //Debug.Log("ID");
                    //Speel sound
                    FindObjectOfType<Hallway_Door>().Counter++;
                }
                else if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Hallway Lock Handprint")) //HallwayDoor.cs
                {
                    GameObject hitParticle = Instantiate(electrical, hit.point, Quaternion.LookRotation(hit.normal), EDParent);
                    Destroy(hitParticle, 2f);
                    //change hand sprite to broken screen sprite
                    //Speel sound
                    FindObjectOfType<Hallway_Door>().HandprintOK = true;
                }
                else if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Decontamination")) //Door_Contamination.cs
                {
                    //Debug.Log("Hit");
                    GameObject hitParticle = Instantiate(electrical, hit.point, Quaternion.LookRotation(hit.normal), EDParent);
                    Destroy(hitParticle, 2f);
                    //change hand sprite to broken screen sprite
                    //Speel sound
                    hit.transform.GetComponentInParent<Door_Contamination>().StartCoroutine("Decontaminate"); //triggers the decontamination to start
                }
                else
                {
                    Debug.LogWarning("Could not determine layer type of raycast hit");
                }

            }
        }
        else if (Ammo == 0) //if the player ran out of ammo he automatically reloads
        {
            StartCoroutine(Reload());
        }
        //the actual shooting

        if (Input.GetKey(KeyCode.R) && !reloading && Ammo < maxAmmo)
        {
            StartCoroutine(Reload());
        }
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

    IEnumerator Reload()
    {
        reloading = true;
        animator.Play("Reloading");
        yield return new WaitForSeconds(39f / 60f); // based upon frameWhenMagazineClicks / totalFramesPerSecond

        Ammo = maxAmmo;
        yield return new WaitForSeconds((60f - 39f) / 60f); //based upon frames left after magazineclick / frames per second

        reloading = false;
    }
}
