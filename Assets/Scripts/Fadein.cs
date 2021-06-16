using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fadein : MonoBehaviour
{
    public float speed;

    void Update()
    {
        Color color = GetComponentInParent<Image>().color;
        if (color.a > 0) { GetComponentInParent<Image>().color = new Color(color.r, color.g, color.b, color.a - speed/100 * Time.deltaTime); }
    }
}
