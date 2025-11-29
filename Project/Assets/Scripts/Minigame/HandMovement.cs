using UnityEngine;

public class HandMovement : MonoBehaviour
{
    // GENERAL MOUSE MOVEMENT
    [Header("Hand movement")]
    private Vector3 Position;

    public State MovementState;
    public bool IsDrunk;

    [Range(0f, 0.5f)]
    [Tooltip("Changes the speed of the movement when NOT SLIDING")]
    public float Speed; 

    [Range(0f, 20f)]
    [Tooltip("Changes the \"lag\" behind the movement")]
    public float FollowSpeed = 10f;

    // ----------------------------------------------------
    // HAND TREMBLING
    [Header("Hand trembling")]
    [Tooltip("The \"length\" of the shake in X axis")]
    public float ShakingX = 1f;
    [Tooltip("The \"length\" of the shake in Y axis")]
    public float ShakingY = 0.3f;

    [Range(0.1f, 20f)]
    [Tooltip("Shake speed")]
    public float ShakeSpeed = 5f;
    private float shakeTimeX;
    private float shakeTimeY;

    // ----------------------------------------------------
    // MOUSE DRIFTING
    //[Header("Mouse drifting")]
    //public bool ApplyDrift = true;
    [Tooltip("How fast it can get to slide")]
    public float DriftAcceleration = 2f;
    [Tooltip("How fast it slows down")]
    public float DriftFriction = 4f;
    private Vector3 driftVelocity;



    public enum State
    {
        KeyControllable,
        NotControllable
    }

    void Start()
    {
        
    }

    void Update()
    {
        switch (MovementState)
        {
            case State.KeyControllable:
                SetPosition();
                break;
            default:
                break;
        }
    }

    void SetPosition()
    {
        Vector2 move = Vector2.zero;

        var keyboard = UnityEngine.InputSystem.Keyboard.current;
        if (keyboard.upArrowKey.isPressed) move.y += Speed;
        if (keyboard.downArrowKey.isPressed) move.y -= Speed;
        if (keyboard.rightArrowKey.isPressed) move.x += Speed;
        if (keyboard.leftArrowKey.isPressed) move.x -= Speed;

        Control(move, IsDrunk);
    }


    void Control(Vector2 move, bool is_drunk)
    {
        if (is_drunk)
        {
            // Apply drift
            if (move != Vector2.zero)
            {
                Vector3 dir = new Vector3(move.x, move.y, 0f).normalized / 30;
                driftVelocity += dir * Speed * DriftAcceleration * Time.deltaTime;
            }
            else
            {
                driftVelocity -= driftVelocity * DriftFriction * Time.deltaTime;
            }

            Position += driftVelocity;
        }
        else
        {
            Position += (Vector3)move * 15 * Time.deltaTime;
        }

        Vector3 result = Position;
        if (!IsDrunk) result += SetTrembling();

        transform.position = Vector3.Lerp(transform.position, result, FollowSpeed * Time.deltaTime);
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
