using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


//TODO: Move ask service off of rounds (make static?)
[Serializable]
public class Round
{
    public string name;

    public RoundData round;
    public List<Ask> roundAsks;
    public Ask currentAsk;
    public QuestionBoolDictionary questionTracker;

    public Round(RoundData _round)
    {
        round = _round;
        name = _round.roundName;
        roundAsks = new List<Ask>();
        questionTracker = new QuestionBoolDictionary();

        foreach (Question question in _round.roundQuestions)
        {
            questionTracker.Add(question, false);
        }

        SetActive();
    }

    // public void OpenAsk(Question newQuestion, float timeAllowance, List<Contestant> respondants)
    // {
    //     if (currentAsk != null && currentAsk.isInitialised)
    //     {
    //         Debug.Log("An old ask was closed.");
    //         Brain.ins.eventManager.e_closeAsk.Invoke(currentAsk);
    //     }

    //     Debug.Log("A new ask was opened.");
    //     currentAsk = new Ask(newQuestion, respondants, timeAllowance, true);

    //     roundAsks.Add(currentAsk);

    // }

    // public void CloseAsk(Ask closedAsk)
    // {
    //     if (currentAsk != closedAsk) return;
    //     //Fade and destroy the object
    //     currentAsk.question.KillInstance();

    //     //Find out what to do next
    //     //If the round style is 'as we go' 
    //     //Correct each question when it is closed
    //     if (round.roundCorrectionStyle == CorrectionStyle.AsWeGo)
    //     {
    //         Brain.ins.eventManager.e_goCorrectionSegment.Invoke(new List<Ask>() { currentAsk }, false);
    //     }

    //     else if (round.roundCorrectionStyle == CorrectionStyle.AtRoundEnd)
    //     {

    //         questionTracker[currentAsk.question] = true;


    //         if (questionTracker.Any(x => x.Value == false)) Brain.ins.eventManager.e_goRoundSegment.Invoke();
    //         else Brain.ins.eventManager.e_goCorrectionSegment.Invoke(roundAsks, true);
    //     }

    //     //Make our current ask null
    //     currentAsk = null;
    // }

    // public void CorrectAsks(List<Ask> correctionList, bool isRoundEnd)
    // {
    //     //Go to an ask and show the contestant responses
    //     Debug.Log("Initialised a correction event.");
    //     Correction newCorrection = new Correction(correctionList, isRoundEnd);

    // }


    public void SetActive()
    {

        //Build the round from the round data
        round.InitialiseRound();

        //TODO: The ask server should be global and accessible by any round that wants it
        // Brain.ins.eventManager.e_openAsk.AddListener(OpenAsk);
        // Brain.ins.eventManager.e_closeAsk.AddListener(CloseAsk);
        // Brain.ins.eventManager.e_goCorrectionSegment.AddListener(CorrectAsks);
    }

    public void SetInactive(bool destroyInstance)
    {
        //Close whatever ask we have open
        //Close the current ask
        if (currentAsk != null && currentAsk.isInitialised) Brain.ins.eventManager.e_closeAsk.Invoke(currentAsk);


        // //Remove listeners
        // Brain.ins.eventManager.e_openAsk.RemoveListener(OpenAsk);
        // Brain.ins.eventManager.e_closeAsk.RemoveListener(CloseAsk);
        // Brain.ins.eventManager.e_goCorrectionSegment.RemoveListener(CorrectAsks);

        //Destroy the current round instance
        if (destroyInstance) GameObject.DestroyImmediate(round.roundInstance);

    }

}