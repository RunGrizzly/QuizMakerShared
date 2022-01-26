using System;
using System.Collections.Generic;
using Nakama.TinyJson;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
[CreateAssetMenu(fileName = "New True or False Question", menuName = "Gameshow Data/Questions/New True or False Question", order = 1)]
public class TrueOrFalse : Question
{

    public string pose;
    public string answer;


    [Header("Pose Info")]
    TextMeshProUGUI poseArea;
    CanvasGroup poseGroup;

    [Header("Answer Info")]
    TextMeshProUGUI answerArea;
    CanvasGroup answerGroup;

    public override GameObject GetNewInstance(int _panelIndex)
    {
        questionInstance = base.GetNewInstance(_panelIndex);
        SetupQuestion();
        return questionInstance;
    }

    public override void SetupQuestion()
    {
        //Get required components
        poseGroup = questionInstance.transform.Find("PoseGroup").GetComponent<CanvasGroup>();
        poseArea = questionInstance.transform.Find("PoseGroup/PoseArea").GetComponent<TextMeshProUGUI>();
        answerGroup = questionInstance.transform.Find("AnswerGroup").GetComponent<CanvasGroup>();
        answerArea = questionInstance.transform.Find("AnswerGroup/AnswerArea").GetComponent<TextMeshProUGUI>();

        //Make sure the pose text reflects the question data.
        poseArea.text = WrappedString(pose, 55);
        //Make sure the answer text reflects the question data.
        answerArea.text = WrappedString(answer, 55);
        //Make sure pose group is visible.
        poseGroup.alpha = 1;
        //Make sure the answer is not visible.
        answerGroup.alpha = 0;
        //Update canvas
        Refresh();
    }

    public override string AsJson()
    {
        var t = this;
        //
        //
        string tj = JsonWriter.ToJson(t);
        Debug.Log(tj);
        return tj;
    }

    string WrappedString(string newString, int maxLineLength)
    {

        int numberOfLines = newString.Length / maxLineLength;

        var result = "";

        for (int i = 0; i <= numberOfLines; i++)
        {

            string newLine = newString.Substring(i * maxLineLength);

            if (newLine.Length > maxLineLength) result += newString.Substring(i * maxLineLength, maxLineLength) + "\n";
            else result += newLine;
        }

        return result;

    }

    public override List<Response> CreateResponses(List<Contestant> _respondants, float _timeAllowance)
    {

        base.CreateResponses(_respondants, _timeAllowance);

        List<Response> responses = new List<Response>();

        foreach (Contestant respondant in _respondants)
        {
            responses.Add(new BoolResponse(respondant, this, _timeAllowance));
        }

        //Create response data
        return responses;
    }

    public override bool CompareResponse(ResponseData contestantResponse)
    {
        if (contestantResponse.responseBool != correctResponse.responseBool) return false;
        return true;
    }
}



