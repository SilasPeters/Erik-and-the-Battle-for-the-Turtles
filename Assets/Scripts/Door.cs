using System.Collections;
using UnityEngine;

public class Door : MonoBehaviour
{
    public Vector3 posClosed;
    public Vector3 posOpen;
    public Transform player;

    public float progressionPerSecond;
    public float triggerRange;

    private bool opened;

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(player.position, posClosed) <= triggerRange && !opened) //has to open
        {
            StopAllCoroutines();
            StartCoroutine(OpenClose(transform.position, posOpen, progressionPerSecond));
            opened = true;
        }
        else if (Vector3.Distance(player.position, posClosed) > triggerRange && opened) //has to close
        {
            StopAllCoroutines();
            StartCoroutine(OpenClose(transform.position, posClosed, progressionPerSecond));
            opened = false;
        }        
    }

    IEnumerator OpenClose(Vector3 posA, Vector3 posB, float progressionPerSecond)
    {
        float progress = 0;
        while (progress < 100)
        {
            progress += progressionPerSecond * Time.deltaTime;
            if (progress > 99) progress = 100;
            transform.position = Vector3.Lerp(posA, posB, progress);
            yield return null;
        }
    }
}
