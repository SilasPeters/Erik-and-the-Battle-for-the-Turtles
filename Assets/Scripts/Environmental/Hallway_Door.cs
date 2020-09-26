using System.Collections;
using UnityEngine;

public class Hallway_Door : MonoBehaviour
{
    public Transform sealLockCodeText;
    public Transform sealLockHandprintText;

    public Animator opening;
    public Transform doorToTrack;
    public Transform doorLock;

    private float counter;
    public float Counter
    {
        get { return counter; }
        set
        {
            counter = value;
            if (counter == 4) //counter wordt 5 na Unlock()
            {
                sealLockCodeText.GetComponent<TextMesh>().text = "OK";
                if (HandprintOK) Unlock();
            }
            else if (counter < 4)
            {
                sealLockCodeText.GetComponent<TextMesh>().text = "ID Required";
            }
        }   //text wordt sowieso naar OK gezet, maar als de handprint ook al goed was dan unlock
    }

    private bool handprintOK;
    public bool HandprintOK
    {
        get { return handprintOK; }
        set
        {
            handprintOK = value;
            if (handprintOK)
            {
                sealLockHandprintText.GetComponent<TextMesh>().text = "OK";
                if (Counter == 4) Unlock(); //counter wordt 5 na Unlock()
            }
            else
            {
                sealLockHandprintText.GetComponent<TextMesh>().text = "Bioprint Required";
            }
        }   //text wordt sowieso naar OK gezet, maar als de code ook al goed was dan unlock
    }

    void Unlock()
    {
        Debug.Log("Unlocked");
        opening.SetBool("Open", true);
        Counter = 5; //voorkomt dat deze functie nog een keer 
    }

    void TrackDoor()
    {
        Debug.Log("Event called");
        while (true)
        {
            doorLock.position = new Vector3(doorToTrack.position.x + (139.58f - 110.84f), doorLock.position.y, doorLock.position.z);
        }
    }
}
