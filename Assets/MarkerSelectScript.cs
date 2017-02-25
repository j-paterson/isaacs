using UnityEngine;
using System.Collections;

public class MarkerSelectScript : MonoBehaviour {


    /* Implements the sequence number change of waypoints and deletion of waypoints
     */ 
    Main main;
    GameObject marker;

	void Start () {
	    main = GameObject.Find("GameManager").GetComponent<Main>();
        marker =  transform.parent.parent.gameObject;
    }

    void OnWaypointSelect()
    {
        
        if (gameObject.name == "MoveFront")
        {
            main.moveUpWayPoint(marker); 
        }
        else if (gameObject.name == "MoveBack")
        {
            main.debugText.text = "29";
            main.moveDownWayPoint(marker);
        }
        else if (gameObject.name == "Delete")
        {
            main.deleteWaypointMarker(marker);
        }
    }
}
