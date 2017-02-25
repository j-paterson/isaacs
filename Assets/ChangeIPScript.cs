using UnityEngine;
using System.Collections;

public class ChangeIPScript : MonoBehaviour {

    Main main;
    TextMesh debugText;
    // Use this for initialization
    ReconnectScript reconnectScript;
    TextMesh reconnectText;
    void Start()
    {
        main = GameObject.Find("GameManager").GetComponent<Main>();
        debugText = GameObject.Find("Debug").GetComponent<TextMesh>();
        reconnectScript = GameObject.Find("ReconnectPanel").GetComponent<ReconnectScript>();
        reconnectText = GameObject.Find("Channel").GetComponent<TextMesh>();
    }

    // Update is called once per frame
    void Update()
    {
        if (debugText == null)
        {
            debugText = GameObject.Find("Debug").GetComponent<TextMesh>();
        }
        if (reconnectScript == null)
        {
            reconnectScript = GameObject.Find("ReconnectPanel").GetComponent<ReconnectScript>();
        }
    }


    void OnSelect()
    {
        debugText.text = "change IP start";
        reconnectScript.currentHost = (reconnectScript.currentHost + 1) % reconnectScript.ipList.Count;
        reconnectText.text = "Change IP [" + reconnectScript.currentHost + "]";
        debugText.text = "change IP end";
    }
}
