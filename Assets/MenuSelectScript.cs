using UnityEngine;
using System.Collections;

public class MenuSelectScript : MonoBehaviour {

    /* This script appears in the three main boards: Waypoint control, Tools control and
     * flight control. In the start it determines which board is currently contains the 
     * script and sets the panel section to be invisible. When the board is selected the
     * script will make the panel section visible again. 
     */ 


    GameObject utilities;
    GameObject flight;
    GameObject wayPoint; 
    bool isOpen;
	void Start () {
        if (gameObject.name == "Tools Control")
        {
            utilities = GameObject.Find("UtilitiesBoard");
            utilities.SetActive(false);
        }
        else if (gameObject.name == "Flight Control")
        {
            flight = GameObject.Find("BackBoard");
            flight.SetActive(false);
        }

        else if (gameObject.name == "WayPoint Control")
        {
            wayPoint = GameObject.Find("WayPointBoard");
            wayPoint.SetActive(false);
        }

    }
    
    void OnSelect()
    {
        if (gameObject.name == "Tools Control")
        {
            if (isOpen)
            {
                utilities.SetActive(false); 
                isOpen = false; 
            }
            else
            {
                utilities.SetActive(true); 
                isOpen = true; 
            }

        } 
        else if (gameObject.name == "Flight Control")
        {
            if (isOpen)
            {
                flight.SetActive(false);
                isOpen = false;
            }
            else
            {
                flight.SetActive(true); 
                isOpen = true;
            }
        }
        else if (gameObject.name == "WayPoint Control") 
        {
            if (isOpen)
            {
                wayPoint.SetActive(false);
                isOpen = false;
            }
            else
            {
                wayPoint.SetActive(true); 
                isOpen = true;
            }
        }

    }
}
