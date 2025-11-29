using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Looking left and right -> he is cheating
// Looking up down -> normal

public class SpriteSequenceManager : MonoBehaviour
{
    enum LookingState : int
    {
        Down = 0,
        Left = 1,
        Right = 2,
        Up = 3
    }
    
    [SerializeField] public SpriteRenderer _spriteRenderer;
    [SerializeField] public Sprite[] _frames;
    [SerializeField] public float _swapTime =  0.8f;
    private List<LookingState> _states = new List<LookingState>();

#if UNITY_EDITOR
    [ContextMenu("Test → Enable Cheating")]
    public void Editor_EnableCheating() => EnableCheating();
    
    [ContextMenu("Test → Disable Cheating")]
    public void Editor_DisableCheating() => DisableCheating();
#endif
    
    public void EnableCheating()
    {
        _states.Clear();
        _states.Add(LookingState.Right);
        _states.Add(LookingState.Left);
    }

    public void DisableCheating()
    {
        _states.Clear();
        _states.Add(LookingState.Down);
        _states.Add(LookingState.Up);
    }

    void Start()
    {
        DisableCheating();
        StartCoroutine(RunSequence());
    }

    IEnumerator RunSequence()
    {
        while (true)
        {
            // Get state 
            LookingState state = _states[Random.Range(0, _states.Count)];
            _spriteRenderer.sprite = _frames[(int)state];
            yield return new WaitForSeconds(_swapTime);
            
        }
    }
    
}
