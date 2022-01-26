using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public enum CorrectionStyle { AtRoundEnd, AsWeGo }

public class RoundData : ScriptableObject
{
    [Header("Generic Round Properties")]


    public GameObject roundTemplate;

    [HideInInspector]
    public GameObject roundInstance;


    [Header("Generic Settings")]
    public string roundName;
    public float roundResponseAllowance;
    public CorrectionStyle roundCorrectionStyle;

    public List<Question> roundQuestions;

    public virtual void OnValidate()
    {
        Debug.Log("Validated list data.");
    }

    //Find the round panel and create an instance
    public virtual void InitialiseRound()
    {
        Debug.Log("A new round instance was created.");

        //Find the game canvas by tag
        PanelLayout roundPanel = Brain.ins.stageManager.layouts[1];

        //Instantiate the round template
        roundInstance = GameObject.Instantiate(roundTemplate, Vector3.zero, Quaternion.identity, roundPanel.transform) as GameObject;

        roundInstance.transform.localPosition = Vector3.zero;
        roundInstance.transform.localRotation = Quaternion.Euler(Vector3.zero);

        roundPanel.SetHeaderText(roundName);
    }

    public virtual void BeginRound()
    {
        //Begin round.
    }
}
