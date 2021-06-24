using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StupidChinese : MonoBehaviour
{
    private bool shouted = false;
    public AudioSource shout;
    void Update()
    {
        if (GetComponent<EnemyBehavior>().engaged && !shouted)
        {
            shout.Play();
            shouted = true;
        }
        if (GetComponent<EnemyBehavior>().health <= 0)
        {
            gameObject.SetActive(false);
        }
    }
}
