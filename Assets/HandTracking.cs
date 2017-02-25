using UnityEngine;
using System.Collections;
using UnityEngine.VR.WSA.Input;
using System.Collections.Generic;

public class HandTracking : MonoBehaviour
{

    // Use this for initialization



    void Awake()
    {
        InteractionManager.SourceDetected += InteractionManager_SourceDetected;
        InteractionManager.SourceUpdated += InteractionManager_SourceUpdated;
        InteractionManager.SourceLost += InteractionManager_SourceLost;


    }


    private void InteractionManager_SourceDetected(InteractionSourceState state)
    {
       
    }



    private void InteractionManager_SourceUpdated(InteractionSourceState state)
    {
      
    }


    private void InteractionManager_SourceLost(InteractionSourceState state)
    {
       
    }

    void Update()
    {
    }

   
  
}
