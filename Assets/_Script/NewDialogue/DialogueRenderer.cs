using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem; //Input System
using System;

public class DialogueRenderer : MonoBehaviour
{
    //PlayerInput input; //Input System
    
    public string charName;
    public string questState = "Q1T1";
    
    public int blockNumber; //get this number from Conversation

    public GameObject speakerLeft;
    public GameObject speakerRight;
    public GameObject questionManager;
    public GameObject nextButton;

    QuestionManager followupQuestion;

    private SpeakerUI speakerUILeft;
    private SpeakerUI speakerUIRight;

    private int activeLineIndex = 0;

    public static event Action<DialogueRenderer> OnStartConversation;
    public static event Action<DialogueRenderer> OnEndConversation;
    public static event Action<DialogueRenderer> OnNextConversation;

    //public ButtonControl buttonControl;

    public void StartConversation()
    {
        if(OnStartConversation != null)
        {
            OnStartConversation(this);
        }
        Debug.Log(DialogueManager.scc.getSCCLine(charName));
        DialogueManager.scc.questState = questState;
        Debug.Log(DialogueManager.scc.questState);
        
        
        nextButton.SetActive(true);
        if(OnNextConversation != null)
        {
            OnNextConversation(this);
        }
        //Retrive current dialogue data (quest state, speaker, line, effect)

        DisplayLine();
        


    }

    public void AdvanceConversation()
    {
        DisplayLine();
        Debug.Log(DialogueManager.scc.getSCCLine(charName));

        
    }

    public void EndConversation()
    {
        if(OnEndConversation != null)
        {
            OnEndConversation(this);
        }

        //change quest state based on the last dialogue line effect

    }

    private void DisplayLine()
    {
        Debug.Log("Displaying dialogue line");
        //Display speaker
        //Display line to UI
    }
}
