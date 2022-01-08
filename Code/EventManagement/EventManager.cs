using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class EventManager : MonoBehaviour
{

    //Game Events
    public AppQuitEvent e_appQuit;

    //Gameshow control

    //Begin a new episode
    public NewEpisodeEvent e_newEpisode;


    public NewRoundEvent e_newRound;

    //Lobby segment
    public LobbySegmentEvent e_goLobbySegment;

    //Hub segment
    public HubSegmentEvent e_goHubSegment;

    //Round segment
    public RoundSegmentEvent e_goRoundSegment;

    //Ask segment
    public OpenAskEvent e_openAsk;
    public CloseAskEvent e_closeAsk;

    //Correction segment
    public CorrectionSegmentEvent e_goCorrectionSegment;

    //Show scores segment
    public ScoresSegmentEvent e_goScoresSegment;

    //Coroutine control
    public NewRoutineEvent e_newRoutine;

    public ResponseConfirmedEvent e_responseConfirmed;

    //Points Scoring
    public AddPointsEvent e_addPoints;
    public RemovePointsEvent e_removePoints;

    //Host Controls
    public RevealAnswerEvent e_revealAnswer;
    public HideAnswerEvent e_hideAnswer;

    public ProgressGameshowEvent e_progressGameshow;

    //Device and Contestant management
    public AddContestantEvent e_addContestant;

    void Awake()
    {
        //Game events.
        if (e_newEpisode == null) e_newEpisode = new NewEpisodeEvent();
        if (e_newRound == null) e_newRound = new NewRoundEvent();

        if (e_appQuit == null) e_appQuit = new AppQuitEvent();

        if (e_responseConfirmed == null) e_responseConfirmed = new ResponseConfirmedEvent();


        //Host controls 
        if (e_revealAnswer == null) e_revealAnswer = new RevealAnswerEvent();
        if (e_hideAnswer == null) e_hideAnswer = new HideAnswerEvent();
        if (e_progressGameshow == null) e_progressGameshow = new ProgressGameshowEvent();


        //Points Scoring
        if (e_addPoints == null) e_addPoints = new AddPointsEvent();
        if (e_removePoints == null) e_removePoints = new RemovePointsEvent();

        //

        if (e_addContestant == null) e_addContestant = new AddContestantEvent();

        if (e_goLobbySegment == null) e_goLobbySegment = new LobbySegmentEvent();
        if (e_goHubSegment == null) e_goHubSegment = new HubSegmentEvent();
        if (e_openAsk == null) e_openAsk = new OpenAskEvent();
        if (e_closeAsk == null) e_closeAsk = new CloseAskEvent();
        if (e_goCorrectionSegment == null) e_goCorrectionSegment = new CorrectionSegmentEvent();
        if (e_goScoresSegment == null) e_goScoresSegment = new ScoresSegmentEvent();
        if (e_goRoundSegment == null) e_goRoundSegment = new RoundSegmentEvent();


        if (e_newRoutine == null) e_newRoutine = new NewRoutineEvent();

    }





}