using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Briefing : MonoBehaviour
{
    public GameObject WelcomeScreen;
    public GameObject BriefingScreen;
    public GameObject Subtitles;
    private Text subs;
    private float timestarted;
    // Start is called before the first frame update
    public void ButtonClicked()
    {
        WelcomeScreen.SetActive(false);
        BriefingScreen.SetActive(true);

        subs = Subtitles.GetComponent<Text>();
        timestarted = Time.time;
    }

    private void Update()
    {
        //set the value of the subtitles
        float progress = Time.time - timestarted;
        if     (progress < 3)    { subs.text = "Erik, we have a very important mission for you."; }
        else if(progress < 13)   { subs.text = "This operation is marked with confidential level 5. Only you, me and your commander know about what is about to happen."; }
        else if(progress < 25)   { subs.text = "Satellites show irregular behavior around a forest in Bolivia. Intelligence suggest the Chinese might be working on a new kind of weapon."; }
        else if(progress < 33)   { subs.text = "Of course, this sounds like a threat itself, but this mission is of top priority because of something else."; }
        else if(progress < 34)   { subs.text = "The Chinese, they..."; }
        else if(progress < 44)   { subs.text = "They are transporting turtles to their base. And not just a few. We believe those turtles are used for their new weapon."; }
        else if(progress < 45)   { subs.text = "Please, Erik."; }
        else if(progress < 47)   { subs.text = "Save those turtles."; }
        else { SceneManager.LoadScene("Main"); }
    }
}
