using UnityEngine;
using UnityEngine.VR.WSA.Input;

public class GazeGestureManager : MonoBehaviour
{
    public static GazeGestureManager Instance { get; private set; }

    // Represents the hologram that is currently being gazed at.
    public GameObject FocusedObject { get; private set; }



    /* GazeGestureManager class is used for recognizing tapping and dragging events. TapState toggles between pressing
     * menu panels (ButtonPress) and placing waypoints (FreePlace). The selectedObject is the focused object that the 
     * Hololens is currently tapping on. timeStartFreePlace is the time when you first start tapping. If you tap twice
     * in less than the doubleTapTime milliseconds max threshold then it will be recognized as a double tap. Double tap 
     * listeners resets after doubleTapResetTime milliseconds from the first tap. 
     */ 
  
    public TextMesh debugText; 
    GestureRecognizer recognizer;
    // Use this for initialization
    GameObject selectedObject;
    Main main;

    TapState tapState = TapState.ButtonPress; 
    enum TapState
    {
        ButtonPress,
        FreePlace
    }
    float timeStartFreePlace;
    float doubleTapTime = .3f;
    float doubleTapResetTime = .6f;


    const int surfaceMapLayer = 31; 


    void Start()
    {
        Instance = this;
        main = GameObject.Find("GameManager").GetComponent<Main>();
        // Set up a GestureRecognizer to detect Select gestures.
        recognizer = new GestureRecognizer();
        recognizer.SetRecognizableGestures(GestureSettings.Tap | GestureSettings.ManipulationTranslate);




        
        recognizer.RecognitionEndedEvent += (source, ray) =>
        {
            if (selectedObject != null)
            {
                selectedObject.SendMessageUpwards("OnManipulationCompleted");
                selectedObject = null;
            }
        };

        /* Tapped events are events where the user taps and lets go
         * Manipulation events occur when the user taps and holds. 
         */
          
        recognizer.TappedEvent += (source, tapCount, ray) =>
        {
            // Send an OnSelect message to the focused object and its ancestors.
            if (FocusedObject != null && FocusedObject.layer != surfaceMapLayer)
            {
                FocusedObject.SendMessageUpwards("OnSelect");
                FocusedObject.SendMessage("OnWaypointSelect");
                FocusedObject.SendMessage("markerExpand");
                tapState = TapState.ButtonPress;
                
            }

            else
            {
                if (tapState == TapState.ButtonPress)
                {
                    tapState = TapState.FreePlace;
                    timeStartFreePlace = Time.time; 
                } else if (tapState == TapState.FreePlace && Time.time - timeStartFreePlace < doubleTapTime)
                {
                    
                    tapState = TapState.ButtonPress;
                    GameObject.Find("GameManager").SendMessageUpwards("OnDoubleTap");

                } else
                {

                    tapState = TapState.ButtonPress; 
                }
            }
            
        };

        recognizer.ManipulationStartedEvent += (source, relativePosition, ray) =>
        {
            
            if (FocusedObject != null)
            {
                FocusedObject.SendMessageUpwards("OnManipulationStart", relativePosition);
                selectedObject = FocusedObject;
            }

        };

        recognizer.ManipulationUpdatedEvent += (source, relativePosition, ray) =>
        {

            if (selectedObject != null)
            {
                selectedObject.SendMessageUpwards("OnManipulationUpdate", relativePosition);
            }
        };

        recognizer.ManipulationCompletedEvent += (source, relativePosition, ray) =>
        {
            selectedObject.SendMessageUpwards("OnManipulationCompleted"); 
            selectedObject = null;
           
        };

        recognizer.ManipulationCanceledEvent += (source, relativePosition, ray) =>
        {
            if (selectedObject != null)
            {
                selectedObject.SendMessageUpwards("OnManipulationCompleted");
                selectedObject = null;
            }
        }; 

          recognizer.StartCapturingGestures();
    }

    /* Update constantly checks whether there is a focused object 
     * and starts the capturing any gestures made by the hand
     */
    void Update()
    {
        // Figure out which hologram is focused this frame.
        GameObject oldFocusObject = FocusedObject;

        // Do a raycast into the world based on the user's
        // head position and orientation.
        var headPosition = Camera.main.transform.position;
        var gazeDirection = Camera.main.transform.forward;

        RaycastHit hitInfo;
        if (Physics.Raycast(headPosition, gazeDirection, out hitInfo))
        {
            // If the raycast hit a hologram, use that as the focused object.
            FocusedObject = hitInfo.collider.gameObject;

        }
        else
        {
            // If the raycast did not hit a hologram, clear the focused object.
            FocusedObject = null;
        }

        // If the focused object changed this frame,
        // start detecting fresh gestures again.
 
        recognizer.StartCapturingGestures();

        if (!recognizer.IsCapturingGestures() && selectedObject !=null)
        {
            selectedObject.SendMessageUpwards("OnManipulationCompleted");
            selectedObject = null;
            recognizer.CancelGestures(); 
        }
        if (tapState == TapState.FreePlace && Time.time - timeStartFreePlace > doubleTapResetTime)
        {
            tapState = TapState.ButtonPress;
        }
    }




}
