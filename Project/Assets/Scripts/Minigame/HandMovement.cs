using UnityEngine;

public class HandMovement : MonoBehaviour
{
    public Vector2 Offset;
    public State movementState;
    [Range(0f, 20f)]
    public float followSpeed = 10f;


    public enum State
    {
        Following
    }

    void Start()
    {
        
    }

    void Update()
    {
        switch (movementState)
        {
            case State.Following:
                SetPositionByMouse();
                break;
            default:
                break;
        }
    }

    void SetPositionByMouse()
    {
        Vector2 mouseScreenPos = UnityEngine.InputSystem.Mouse.current.position.ReadValue();
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(new Vector3(mouseScreenPos.x, mouseScreenPos.y, 0));
        mouseWorldPos.z = 0;
        mouseWorldPos += (Vector3)Offset;

        transform.position = Vector3.Lerp(transform.position, mouseWorldPos, followSpeed * Time.deltaTime);
    }
}
