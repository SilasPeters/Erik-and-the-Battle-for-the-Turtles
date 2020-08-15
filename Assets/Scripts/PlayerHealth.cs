using System.Threading;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PlayerHealth : MonoBehaviour
{
    public static float playerHealth = 100;
    public float maxHealth;
    public static float lastHit = 0;
    public PostProcessVolume volume;

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
                                    Mathf.Clamp((maxVignetteIntensity - minVignetteIntensity) * (1 - playerHealth/100) + minVignetteIntensity, minVignetteIntensity, maxVignetteIntensity),
                                    vignetteShiftSpeed);
        Debug.Log(vignette.intensity.value);

        volume.profile.TryGetSettings(out colorGrading);
        colorGrading.saturation.value = Mathf.Lerp(colorGrading.saturation.value,
                                        Mathf.Clamp(maxColorGradingSaturation * (1 - playerHealth/100), maxColorGradingSaturation, 0),
                                        colorGradingShiftSpeed);
        //regelt alle visualFX
        //audio minder
        

        if (Time.time - lastHit > regenTime && playerHealth < 100) //&& playeHealth < 100 omdat je miss een keer een health boost krijgt en je niet wilt dat hij geclamped wordt naar maxHealth
        {
            playerHealth = Mathf.Clamp(playerHealth + (regenAmountPerS * Time.deltaTime), 0, maxHealth);
        } //regent tot max health

        Debug.Log(playerHealth);
    }
}
