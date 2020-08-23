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

    public void TakeDamage(float damageMultiplier, float flinch, Vector3 enemyPosition, float hitIndicatorOpacity, float hitIndicatorDestroyTime)
    {
        playerHealth *= damageMultiplier;
        lastTimeHit = Time.time;

        if (hitIndicatorOpacity != 0)
        {
            Vector3 relativePosition = enemyPosition - transform.position;
            float angle = Vector3.Angle(relativePosition, transform.forward); //calculates the angle on wich the enemy hit you
                Debug.Log(angle);

            Image hitIndicator = Instantiate(hitIndicatorSource, UIParent);
            var tempColor = hitIndicator.color;
            tempColor.a = hitIndicatorOpacity;
            hitIndicator.color = tempColor; //sets initial opacity of the hitIndicator
            hitIndicator.gameObject.SetActive(true); //source is disabled, but instantiated should be enabled

            hitIndicator.transform.rotation = Quaternion.Euler(hitIndicatorSource.transform.rotation.x, hitIndicatorSource.transform.rotation.y, angle);
            //sets rotation

            Destroy(hitIndicator.gameObject, hitIndicatorDestroyTime);
            //destroys (should become fades and destroys)
        }


        //flinch *= Mathf.Sin(angle * (Mathf.PI / 180));
        //Camera.main.transform.Rotate(new Vector3(0, 0, flinch));
    }
}
