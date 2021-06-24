using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayMusic : MonoBehaviour
{
    public AudioSource song;
    public float delay;
    public void Play()
    {
        song.PlayDelayed(delay);
    }
}
