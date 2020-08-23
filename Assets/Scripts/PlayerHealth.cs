using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public float playerHealth = 100;
    public float maxHealth;
    private float lastTimeHit = 0;
    public PostProcessVolume volume;
    public Transform UIParent;
    public Image hitIndicatorSource;

    private Vignette vignette;
    public float minVignetteIntensity;
    public float maxVignetteIntensity;
    public float vignetteShiftSpeed;

    private ColorGrading colorGrading;
    public float maxColorGradingSaturation;
    public float colorGradingShiftSpeed;

    public float regenTime;
    public float regenAmountPerS;

    void Update()
    {
        volume.profile.TryGetSettings(out vignette);
        vignette.intensity.value = Mathf.Lerp(vignette.intensity.value,
                                    Mathf.Clamp((maxVignetteIntensity - minVignetteIntensity) * (1 - playerHealth / 100) + minVignetteIntensity, minVignetteIntensity, maxVignetteIntensity),
                                    vignetteShiftSpeed);
        //Debug.Log(vignette.intensity.value);

        volume.profile.TryGetSettings(out colorGrading);
        colorGrading.saturation.value = Mathf.Lerp(colorGrading.saturation.value,
                                        Mathf.Clamp(maxColorGradingSaturation * (1 - playerHealth / 100), maxColorGradingSaturation, 0),
                                        colorGradingShiftSpeed);
        //regelt alle visualFX
        //audio minder

        if (Time.time - lastTimeHit > regenTime && playerHealth < 100) //&& playeHealth < 100 omdat je miss een keer een health boost krijgt en je niet wilt dat hij geclamped wordt naar maxHealth
        {
            playerHealth = Mathf.Clamp(playerHealth + (regenAmountPerS * Time.deltaTime), 0, maxHealth);
        } //regent tot max health

        //doe fov + 0.2 als je geraakt wordt, plus een hit indicator

        if (Camera.main.transform.localEulerAngles.z != 0)
        {
            float flinchReset = Mathf.SmoothStep(Camera.main.transform.localEulerAngles.z, 0, 0.1f);
            if (flinchReset < 0.1) { flinchReset = 0; }
            //Camera.main.transform.localEulerAngles = new Vector3(Camera.main.transform.localEulerAngles.x, Camera.main.transform.localEulerAngles.y, flinchReset);
        } //when taking damage the player flinches to a side, this returns it to 0 (see TakeDamage() )
    }

    public void TakeDamage(float damageMultiplier, float flinch, Vector3 enemyPosition, float hitIndicatorOpacity)///, float hitIndicatorLifetime) (zie coroutine ImageFD)
    {
        playerHealth *= damageMultiplier;
        lastTimeHit = Time.time;

        if (hitIndicatorOpacity != 0) //do show hitindicator
        {        
            Image hitIndicator = Instantiate(hitIndicatorSource, UIParent);
            var tempColor = hitIndicator.color;
            tempColor.a = hitIndicatorOpacity;
            hitIndicator.color = tempColor; 
            //sets initial opacity of the hitIndicator

            Vector3 relativePosition = enemyPosition - transform.position;
            relativePosition.y = 0; //You don't want to make Vector3.Angle add the height difference to the total angle difference
            float angle = Vector3.Angle(relativePosition, transform.forward); //calculates the angle on wich the enemy hit (the smallest angle between the forward and relativePos factor) 
            if (transform.InverseTransformPoint(enemyPosition).x > 0) { angle = 360 - angle; } //if the enemy is on the right, the angle between the vectors will be substracted from 360, so that the hit indicator lands on the right side
            hitIndicator.transform.rotation = Quaternion.Euler(hitIndicatorSource.transform.rotation.x, hitIndicatorSource.transform.rotation.y, angle);
////////////x rotation reset naar 0            //sets rotation

            hitIndicator.gameObject.SetActive(true);
            //source is disabled, but instantiated should be enabled

            StartCoroutine(ImageFD(hitIndicator));
            //Fades and destroys hitIndicator
        }


        //flinch *= Mathf.Sin(angle * (Mathf.PI / 180));
        //Camera.main.transform.Rotate(new Vector3(0, 0, flinch));
    }

    public IEnumerator ImageFD(Image img)///, float duration) //image Fade & Destroy
    {
        float FadePerSecond = 0.20f; /// img.color.a / duration; (een vaste duratie)

        var tempColor = img.color;
        while (tempColor.a > 0)
        {
            tempColor.a -= FadePerSecond * Time.deltaTime;
            img.color = tempColor;
            yield return null;
        }
        Destroy(img.gameObject);
    }
}
