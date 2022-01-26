using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Grid Round", menuName = "Gameshow Data/Rounds/New Grid Round", order = 1)]
public class GridRoundData : RoundData
{
    [Header("Grid Round Properties")]

    public GameObject gridButtonPrefab;

    [Header("Grid Settings")]

    [Range(1, 20)]
    public int gridWidth, gridHeight;

    public override void OnValidate()
    {
        //Cache old questions that we want to keep (non-null)
        List<Question> oldQuestions = new List<Question>(roundQuestions.Where(x => x != null));
        //Calculate the newly validated capacity
        int capacity = gridWidth * gridHeight;
        //Create a new list with said capacity
        roundQuestions = new List<Question>(new Question[capacity].ToList());
        //Add old questions back in
        roundQuestions.InsertRange(0, oldQuestions);
        //Truncate back to our new capacity
        roundQuestions.RemoveRange(capacity, (roundQuestions.Count - 1) - (capacity - 1));
    }
    public override void InitialiseRound()
    {
        //Do base initialise (creating round template)
        base.InitialiseRound();

        //Get the grid layout group
        GridLayoutGroup gridLayoutGroup = roundInstance.GetComponentInChildren<GridLayoutGroup>();

        //An elegant way of maintaining proper proportions
        //Set the layout groups column contraint to the grid width
        //The grid height will then always be correct as it maintains the proper question count

        //Make sure we are in fixed column count mode
        gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        //Set the column count to grid width
        gridLayoutGroup.constraintCount = gridWidth;

        //Check for a layoutgroup null.
        if (gridLayoutGroup == null)
        {
            Debug.Log("Layout group is null");
            return;
        }

        for (int i = 0; i < (gridWidth * gridHeight); i++)
        {
            //Spawn a new grid button into the grid object
            Button newGridButton;
            Image buttonImage;
            QuestionServeButton newQuestionServer;

            newGridButton = Instantiate(gridButtonPrefab, Vector3.zero, Quaternion.Euler(Vector3.zero), gridLayoutGroup.transform).GetComponent<Button>();

            newGridButton.transform.localPosition = Vector3.zero;
            newGridButton.transform.localEulerAngles = Vector3.zero;

            newGridButton.GetComponentInChildren<TextMeshProUGUI>().text = "";

            newQuestionServer = newGridButton.GetComponent<QuestionServeButton>();
            buttonImage = newGridButton.GetComponent<Image>();

            newQuestionServer.question = roundQuestions[i];

            newQuestionServer.timeAllowance = roundResponseAllowance;

            buttonImage.sprite = roundQuestions[i].questionIcon;
        }
    }

    public override void BeginRound()
    {
        base.BeginRound();
    }




}
