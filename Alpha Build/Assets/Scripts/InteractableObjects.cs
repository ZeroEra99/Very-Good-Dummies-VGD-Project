using System;
using UnityEngine;

/*
 * Types of interactables:
 * Executes a later specified game event (Starts combat or sth)
 * Displays a Dialogue
 * Changes the players Stats in some way
 */

public enum InteractableType { dialogueType, gameEventType, uselessType };
public class InteractableObjects : MonoBehaviour
{
    public Animator animator;
    [SerializeField] GameManager.GameLevel interactableLevel;
    DialogueManager dialogueManager;
    Transform playerTransform;
    public InteractableType interactableType;
    public CreateDialogue dialogue;
    public float typingSpeed = 0.02f;
    public float radius = 3f;
    public KeyCode interactableKey = KeyCode.E; // Changeable
    public int drop = 0;
    bool isInteracting = false;


    private void Awake()
    {
        GameManager.OnGameStart += LoadProgress;
        GameManager.OnGameSave += SaveProgress;
    }

    private void OnDestroy()
    {
        GameManager.OnGameStart -= LoadProgress;
        GameManager.OnGameSave -= SaveProgress;

    }

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
        dialogueManager = FindObjectOfType<DialogueManager>();
        playerTransform = GameObject.Find("Player Body").GetComponent<Transform>();
    }

    void Update()
    {
        float distance = Vector3.Distance(playerTransform.position, this.transform.position);
        transform.LookAt(playerTransform);
        // Remove vertical rotation component
        Vector3 newRotation = transform.eulerAngles;
        newRotation.x = 0; // Reset X rotation (vertical rotation)
        transform.eulerAngles = newRotation;
        // If the player is close enough
        if (Input.GetKeyDown(interactableKey))
        {
            if (distance <= radius || isInteracting)
            {
                Interact();
                animator.SetBool("isTalking", true);
            }
            if(!isInteracting)
                animator.SetBool("isTalking", false);
        }
    }

    // This method is meant to be overwritten
    public virtual void Interact()
    {
        GameManager.PlayerInteracted(this.gameObject);
        switch (interactableType)
        {
            case InteractableType.dialogueType:
                if (isInteracting)
                {
                    isInteracting = dialogueManager.DisplayNextSentence();
                }
                else
                {
                    dialogueManager.StartDialogue(dialogue, typingSpeed, drop);
                    drop = 0;
                    isInteracting = dialogueManager.DisplayNextSentence();
                }
                break;
            case InteractableType.gameEventType:
                break;
            case InteractableType.uselessType:
                break;
        }
    }

    // to show a range Wire Sphere in the editor
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(this.transform.position, radius);
    }
    
    private void SaveProgress(GameManager.SaveType saveType)
    {
        PlayerPrefs.SetFloat(dialogue.name, drop);
        PlayerPrefs.Save();
    }
    private void LoadProgress(GameManager.GameLevel level)
    {
        if (level > interactableLevel)
        {
            drop = 0;
            SaveProgress(0);
        }
    }
}
