using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;


public class Shot : MonoBehaviour
{
    public Vector2 StartPosition;
    public Vector2 EndPosition;
    public float Duration = 0.5f;

    void Start()
    {
        AudioController.Instance.PlayTableSlideIn();

        transform.position = StartPosition;
        transform.DOMove(EndPosition, Duration)
                 .SetEase(Ease.OutCubic);
    }

    public HandMovement hm;

    void Update()
    {
        if (Pointer.current == null) return;

        if (Pointer.current.press.wasPressedThisFrame)
        {
            Vector2 mouseScreenPos = Pointer.current.position.ReadValue();
            Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(mouseScreenPos);

            Collider2D col = Physics2D.OverlapPoint(mouseWorldPos);
            if (col != null && col.gameObject == gameObject)
            {
                hm.IsDrunk = true;
                gameObject.SetActive(false);
            }
        }
    }

}
