using System;
using System.Collections.Generic;
using UnityEngine;



[Serializable]
public class Gameshow //A gameshow instance that is controlled by passed gameshow data
{

    public Segment segment;

    public List<Contestant> contestants = new List<Contestant>();

    public GameshowData gameshowData; //The gameshow that this episode is an instance of

    public int currentRoundIndex;
    // public QuestionBoolDictionary questionTracker;
    //Each round will be stored as it is instanced, and asks associated with that round
    //Will be stored as its value pair
    public List<Round> rounds = new List<Round>();

    public bool isInitialised = false;

    //Constructor called when a new episode is made
    //It takes in a gameshow which contains round and question data
    public Gameshow(GameshowData _gameshowData)
    {
        Debug.Log("A new gameshow episode was started");
        Debug.Log("The gameshow in the episode is: " + _gameshowData.gameshowName);

        //Initiate the new episode here.
        gameshowData = _gameshowData;
        //Initialise round index
        currentRoundIndex = 0;

        //Begin a new match on the server
        //TODO: Initialise match state here (round data, contestants etc)
        Brain.ins.eventManager.e_newRoutine.Invoke(Brain.ins.serverControls.CreateMatch());

        //Subscribe to the new round event
        Brain.ins.eventManager.e_newRound.AddListener(PlayRound);
        //Point management
        Brain.ins.eventManager.e_addPoints.AddListener(AddPoints);
        Brain.ins.eventManager.e_removePoints.AddListener(RemovePoints);

        //Set the initialised flag to true
        isInitialised = true;
    }

    public void AddPoints(Contestant contestant, float pointChange)
    {
        contestant.points += pointChange;
    }

    public void RemovePoints(Contestant contestant, float pointChange)
    {
        contestant.points -= pointChange;
    }

    public void PlayRound(int newRoundIndex)
    {
        Debug.Log("Initiating New Round");
        Debug.Log("New round index is " + newRoundIndex);

        if (gameshowData.rounds.Count - 1 < newRoundIndex)
        {
            Debug.Log("Gameshow does not have the minimum number of rounds (1)" + ")");
            return;
        }

        if (newRoundIndex < 0)
        {
            Debug.Log("Tried to instantiate an invalid round index (" + newRoundIndex + ")");
            return;
        }

        //Deactivate the tracking of the current tracker
        if (CurrentRound() != null) CurrentRound().SetInactive(true);

        //Set the current round index
        currentRoundIndex = newRoundIndex;

        //Begin the initial round by constructing an instance of it
        rounds.Add(new Round(FetchRoundData(currentRoundIndex)));

    }

    public Round CurrentRound()
    {
        //If we have initialised any rounds so far
        //Return the current round 
        if (rounds.Count > 0) return rounds[rounds.Count - 1];
        //Or return null
        else
        {
            Debug.Log("No round info has been created.");
            return null;
        }
    }

    //Purge old episode info
    public void EndGameshow()
    {
        //If we currently have a round tracker
        //Set it to inactive (kill it)
        if (CurrentRound() != null) CurrentRound().SetInactive(true);
        Brain.ins.eventManager.e_newRound.RemoveListener(PlayRound);
    }

    public RoundData FetchRoundData(int roundIndex)
    {
        Debug.Log("Fetching round data");
        return gameshowData.rounds[roundIndex];
    }
}
