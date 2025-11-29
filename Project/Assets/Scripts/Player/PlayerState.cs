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
        if (_Instance == null)
            _Instance = this;
        else
            Destroy(this);
        
        DontDestroyOnLoad(this);
    }
    

    private void Start()
    {
        StartCoroutine(TestCheatSpawn());
    }


    private IEnumerator TestCheatSpawn()
    {
        for (int i = 0; i < 8; ++i)
        {
            if (i % 2 == 0)
                GainCheat(CheatType.CLEAR_CUP);
            else
                GainCheat(CheatType.ADD_BALL);

            yield return new WaitForSeconds(0.5f);
        }

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