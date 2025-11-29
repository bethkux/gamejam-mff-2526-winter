using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class SwapManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> _cupPrefabs = new List<GameObject>();
    [SerializeField] private float _padding = 1.0f;
    [SerializeField] private float _swapTime = 1.0f;
    [Space]
    [Space]
    [Space]
    [Space]
    [Header("Events")]
    [SerializeField] private UnityEvent OnSwappingFinished = new UnityEvent();
    [SerializeField] private UnityEvent OnRepositionFinished = new UnityEvent();
    
    
#if UNITY_EDITOR
    [ContextMenu("Test → Register Cup")]
    public void Editor_RegisterCup()
    {
        RegisterCup();
    }

    [ContextMenu("Test → Swap Cups")]
    public void Editor_Swap()
    {
        Swap(_cups.Count);
    }

    [ContextMenu("Test → Reveal Random Cup")]
    public void Editor_RevealCup()
    {
        if (_cups.Count > 0)
            RevealCup(_cups[Random.Range(0, _cups.Count)]);
    }
#endif

    
    private float _totalWidth = 0.0f;
    private List<Cup> _cups = new List<Cup>();
    private int _activeRepositionTweens = 0;
    private bool _isSwapping = false;
    
    
    
    
    public void Swap(int swapCount)
    {
        StartCoroutine(SwapRoutine(swapCount));
    }
    
    public void RegisterCup()
    {
        // Get random index of prefabs
        int idx =  Random.Range(0, _cupPrefabs.Count);
        
        //Create a cup instance
        GameObject cupInstance = Instantiate(_cupPrefabs[idx], new Vector3(0, 0, 0), Quaternion.identity);
        
        // Get the cup script     
        Cup cup = cupInstance.GetComponent<Cup>();
        if (!cup)
        {
            Debug.LogError(_cupPrefabs[idx].name + " is not a Cup");
            return;
        }
        
        RegisterCup(cup);
    }
    
    public void RemoveCup()
    {
        // Get random index of cup
        int idx =  Random.Range(0, _cups.Count);
        RemoveCup(_cups[idx]);
    }

    public void RevealCup(Cup cup)
    {
        Sequence seq = DOTween.Sequence();

        Vector3 startPos = cup.transform.position;
        Quaternion startRot = cup.transform.rotation;

        // Reveal target position & rotation
        Vector3 targetPos = startPos + new Vector3(0.5f, 1.5f, 0f);
        Quaternion targetRot = startRot * Quaternion.Euler(0, 0, -25);

        
        seq.Append(
            cup.transform.DOMove(targetPos, 0.3f)
                .SetEase(Ease.OutBack)
        );
        seq.Join(
            cup.transform.DORotateQuaternion(targetRot, 0.3f)
                .SetEase(Ease.OutBack)
        );
        
        seq.Append(
            cup.transform.DOPunchScale(new Vector3(0.1f, 0.1f, 0), 0.2f, 6, 0.25f)
        );

        // Wait before lowering the cup back
        seq.AppendInterval(0.5f);

        // Move back down & rotate back to original
        seq.Append(
            cup.transform.DOMove(startPos, 0.25f)
                .SetEase(Ease.InOutQuad)
        );
        seq.Join(
            cup.transform.DORotateQuaternion(startRot, 0.25f)
                .SetEase(Ease.InOutQuad)
        );
    }

        
    private void RegisterCup(Cup cup)
    {
        _cups.Add(cup);
        
        RecalculateWidth();
        TweenRepositionAllCups();
        Bounce(cup.transform);
    }
    
    private void RemoveCup(Cup cup)
    {
        if (!_cups.Contains(cup)) return;

        _cups.Remove(cup);

        cup.transform
            .DOPunchScale(new Vector3(-0.5f, -0.5f, 0f), 0.25f, 5, 0.5f)
            .OnComplete(() =>
            {
                cup.transform.DOScale(0f, 0.2f)
                    .SetEase(Ease.InBack)
                    .OnComplete(() =>
                    {
                        Destroy(cup.gameObject);

                        // After cup is destroyed, reposition others
                        RecalculateWidth();
                        TweenRepositionAllCups();
                    });
            });
    }
    
    private void RecalculateWidth()
    {
        _totalWidth = 0f;
        foreach (Cup c in _cups)
            _totalWidth += c.GetSize().x + _padding;
    }
    
    private void TweenRepositionAllCups() 
    {
        if (_cups.Count == 0) return; 
        float halfWidth = _totalWidth / 2f; 
        Vector3 middle = transform.position; 
        float cupPosition = middle.x - halfWidth; 
        _activeRepositionTweens = 0;   // reset global counter
       
        foreach (Cup c in _cups)
        {
            float width = c.GetSize().x; 
            Vector3 targetPos = new Vector3( cupPosition + width / 2f, middle.y, middle.z ); 
            _activeRepositionTweens++; 
            c.transform.DOMove(targetPos, 0.4f) .SetEase(GetRandomEase()) .OnComplete(() =>
            {
                Bounce(c.transform); 
                JitterHorizontal(c.transform); 
                _activeRepositionTweens--;
                if (_activeRepositionTweens == 0)
                {
                    OnRepositionFinished?.Invoke();
                }
            });
            cupPosition += width + _padding;
        } }
    
    private Ease GetRandomEase()
    {
        Ease[] eases =
        {
            Ease.OutQuad, Ease.OutCubic, Ease.OutQuint,
            Ease.InOutQuad, Ease.InOutCubic
        };
    
        int index = Random.Range(0, eases.Length);
        return eases[index];
    }
    
    private Tween Bounce(Transform t)
    {
       return t.DOPunchScale(new Vector3(0.08f, 0.08f, 0f), 0.15f, 6, 0.25f);
    }
    
    private Tween JitterHorizontal(Transform t, float strength = 0.08f, float duration = 0.15f)
    {
        return t.DOShakePosition(duration, new Vector3(strength, 0, 0), 20, 100);
    }
    
    private IEnumerator SwapRoutine(int swapCount)
    {
        if (_cups.Count <= 1 || _isSwapping)
            yield break;

        _isSwapping = true;
        
        for (int i = 0; i < swapCount; i++)
        {
            int idx1 = Random.Range(0, _cups.Count);
            int idx2 = Random.Range(0, _cups.Count);

            while (idx1 == idx2)
                idx2 = Random.Range(0, _cups.Count);

            yield return StartCoroutine(SwapCupsRoutine(_cups[idx1], _cups[idx2]));
        }
        
        _isSwapping = false;
        OnSwappingFinished?.Invoke();
    }
    
    private IEnumerator SwapCupsRoutine(Cup a, Cup b)
    {
        Vector3 posA = a.transform.position;
        Vector3 posB = b.transform.position;

        float halfTime = _swapTime / 2f;
        Ease ease = GetRandomEase();

        Tween t1 = a.transform.DOMove(posB + Vector3.up * 0.4f, halfTime).SetEase(ease);
        Tween t2 = b.transform.DOMove(posA + Vector3.up * 0.4f, halfTime).SetEase(ease);

        yield return DOTween.Sequence().Join(t1).Join(t2).WaitForCompletion();

        Tween t3 = a.transform.DOMove(posB, halfTime).SetEase(ease);
        Tween t4 = b.transform.DOMove(posA, halfTime).SetEase(ease);

        yield return DOTween.Sequence().Join(t3).Join(t4).WaitForCompletion();

        
        // Bounce + jitter at end
        Bounce(a.transform);
        Bounce(b.transform);
        
        JitterHorizontal(a.transform);
        JitterHorizontal(b.transform);
    }
}