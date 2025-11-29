using UnityEngine;

public class HandMovement : MonoBehaviour
{
    [Header("Hand movement")]
    public Vector2 Offset;
    public State MovementState;
    [Range(0f, 20f)]
    public float FollowSpeed = 10f;

    [Header("Hand trembling")]
    public float ShakingX = 1f;
    public float ShakingY = 0.3f;
    [Range(0.1f, 20f)]
    public float ShakeSpeed = 5f;
    private float shakeTimeX;
    private float shakeTimeY;

    [Header("Mouse drifting (ice)")]
    public float DriftAcceleration = 10f;
    public float DriftFriction = 3f;
    private Vector3 driftVelocity;



    public enum State
    {
        FollowingMouse,
        NotFollowingMouse
    }

    void Start()
    {
        
    }

    void Update()
    {
        switch (MovementState)
        {
            case State.FollowingMouse:
                SetPositionByMouse_org();
                break;
            default:
                break;
        }
    }

    void SetPositionByMouse_org() 
    { 
        Vector2 mouseScreenPos = UnityEngine.InputSystem.Mouse.current.position.ReadValue(); 
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(
            new Vector3(mouseScreenPos.x, mouseScreenPos.y, 0)
        ); 
        mouseWorldPos.z = 0; mouseWorldPos += (Vector3)Offset; 
        transform.position = Vector3.Lerp(transform.position, mouseWorldPos + SetTrembling(), FollowSpeed * Time.deltaTime); 
    }

    void SetPositionByMouse()
    {
        Vector2 mouseScreenPos = UnityEngine.InputSystem.Mouse.current.position.ReadValue();
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(
            new Vector3(mouseScreenPos.x, mouseScreenPos.y, 0)
        );
        mouseWorldPos.z = 0;
        mouseWorldPos += (Vector3)Offset;

        mouseWorldPos += SetTrembling();

        Vector3 dir = mouseWorldPos - transform.position;
        driftVelocity += dir * DriftAcceleration * Time.deltaTime;

        driftVelocity -= driftVelocity * DriftFriction * Time.deltaTime;

        transform.position += driftVelocity * Time.deltaTime;
    }


    Vector3 SetTrembling()
    {
        shakeTimeX += Time.deltaTime * ShakeSpeed;
        shakeTimeY += Time.deltaTime * ShakeSpeed * 1.13f;

        float x = Mathf.PerlinNoise(shakeTimeX, 0f) * 2f - 1f;
        float y = Mathf.PerlinNoise(shakeTimeY, 0f) * 2f - 1f;

        return new Vector3(x * ShakingX, y * ShakingY, 0f);
    }
}
