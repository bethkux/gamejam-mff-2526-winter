using System;
using UnityEngine;


public class Cup : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    
    private void Start()
    {
        if (_spriteRenderer == null)
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            Debug.unityLogger.Log("SpriteRenderer", _spriteRenderer);
        }
        
        if (_spriteRenderer == null)   Debug.LogError("Cup SpriteRenderer is missing");
    }

    public Vector2 GetSize()
    {
        return new Vector2(_spriteRenderer.bounds.size.x, _spriteRenderer.bounds.size.y);
    }
}
