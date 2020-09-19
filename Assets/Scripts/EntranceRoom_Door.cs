using System.Collections;
using UnityEngine;

public class EntranceRoom_Door : MonoBehaviour
{
    public Vector3 posClosed;
    public Vector3 posOpen;

    public float progressionPerSecond;

    public Transform player;
    private bool busy;
    private bool opened;

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(player.position, posClosed) <= 50 && !busy && !opened) //has to open
        {
            StartCoroutine(OpenClose(posClosed, posClosed, progressionPerSecond));
            Debug.Log("Opening");
            opened = true;
        }
        else if (Vector3.Distance(player.position, posOpen) > 50 && !busy && opened) //has to close
        {
            StartCoroutine(OpenClose(posOpen, posClosed, progressionPerSecond));
            Debug.Log("Closing");
            opened = false;
        }
    }

    IEnumerator OpenClose(Vector3 posA, Vector3 posB, float progressionPerSecond)
    {
        busy = true;
        float progress = 0;
        while (progress < 100)
        {
            progress += progressionPerSecond * Time.deltaTime;
            if (progress > 99) progress = 100;
            transform.position = Vector3.Lerp(posA, posB, progress);
            yield return null;
        }
        busy = false;
    }
}
