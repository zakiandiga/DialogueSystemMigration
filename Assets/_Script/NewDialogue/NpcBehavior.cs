using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;
using Pathfinding;

public class NpcBehavior : MonoBehaviour
{
    public GameObject dialogue;
    DialogueRenderer dialogueRenderer;
    public GameObject dialogueMaster;
    DialogueManager dialogueManager;
    
    [SerializeField] private string nPCName;

    GameObject sign;
    PlayerInput input;

    DialogueDisplay dialogueDisplay;

    /*
     * PATHFINDING
    public Transform target;
    public float speed = 200f;
    public float nextWayPointDistance = 1f;

    Path path;
    private int currentWaypoint = 0;
    private bool reachEnd = false;

    Seeker seeker;
    Rigidbody2D rb;

    */
    Animator anim;

    private NPCState npcMode = NPCState.idle;
    public enum NPCState
    {
        readyToTalk,
        talking,
        idle,
        walking,
        offended
    }

    public static event Action<NpcBehavior> OnTalkStart; //Announce if the NPC innitiate a talk at Talk()
    public static event Action<NpcBehavior> OnQuestCheck;

    void Start()
    {
        sign = transform.Find("Sign").gameObject;

        anim = GetComponentInChildren<Animator>();
        input = GetComponent<PlayerInput>();
        dialogueRenderer = dialogue.GetComponent<DialogueRenderer>();
        dialogueManager = dialogueMaster.GetComponent<DialogueManager>();

        DialogueManager.OnStartConversation += DisableInput; //Observe if the dialogue ends, enable input
        DialogueManager.OnEndConversation += EnableInput; //Observe if the dialogue starts, disable input
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player" && npcMode == NPCState.idle)
        {
            npcMode = NPCState.readyToTalk;
            sign.SetActive(true);
            //Debug.Log(gameObject.name + " ready to talk!");            
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player" && npcMode == NPCState.readyToTalk)
        {
            //Debug.Log(gameObject.name + " player too far to talk");
            npcMode = NPCState.idle;
            sign.SetActive(false);
        }
    }

    private void Talk()
    {
        //run function on dialoguerenderer
        dialogueManager.charName = this.nPCName;
        dialogueManager.StartConversation();
        //dialogueRenderer.charName = this.nPCName;
        //dialogueRenderer.StartConversation();
        //Debug.Log(DialogueManager.scc.getSCCLine(this.nPCName));
        //IMPLEMENT TALK BASED ON MIKE's DIALOGUE MANAGER
           

        //Debug.Log(dialogueHolderIndex);
        input.enabled = false;
    }

    private void DisableInput(DialogueManager d)
    {
        //Debug.Log(gameObject.name + " Alright DialogueManager, we disable our Player Input!");
        input.enabled = false;
    }

    private void EnableInput(DialogueManager d)
    {
        //Debug.Log(gameObject.name + " enabling input");
        Invoke("EnablingInput", 1.2f); //cheat the input enable

    }

    private void EnablingInput()
    {
        input.enabled = true;
    }

    public void InteractInput(InputAction.CallbackContext con)
    {
        if (con.started && npcMode == NPCState.readyToTalk)
        {
            Talk();
        }
    }
}
