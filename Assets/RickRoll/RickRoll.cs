using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class RickRoll : MonoBehaviour
{
    public GameObject rickRollViewer;
    public GameObject rickRollPlayer;
    public void Hehe()
    {
        rickRollViewer.SetActive(true);
        rickRollPlayer.SetActive(true);
    }
}