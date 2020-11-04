using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem; //Input System
using System;

public class DialogueManager : MonoBehaviour
{
	public static SimpleConditionalConversation scc;

	public string charName;
	public GameObject nextButton;

	// NOTE: When you do not use the google sheet option, it is expecting the file
	// to be named "data.csv" and for it to be in the Resources folder in Assets.
	public bool useGoogleSheet = false;
	public string googleSheetDocID = "";

	public static event Action<DialogueManager> OnStartConversation;
	public static event Action<DialogueManager> OnEndConversation;
	public static event Action<DialogueManager> OnNextConversation;

	// Start is called before the first frame update
	void Start()
	{
		if (useGoogleSheet) {
			// This will start the asyncronous calls to Google Sheets, and eventually
			// it will give a value to scc, and also call LoadInitialHistory().
			GoogleSheetSimpleConditionalConversation gs_ssc = gameObject.AddComponent<GoogleSheetSimpleConditionalConversation>();
			gs_ssc.googleSheetDocID = googleSheetDocID;
		} else {
			scc = new SimpleConditionalConversation("data");
			LoadInitialSCCState();
		}
	}
	
	public static void LoadInitialSCCState()
	{
		// Example of setting the initial state:
		//scc.setGameStateValue("playerWearing", "equals", "Green shirt");
	}
	
	// Update is called once per frame
	void Update()
	{
		// An example of getting a line of dialogue.
		/*
	    if (Input.GetKeyDown(KeyCode.Space)) {
			Debug.Log(DialogueManager.scc.getSCCLine("Emma"));
		}
		*/

		// An example of modifying the state outside of the DialogueManager (e.g. you could put this in some
		// OnTriggerEnter or something)
		/*
	    if (Input.GetKeyDown(KeyCode.G)) {
			scc.setGameStateValue("playerWearing", "equals", "Green shirt");
		}
		*/
	}

	public void StartConversation()
	{
		if (OnStartConversation != null)
		{
			OnStartConversation(this);
		}

		Debug.Log(DialogueManager.scc.getGameStateValue("speaker"));
		Debug.Log((string)DialogueManager.scc.getGameStateValue("speaker") + ": " + DialogueManager.scc.getSCCLine(charName));
		//scc.questState = questState;
		

		nextButton.SetActive(true);

		if (OnNextConversation != null)
		{
			OnNextConversation(this);
		}
		//Retrive current dialogue data (quest state, speaker, line, effect)

		DisplayLine();



	}

	public void AdvanceConversation()
	{
		DisplayLine();
		if (scc.checkCondition("lastLine", "equals", 1))
		{
			//if (scc.checkCondition("lvlState", "equals", 1))
			//{
			//	scc.setGameStateValue("questState", "set", "Q2T2");
			//	Debug.Log(scc.getGameStateValue("questState"));
			//}
			EndConversation();
		}
		else 
		{
			//Debug.Log(DialogueManager.scc.getSCCLine(charName));
			Debug.Log((string)DialogueManager.scc.getGameStateValue("speaker") + ": " + DialogueManager.scc.getSCCLine(charName));
		}
	}

	public void EndConversation()
	{		
		if (OnEndConversation != null)
		{
			OnEndConversation(this);
		}

		scc.setGameStateValue("questState", "set", "Q2T2");
		scc.questState = (string)scc.getGameStateValue("questState");
		Debug.Log("Current questState = " + scc.getGameStateValue("questState"));

		nextButton.SetActive(false);
		scc.setGameStateValue("lastLine", "equals", 0);
		scc.setGameStateValue("lvlState", "equals", 0);
		

		//change quest state based on the last dialogue line effect

	}

	private void DisplayLine()
	{
		//Debug.Log("Displaying dialogue line");
		//Display speaker
		//Display line to UI
	}
}
