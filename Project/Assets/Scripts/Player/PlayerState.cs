using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerState : MonoBehaviour
{
    private static PlayerState _Instance;
    public static PlayerState Instance { get => _Instance; }

    private const int MAX_CHEATS = 8;

    [SerializeField, Tooltip("In the order of the enum. The enum will be used for indexing.")]
    private Cheat[] _Cheats;
    private List<CheatType> _AvailableCheats = new();

    private int _CheatsUsed;
    public int CheatsUsed { get => _CheatsUsed; }


    private void Awake()
    {
        if (_Instance && _Instance != this)
            Destroy(_Instance.gameObject);

        _Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    

    public void GainCheat(CheatType cheat)
    {
        if (_AvailableCheats.Count == MAX_CHEATS)
            return;

        _AvailableCheats.Add(cheat);
        UI.Instance.UpdateCheatList(_Cheats[(int)cheat].Icon);
    }


    // index in the _AvailableCheats list
    public void UseCheat(int index)
    {
        CheatType cheatType = _AvailableCheats[index];
        _AvailableCheats.RemoveAt(index);

        Cheat cheat = _Cheats[(int)cheatType];
        cheat.OnCheatExecuted?.Invoke(cheat.RiskChance);

        _CheatsUsed++;
    }
}