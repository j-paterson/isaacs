using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Windows.Speech;

/* SpeechManager script allows you to add your own voice commands. The current relevant command is Follow Me which will call
 * the OnFollowMe method in the main class. This will create a waypoint at the location of where the hololens is located. 
 * Keywords stores a list of active commands. 
 */ 
public class SpeechManager : MonoBehaviour
{
    KeywordRecognizer keywordRecognizer = null;
    Dictionary<string, System.Action> keywords = new Dictionary<string, System.Action>();
    public GameObject main;
    public Main mainScript;

    void Start()
    {
        keywords.Add("Reset world", () =>
        {
            mainScript.debugText.text = "Reset world";
        });
        keywords.Add("Follow Me", () =>
        {
            mainScript.debugText.text = "Follow Me!";
            main.SendMessage("OnFollowMe");
        });
        // Tell the KeywordRecognizer about our keywords.
        keywordRecognizer = new KeywordRecognizer(keywords.Keys.ToArray());

        // Register a callback for the KeywordRecognizer and start recognizing!
        keywordRecognizer.OnPhraseRecognized += KeywordRecognizer_OnPhraseRecognized;
        keywordRecognizer.Start();
    }

    private void KeywordRecognizer_OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        System.Action keywordAction;
  
        if (keywords.TryGetValue(args.text, out keywordAction))
        {
            keywordAction.Invoke();
        }
    }
}