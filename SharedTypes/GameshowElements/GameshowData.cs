using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "NewGameshow", menuName = "Gameshow Data/New Gameshow", order = 0)]
public class GameshowData : ScriptableObject
{

    public string gameshowName;

    [Header("Visual Elements")]
    public VisualStyleBank theme;

    [Header("Round Options")]
    public List<RoundData> rounds;

}
