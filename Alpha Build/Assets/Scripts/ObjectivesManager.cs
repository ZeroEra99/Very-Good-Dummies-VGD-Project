using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class ObjectivesManager : MonoBehaviour
{
    public Animator animator;
    public Objective[] objectives;
    public TextMeshProUGUI _objectivesTitle;
    public TextMeshProUGUI _objectivesText;
    public bool objectivesCompleted;

    private void Awake()
    {
        GameManager.OnInteraction += finishObjectives;
        GameManager.OnEnemyKill += finishObjectives;
        GameManager.OnCollection += finishObjectives;
        GameManager.OnCheckpointReached += showCurrentObjectives;
    }

    private void OnDestroy()
    {
        GameManager.OnInteraction -= finishObjectives;
        GameManager.OnEnemyKill -= finishObjectives;
        GameManager.OnCollection -= finishObjectives;
        GameManager.OnCheckpointReached -= showCurrentObjectives;
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        _objectivesTitle = GameObject.Find("ObjectivesTitle").GetComponent<TextMeshProUGUI>();
        _objectivesText = GameObject.Find("ObjectivesText").GetComponent<TextMeshProUGUI>();
        objectives = FindObjectsOfType<Objective>();
        objectivesCompleted = true;
        showCurrentObjectives(PlayerManager.LastCheckpoint);
    }
    private void Update()
    {
        //this should only be called after relevant actions (enemy dies, dialogue ends etc.)
        // this script needs to know which level is currently being played
        // showCurrentObjectives(PlayerManager.LastCheckpoint);
    }

    public void showCurrentObjectives(Transform checkpoint) // looks through all objectives and displays the ones that are not finished and from the current checkpoint (or displays nothing if there are none)
    {
        objectives = FindObjectsOfType<Objective>();
        _objectivesTitle.text = "Objectives";
        string objectivesText = "";
        int counter = 1;
        for (int i = 0; i < objectives.Length; i++)
        {
            if (objectives[i].connectedCheckpoint == checkpoint && !objectives[i].finished)
            {
                objectivesText += counter + ". " + objectives[i].objectiveName + "\n";
                counter++;
            }
        }
        if (objectivesText != "")
        {
            _objectivesText.text = objectivesText;
            animator.SetBool("IsOpen", true);
            objectivesCompleted = false;
        }
        else
        {
            animator.SetBool("IsOpen", false);
            objectivesCompleted = true;
        }

    }

    public void finishObjectives(GameObject Object)
    {
        Objective targetObjective = Object.GetComponent<Objective>();
        if (targetObjective != null)
        {
            targetObjective.objectiveFinished();
        }
        showCurrentObjectives(PlayerManager.LastCheckpoint);
    }

}
