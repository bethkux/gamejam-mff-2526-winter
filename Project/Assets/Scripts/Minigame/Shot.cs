using UnityEngine;
using UnityEngine.InputSystem;


public class Shot : MonoBehaviour
{
    void Start()
    {
        
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
