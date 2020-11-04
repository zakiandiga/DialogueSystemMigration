using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;
using Pathfinding;


public class NpcBehaviorWithPath : MonoBehaviour
{
/*
    public GameObject dialogueManager;
    

    GameObject sign;
    PlayerInput input;
    GameObject dialogue;
    DialogueDisplay dialogueDisplay;
    DialogueHolder dialogueHolder;
    QuestHolder questHolder;

    public Transform target;
    public float speed = 200f;
    public float nextWayPointDistance = 1f;

    Path path;
    private int currentWaypoint = 0;
    private bool reachEnd = false;

    Seeker seeker;
    Rigidbody2D rb;
    Animator anim;

    private NPCState npcMode = NPCState.idle;

    //To be replaced
    [SerializeField] int myLevel = 0;
    [SerializeField] int myQuestLevel = 0;
    int dialogueHolderIndex = 0;
    int questHolderIndex = 0;
    bool onQuest = false;  //cheat
    bool isOffended = false;
    bool isQuestFail = false; //Exclusive for Briar
    //End line

    public static event Action<NpcBehavior> OnTalkStart; //Announce if the NPC innitiate a talk at Talk()
    public static event Action<NpcBehavior> OnQuestCheck;

    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        
        dialogueHolderIndex = myLevel;

        sign = transform.Find("Sign").gameObject;
        dialogue = GameObject.Find("Dialogue");
        anim = GetComponentInChildren<Animator>();

        
        //dialogue = dialogueManager.GetComponent<DialogueManager>();

        /* to be replaced
        dialogueDisplay = dialogue.GetComponent<DialogueDisplay>();
        dialogueHolder = GetComponent<DialogueHolder>();
        questHolder = GetComponent<QuestHolder>();
        */
    /*
            input = GetComponent<PlayerInput>();

            DialogueDisplay.OnStartConversation += DisableInput; //Observe if the dialogue ends, enable input
            DialogueDisplay.OnEndtoNothing += EnableInput; //Observe if the dialogue starts, disable input
            RoomState.OnRoomLevelUp += NpcLevelup; //Observe if the room level up

            /* To be replaced
            RoomState.OnQuesting += QuestSetup;
            RoomState.OnQuestFail += QuestFailState;
            RoomState.OnQuestSuccess += QuestSuccess;
            DialogueDisplay.OnQuestCheck += CheckQuest;
            //////

            DialogueDisplay.OnOffendedCheck += CheckOffended;
        }

        void OnPathComplete(Path p)
        {
            if(!p.error)
            {
                path = p;
                currentWaypoint = 0;
            }
        }

        void FindPath()
        {
            //this seeker should be executed on GOAP
            seeker.StartPath(rb.position, target.position, OnPathComplete); 
            //start position, end position, function to call after path is created
        }

        void UpdatePath() //call this function to move the npc!
        {
            if(seeker.IsDone())
                seeker.StartPath(rb.position, target.position, OnPathComplete); //start position, end position, function to call after path is created

        }

        private void CheckOffended (DialogueDisplay d) //cheat
        {
            string whoTalk = dialogueDisplay.conversation.convoOwner;
            if (this.gameObject.name == whoTalk)
            {
                this.isOffended = true;
                this.questHolderIndex = 4; //cheat
                this.dialogueHolderIndex = 4;

            }
        }

        void OnTriggerEnter2D (Collider2D col)
        {
            if(col.gameObject.tag == "Player" && npcMode == NPCState.idle)
            {
                npcMode = NPCState.readyToTalk;
                sign.SetActive(true);            
                //Debug.Log(gameObject.name + " ready to talk!");            
            }
        }

        void OnTriggerExit2D (Collider2D col)
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
            //Debug.Log(gameObject.name + " if anybody need my cue, I'm talking now!");
            if (OnTalkStart != null)
            {
                OnTalkStart(this);           
            }

            if (isQuestFail && this.gameObject.name == " Briar_Bartender")
            {
                dialogueDisplay.conversation = dialogueHolder.conversation[5]; //cheat
                dialogueDisplay.StartConversation();
            }

            if (isOffended)
            {
                dialogueDisplay.conversation = dialogueHolder.conversation[5]; //cheat
                dialogueDisplay.StartConversation();
            }

            if (!onQuest && !isOffended)
            {
                dialogueDisplay.conversation = dialogueHolder.conversation[dialogueHolderIndex];
                dialogueDisplay.StartConversation();
            }

            if (onQuest && !isOffended)
            {
                dialogueDisplay.conversation = questHolder.questConversation[questHolderIndex];
                dialogueDisplay.StartConversation();
            }

            //Debug.Log(dialogueHolderIndex);
            input.enabled = false;        
        }

        private void DisableInput(DialogueDisplay d)
        {
            //Debug.Log(gameObject.name + " Alright DialogueManager, we disable our Player Input!");
            input.enabled = false;
        }

        private void EnableInput(DialogueDisplay d)
        {
            //Debug.Log(gameObject.name + " read notif from dialogue manager, we can initiate talk again");
            Invoke("EnablingInput", 1); //cheat the input enable

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

        private void NpcLevelup(RoomState r)
        {
            myLevel += 1;
            OnMyLevelup();
        }

        private void OnMyLevelup()
        {
            //For now, just add the dialogueHolderIndex
            dialogueHolderIndex += 1;

        }

        public void TestInput(InputAction.CallbackContext con) //press P to test the function
        {
            if(con.started)
            {
                FindPath();
            }
        }
        public enum NPCState
        {
            readyToTalk,
            talking,
            idle,
            walking,
            offended
        }

        /* to be replaced
        private void QuestFailState (RoomState r)
        {
            myLevel = 5;
            dialogueHolderIndex = 5;
            isQuestFail = true;
            onQuest = false;
        }

        private void QuestSuccess(RoomState r)
        {
            onQuest = false;
        }

        private void QuestSetup(RoomState r)
        {
            questHolderIndex = 0;
            onQuest = true;
        }

        private void CheckQuest(DialogueDisplay d) //Cheat
        {
            Debug.Log("Observed OnQuestCheck");
            string whoTalk = dialogueDisplay.conversation.convoOwner;
            Debug.Log(whoTalk);
            if (this.gameObject.name == whoTalk)
            {
                this.myQuestLevel += 1;
                Debug.Log(this.gameObject.name + " Level Up Quest");
                this.questHolderIndex += 1;
                if(OnQuestCheck != null)
                {
                    OnQuestCheck(this);
                }
            }
        }
        ////////

        void OnDestroy()
        {
            DialogueDisplay.OnStartConversation -= DisableInput; //Observe if the dialogue ends, enable input
            DialogueDisplay.OnEndtoNothing -= EnableInput; //Observe if the dialogue starts, disable input
            RoomState.OnRoomLevelUp -= NpcLevelup; //Observe if the room level up

            /* To Be Replaced
            RoomState.OnQuesting -= QuestSetup;
            RoomState.OnQuestFail -= QuestFailState;
            RoomState.OnQuestSuccess -= QuestSuccess;
            DialogueDisplay.OnQuestCheck -= CheckQuest;
            //////
            DialogueDisplay.OnOffendedCheck -= CheckOffended;
        }


        private void FixedUpdate()
        {
            if (path == null)
            {
                Debug.Log("Path is null");
                return;
            }            

            if(currentWaypoint >= path.vectorPath.Count) //if reach the last path
            {
                reachEnd = true;
                Debug.Log(reachEnd);
                return;
            }
            else //if there is still path on queue (or not interupted)
            {
                reachEnd = false;
                Debug.Log(reachEnd);
            }

            Vector2 dir = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
            Vector2 force = dir * speed * Time.deltaTime; 

            rb.AddForce(force);
            //Debug.Log(force);


            float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

            if (distance < nextWayPointDistance)
            {
                currentWaypoint++;
            }
        }
    */
}
