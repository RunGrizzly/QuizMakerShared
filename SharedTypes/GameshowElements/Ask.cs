using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Nakama.TinyJson;
using UnityEngine;

[Serializable]
public class Ask
{
    // [Header("Question Data")]
    public Question question;
    public float timeAllowance;
    public float elapsed;

    [HideInInspector]
    public List<Contestant> respondants;
    public ResponseBoolDictionary responses;
    public bool isInitialised = false;
    public bool isPaused = false;

    #region Constructors
    //Construct new ask from parameters
    public Ask(Question _question, List<Contestant> _contestants, float _timeAllowance, bool serve)
    {
        elapsed = 0;
        timeAllowance = _timeAllowance;
        question = _question;
        respondants = new List<Contestant>(_contestants);

        isInitialised = true;

        if (serve) Brain.ins.eventManager.e_newRoutine.Invoke(ServeAsk(question, timeAllowance));


        Debug.Log("An ask was opened with " + _contestants.Count + " respondants.");

        Serialise();
    }

    //Copy a new ask and reconstruct it
    public Ask(Ask askTemplate, bool serve)
    {
        timeAllowance = askTemplate.timeAllowance;
        elapsed = askTemplate.elapsed;
        question = askTemplate.question;
        respondants = askTemplate.respondants;
        // responses = askTemplate.responses;
        responses = askTemplate.responses;


        isInitialised = true;

        responses = new ResponseBoolDictionary();

        Serialise();
    }
    #endregion



    void Serialise()
    {
        //Whenever an ask is created. Serialise it to a json file.
        string savedAsk = JsonUtility.ToJson(this, true);
        savedAsk += "\n";
        savedAsk += JsonUtility.ToJson(question, true);
        System.IO.File.WriteAllText(Application.dataPath + "/AskLog/savedAsk" + decimal.Truncate((decimal)Time.time * 1000) + ".json", savedAsk);
    }

    public void ToggledPaused()
    {
        isPaused = !isPaused;
        // question.Timer().animator.speed = BooleanHelpers.BooleanToInt(!isPaused);
        question.Timer().animator.speed = BooleanHelpers.BooleanToInt(!isPaused);
    }


    public IEnumerator ServeAsk(Question newQuestion, float timeToRespond)
    {
        //Wait for a clean frame
        yield return new WaitForEndOfFrame();

        Debug.Log("A new ask was served");

        //Initialise the ask-------
        //Set elapsed to zero
        elapsed = 0;
        //Get a new gameobject instance of the question
        question.GetNewInstance(2);
        //Initialise the question timer
        question.Timer().Initialise(timeToRespond);
        //-------------------------

        //Iterate through all of the issued respondants
        //Add a new response for each respondant
        // responses = (question.CreateResponses(respondants, timeToRespond));
        responses = new ResponseBoolDictionary();

        foreach (Response response in question.CreateResponses(respondants, timeToRespond))
        {
            responses.Add(response, false);
        }

        Debug.Log("Response tracker has " + responses.Count + " responses in it.");

        Brain.ins.eventManager.e_responseConfirmed.AddListener
        (
        (Response response) =>
        {
            Debug.Log("A response was confirmed.");
            responses[response] = true;
        }
        );

        //Wait for a clean frame
        yield return new WaitForEndOfFrame();

        while (elapsed < timeToRespond && responses.Any(x => x.Value == false))
        {
            if (isPaused) yield return new WaitUntil(() => !isPaused);

            //Update the visual timer object
            if (question.Timer() != null) question.Timer().Refresh(elapsed);

            //Tick is not paused
            elapsed += Time.deltaTime;

            //Continue the enumeration
            yield return null;

        }

        Debug.Log("Ask event was ended.");

        //Stop listening for responses
        Brain.ins.eventManager.e_responseConfirmed.RemoveAllListeners();

        //Call the close event and pass this ask
        Brain.ins.eventManager.e_closeAsk.Invoke(this);
    }



}
