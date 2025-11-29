using System;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class Cup : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Sprite _transparentSprite;
    private Sprite _sprite;
    
#if UNITY_EDITOR
    [ContextMenu("Test → Make Transparent")]
    public void Editor_MakeTransparent()
    {
        MakeTransparent();
    }
#endif

    private void Start()
    {
        if (_spriteRenderer == null)
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            Debug.unityLogger.Log("SpriteRenderer", _spriteRenderer);
        }

        if (_spriteRenderer == null) Debug.LogError("Cup SpriteRenderer is missing");
        if(_transparentSprite == null) Debug.LogError("Cup SpriteRenderer is missing");
        
        //Store original sprite
        _sprite =  _spriteRenderer.sprite;
        
        //DOVirtual.DelayedCall(2f, () => MakeTransparent());
        //DOVirtual.DelayedCall(3f, () => ResetSprite());
    }
    
    public Vector2 GetSize()
    {
        return new Vector2(_spriteRenderer.bounds.size.x, _spriteRenderer.bounds.size.y);
    }
    
    public void MakeTransparent()
    {
        _spriteRenderer.sprite = _transparentSprite;
        Glitch();
        
    }

    public void ResetSprite()
    {
        _spriteRenderer.sprite = _sprite;
        Glitch();
    }

    private void Glitch()
    {
        Sequence seq = DOTween.Sequence();
        
        // Shake
        seq.Join(transform.DOShakePosition(0.3f, new Vector3(0.15f, 0.1f, 0), 40, 90));

        // Rotational jitter
        seq.Join(transform.DOShakeRotation(0.3f, new Vector3(0, 0, 15), 40, 90));

        // Rapid scale
        seq.Join(transform.DOScale(1.05f, 0.05f).SetLoops(4, LoopType.Yoyo));

        // Sprite flicker
        seq.InsertCallback(0.05f, () => _spriteRenderer.enabled = false);
        seq.InsertCallback(0.10f, () => _spriteRenderer.enabled = true);
        seq.InsertCallback(0.15f, () => _spriteRenderer.enabled = false);
        seq.InsertCallback(0.20f, () => _spriteRenderer.enabled = true);

        // Teleport jitter
        seq.AppendCallback(() =>
        {
            transform.localPosition += new Vector3(
                Random.Range(-0.05f, 0.05f),
                Random.Range(-0.05f, 0.05f),
                0
            );
        });
    }

}
