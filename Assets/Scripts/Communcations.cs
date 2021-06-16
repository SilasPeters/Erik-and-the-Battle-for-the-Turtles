using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Communcations : MonoBehaviour
{
    public float transitionSpeed;
    private Vector2 posActive;
    public Vector2 posInactive;
    private bool active = false;
    public AudioSource instructionsOnLaunch;

    void Start()
    {
        //remmeber current position as active position and move the banner to it's inactive position
        posActive = transform.localPosition;
        transform.localPosition = posInactive;

        //launch first instructions
        StartCoroutine("Transmit", instructionsOnLaunch);
    }

    void Update()
    {
        // transition between two positions of the banner, depending on active state
        if (active) { transform.localPosition = Vector2.Lerp(transform.localPosition, posActive, transitionSpeed * Time.deltaTime); }
        if (!active) { transform.localPosition = Vector2.Lerp(transform.localPosition, posInactive, transitionSpeed * Time.deltaTime); }
    }

    /// <summary>
    /// Activating the banner and plays the given audio. After the audio has played, inactivate the banner again
    /// </summary>
    public IEnumerator Transmit(AudioSource audio)
    {
        active = true;

        audio.PlayDelayed(0.2f);
        Debug.Log("Voice-line is playing");
        while (audio.isPlaying) { yield return 0; } //keep looping until audio has been played completely

        active = false;
    }
}
