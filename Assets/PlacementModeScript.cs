using UnityEngine;
using System.Collections;

public class PlacementModeScript : MonoBehaviour {

    /* Toggles waypoint placement mode, if toggleMode is 0 then waypoint will be placed 1 meter 
     * in front of you. If toggleMode is 1, then it will be placed some distance above the cursor.
     */
      
    Main main;
    int toggleMode = 0;
    int optionSize = 2;
    TextMesh toggleText; 

	void Update () {

	    if (main == null)
        {
            GameObject gameManager = GameObject.Find("GameManager");
            main = gameManager.GetComponent<Main>();

        }
        if (toggleText == null)
        {
            toggleText = GameObject.Find("ToggleText").GetComponent<TextMesh>();
        }
    }

    void OnSelect()
    {
        toggleMode = (toggleMode + 1) % optionSize;
        if (toggleMode == 1)
        {
            main.floorPlacement = true;
            toggleText.text = "Toggle [B]";
        } if (toggleMode == 0)
        {
            main.floorPlacement = false;
            toggleText.text = "Toggle [A]";

        }
    }


}


