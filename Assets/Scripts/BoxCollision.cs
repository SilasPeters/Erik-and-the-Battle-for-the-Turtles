using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxCollision : MonoBehaviour
{
    public bool collidedWithPlayer;

    private void OnTriggerEnter(Collider other)
    {
        collidedWithPlayer = true;
    }

    private void OnTriggerExit(Collider other)
    {
        collidedWithPlayer = false;
    }
}
