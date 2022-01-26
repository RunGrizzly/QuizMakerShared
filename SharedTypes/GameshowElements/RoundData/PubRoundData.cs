using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Pub Round", menuName = "Gameshow Data/Rounds/New Pub Round", order = 1)]
public class PubRoundData : RoundData
{
    [Header("Pub Round Properties")]
    public GameObject listButtonTemplate;


    [Header("List Settings")]

    [Range(0, 50)]
    public int questionAmount;

    public override void OnValidate()
    {
        //Cache old questions that we want to keep (non-null)
        List<Question> oldQuestions = new List<Question>(roundQuestions.Where(x => x != null));
        //Create a new list with said capacity
        roundQuestions = new List<Question>(questionAmount);
        //Add old questions back in
        roundQuestions.InsertRange(0, oldQuestions);
        //Truncate back to our new capacity
        roundQuestions.RemoveRange(questionAmount, (roundQuestions.Count - 1) - (questionAmount - 1));
    }

    public override void InitialiseRound()
    {
        //Do base initialise (creating round template)
        base.InitialiseRound();

        //Get the grid layout group
        GridLayoutGroup gridLayoutGroup = roundInstance.GetComponentInChildren<GridLayoutGroup>();

        //Check for a layoutgroup null.
        if (gridLayoutGroup == null)
        {
            Debug.Log("Layout group is null");
            return;
        }

        for (int i = 0; i < questionAmount; i++)
        {
            //Spawn a new grid button into the grid object
            Button newButton;
            // Image buttonImage;
            QuestionServeButton newQuestionServer;

            newButton = Instantiate(listButtonTemplate, Vector3.zero, Quaternion.Euler(Vector3.zero), gridLayoutGroup.transform).GetComponent<Button>();

            newButton.transform.localPosition = Vector3.zero;
            newButton.transform.localEulerAngles = Vector3.zero;

            newButton.GetComponentInChildren<TextMeshProUGUI>().text = "Question " + i;

            newQuestionServer = newButton.GetComponent<QuestionServeButton>();
            //buttonImage = newButton.GetComponent<Image>();

            newQuestionServer.question = roundQuestions[i];

            newQuestionServer.timeAllowance = roundResponseAllowance;

            //buttonImage.sprite = listElements[i].icon;
        }
    }

    public override void BeginRound()
    {
        base.BeginRound();
    }


}
