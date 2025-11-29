using UnityEngine;
using UnityEngine.Events;


public enum CheatType
{
    CLEAR_CUP,      // one cup is see-through
    ADD_BALL,       // add a ball under one cup
    SWITCH_BALL,    // switch under which cup the ball is
}



[CreateAssetMenu(fileName = "Cheat", menuName = "ScriptableObjects/Cheat")]
public class Cheat : ScriptableObject
{
    public CheatType Type;

    public Sprite Icon;

    [Range(0, 100), Tooltip("The probability that you will get caught using this cheat")]
    public int RiskChance;

    public UnityEvent<int> OnCheatExecuted;
}
