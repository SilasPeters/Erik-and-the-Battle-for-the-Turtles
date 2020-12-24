using System.Collections;
using UnityEngine;

public class Door_Contamination : MonoBehaviour
{
    /// <summary>
    /// this script is bugged: it will not allow the player to start the decontamination twice
    /// However, this is perfect for the story, since the player should be locked in the room until the doors open themselves for emergency protocol needs
    /// </summary>
    /// 
    Vector3 posClosed;
    Vector3 posOpen;
    public Vector3 openedOffset;
    public Transform player;
    public Transform DoorOther;
    public GameObject hitbox;
    public ParticleSystem[] smoke;
    public Light[] lights;
    public Color NotOK;
    public Color OK;

    public float progressionPerSecond;
    
    
    private void Start()
    {
        posClosed = transform.position;
        posOpen = posClosed + openedOffset;
    }
    
    public IEnumerator Decontaminate()
    {
        //Debug.Log("Start");
        StartCoroutine(MoveDoor(transform, posOpen)); //open first door
        while (!hitbox.GetComponent<BoxCollision>().collidedWithPlayer) { yield return null; } //wait for player to enter

        //Debug.Log("2");
        StopCoroutine("MoveDoor"); //stops the door behind you from keeping opening
        StartCoroutine(MoveDoor(transform, posClosed)); //close first door
        yield return new WaitForSeconds((1 / progressionPerSecond) + 2); //time for the door to close + 2

        foreach (var o in smoke)
        {
            o.Play();
        } //play smoke particles
          //sound?

        yield return new WaitForSeconds(3); //wait for the cleaning + extra


        foreach (var o in lights)
        {
            o.color = OK;
        } //turn lights to OK color

        DoorOther.GetComponent<Door_Contamination>().StartCoroutine(MoveDoor(DoorOther.GetComponent<Door_Contamination>().transform, DoorOther.GetComponent<Door_Contamination>().posOpen));
        while (hitbox.GetComponent<BoxCollision>().collidedWithPlayer) { yield return null; } //wait for player to leave

        DoorOther.GetComponent<Door_Contamination>().StartCoroutine(MoveDoor(DoorOther.GetComponent<Door_Contamination>().transform, DoorOther.GetComponent<Door_Contamination>().posClosed));
        yield return new WaitForSeconds(1 / DoorOther.GetComponent<Door_Contamination>().progressionPerSecond); //time for the door to close

        foreach (var o in lights)
        {
            o.color = NotOK;
        } //turn lights to NotOK color
    }   



    IEnumerator MoveDoor(Transform door, Vector3 posB)
    {
        Vector3 posA = door.position;
        float progress = 0;
        while (progress < 100)
        {
            progress += progressionPerSecond * Time.deltaTime;
            if (progress > 99) progress = 100;
            door.position = Vector3.Lerp(posA, posB, progress);
            yield return null;
        }
    }
}
