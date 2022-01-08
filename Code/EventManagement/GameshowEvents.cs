using System.Collections.Generic;
using System.Collections;
using UnityEngine.Events;

public class NewEpisodeEvent : UnityEvent<int> { }

public class LobbySegmentEvent : UnityEvent { }
public class OpenAskEvent : UnityEvent<Question, float, List<Contestant>> { }
public class CloseAskEvent : UnityEvent<Ask> { }
public class NewRoundEvent : UnityEvent<int> { }
public class ScoresSegmentEvent : UnityEvent { }
public class CorrectionSegmentEvent : UnityEvent<List<Ask>, bool> { }
public class HubSegmentEvent : UnityEvent { }
public class RoundSegmentEvent : UnityEvent { }

public class ResponseConfirmedEvent : UnityEvent<Response> { }

//
//Scoring
public class AddPointsEvent : UnityEvent<Contestant, float> { }
public class RemovePointsEvent : UnityEvent<Contestant, float> { }

//
//Host controls
public class AddContestantEvent : UnityEvent<Device, string> { }
public class RevealAnswerEvent : UnityEvent { }
public class HideAnswerEvent : UnityEvent { }
public class ProgressGameshowEvent : UnityEvent { }

//
public class NewRoutineEvent : UnityEvent<IEnumerator> { }

public class AppQuitEvent : UnityEvent { }