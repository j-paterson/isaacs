using UnityEngine;
using System.Collections;

public class SelectedScript : MonoBehaviour {


    /* This script will briefly make the button panel brighter when the panel is selected. SelectTime is the 
     * time at which you pressed the button. And maxtime determines how long the brightness will last. BaseColor
     * is the color of the panel when it is not selected. 
     */ 

    const float lightnessFactor = .25f;
    bool selected;
    float selectTime;
    float maxTime = .25f;
    Color baseColor = new Color();

    void Start () {
        Material copyCat = Instantiate(Resources.Load("Panel", typeof(Material)) as Material);
        Material oldMaterial = GetComponent<Renderer>().material;
        copyCat.color = new Color(oldMaterial.color.r, oldMaterial.color.g,
            oldMaterial.color.b, oldMaterial.color.a);
        baseColor = copyCat.color;
        GetComponent<Renderer>().material = copyCat;
        OnSelect();

    }

    void Update () {
	    if (selected)
        {
            if (Time.time - selectTime > maxTime)
            {
                GetComponent<Renderer>().material.color = baseColor;
                selected = false; 
            } else if (Time.time - selectTime > maxTime/2)
            {
                float lightnessDecrement= (maxTime - Time.time) / (maxTime / 2) * lightnessFactor;
                float colorValue = baseColor.r + lightnessDecrement;
                GetComponent<Renderer>().material.color = new Color(colorValue, colorValue, colorValue, baseColor.a);
            } else
            {
                float lightnessIncrement = (Time.time - selectTime) / (maxTime / 2) * lightnessFactor;
                float colorValue = baseColor.r + lightnessIncrement;
                GetComponent<Renderer>().material.color = new Color(colorValue, colorValue, colorValue, baseColor.a);

            }
        }
	}

    void OnSelect()
    {
        selected = true; 
        selectTime = Time.time; 
     
    }
}
