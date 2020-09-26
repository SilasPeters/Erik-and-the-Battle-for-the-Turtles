using System.Collections;
using UnityEngine;

public class Hallway_Door : MonoBehaviour
{
    public Transform sealLockCodeText;
    public Transform sealLockHandprintText;

    public Animator animator;
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
                if (Counter >= 4) Unlock(); //counter is 4 of meer na de juiste code, en wordt 5 na Unlock()
            }
            else
            {
                sealLockHandprintText.GetComponent<TextMesh>().text = "Bioprint Required";
            }
        }   //text wordt sowieso naar OK gezet, maar als de code ook al goed was dan unlock
    }

    void Unlock()
    {
        Counter = 5; //voorkomt dat deze functie nog een keer afgaat

        animator.SetBool("Open", true);
        StartCoroutine(TrackDoor());
    }

    IEnumerator TrackDoor() //makes sure that the lock follows the door, because when parenting it fucking scales wrong
    {
        float time = Time.time;
        while (Time.time - time < 10) //10 == animation duration
        {
            doorLock.position = new Vector3(doorToTrack.position.x - 28.74f, doorLock.position.y, doorLock.position.z);
            yield return null;
        }
    } //het is netter om dit in de animation tree te zetten
}