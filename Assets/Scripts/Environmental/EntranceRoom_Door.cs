using System.Collections;
using UnityEngine;

public class EntranceRoom_Door : MonoBehaviour
{
    Vector3 posClosed;
    Vector3 posOpen;
    public Vector3 openedOffset;
    public Transform player;
    public Transform dialPadText;

    public float progressionPerSecond;
    public float triggerRange;

    private bool opened;
    private bool unlocked;
    //private bool musicStarted;
    public bool Unlocked
    {
        get { return unlocked; }
        set
        {
            unlocked = value;
            if (dialPadText.GetComponent<TextMesh>().text != "Y u do this") {; }; //if this is the first time the door is enabled
            dialPadText.GetComponent<TextMesh>().text = "Y u do this";
        }
    }

    private void Start()
    {
        posClosed = transform.position;
        posOpen = posClosed + openedOffset;
    }

    // Update is called once per frame
    void Update()
    {
        if (Unlocked) //unlocked becomes true when PlayerShoot.cs hits the dialpad at the entrance (which triggers Unlock() )
        {
            /*if (!musicStarted)
            {
                GetComponent<PlayMusic>().Play();
                musicStarted = true;
            }*/
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
