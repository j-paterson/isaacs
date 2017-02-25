using UnityEngine;
using System.Collections;

public class MarkerExpandScript : MonoBehaviour {

    /* Used for displaying the (decrement, delete, increment) option under waypoint markers. moveFront is the 
     * button that coorsponds to putting the waypoint sequence number ahead by 1. moveBack for putting it 
     * behind by 1. Delete button deletes the gameobject. 
     */ 

    bool isExpanded;

    GameObject moveFront;
    GameObject moveBack;
    GameObject delete; 

	void Start () {
        moveFront = transform.GetChild(1).GetChild(0).gameObject;
        moveBack = transform.GetChild(1).GetChild(1).gameObject;
        delete = transform.GetChild(1).GetChild(2).gameObject;
        moveFront.SetActive(false);
        moveBack.SetActive(false);
        delete.SetActive(false);

    }

    /* Called when the user presses on the waypoint. Displays an option panel below the waypoint
     */  
    void markerExpand()
    {
        if (!isExpanded)
        {
            isExpanded = true;
            moveFront.SetActive(true);
            moveBack.SetActive(true);
            delete.SetActive(true); 
        } else
        {

            isExpanded = false;
            moveFront.SetActive(false);
            moveBack.SetActive(false);
            delete.SetActive(false);     
        }
        
    }
}
