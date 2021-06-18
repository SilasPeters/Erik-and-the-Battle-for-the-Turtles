using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerVoiceLine : MonoBehaviour
{
    private bool triggered;
    public GameObject commmander;

    private void OnTriggerEnter(Collider other)
    {
        if (triggered || other.name != "Player") return; //only trigger once, if the player triggered

        StartCoroutine(commmander.GetComponent<Communcations>().Transmit(GetComponent<AudioSource>()));

        triggered = true;
    }
}