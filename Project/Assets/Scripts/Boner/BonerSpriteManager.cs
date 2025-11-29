using System.Collections;
using UnityEngine;

public class SpriteSequenceManager : MonoBehaviour
{
    [SerializeField] public SpriteRenderer _spriteRenderer;
    [SerializeField] public Sprite[] _frames;
    [SerializeField] public float _swapTime =  0.5f;

    void Start()
    {
        StartCoroutine(RunSequence());
    }

    IEnumerator RunSequence()
    {
        while (true)
        {
            // Show frame 0 always
            _spriteRenderer.sprite = _frames[0];
            yield return new WaitForSeconds(_swapTime);

            // Conditional frame
            if (ConditionA())
            {
                _spriteRenderer.sprite = _frames[1];
                yield return new WaitForSeconds(_swapTime);
            }

            // Another conditional frame
            if (ConditionB())
            {
                _spriteRenderer.sprite = _frames[Random.Range(0, _frames.Length)];
                yield return new WaitForSeconds(_swapTime);
            }

            // Final frame always
            _spriteRenderer.sprite = _frames[^1];
            yield return new WaitForSeconds(_swapTime);
        }
    }


    bool ConditionA()
    {
        return Random.value < 0.2f;
    }

    bool ConditionB()
    {
        return Random.value > 0.2f;
    }
}
