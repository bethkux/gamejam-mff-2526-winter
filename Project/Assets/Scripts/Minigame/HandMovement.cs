using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngineInternal;

public class HandMovement : MonoBehaviour
{
    // ---GENERAL MOUSE MOVEMENT---
    [Header("Hand movement")]
    [HideInInspector] public Vector3 Position;

    public State MovementState;
    public bool IsDrunk;

    [Range(0f, 0.5f)]
    [Tooltip("Changes the speed of the movement when NOT SLIDING")]
    public float Speed; 

    [Range(0f, 20f)]
    [Tooltip("Changes the \"lag\" behind the movement")]
    public float FollowSpeed = 10f;

    public List<Cup> Cups = new List<Cup>();
    int idx = -1;

    // ----------------------------------------------------
    // ---HIDING/REVEALING---
    [Header("Hiding/Revealing")]
    public Vector2 RevealPosition;
    public Vector2 HidePosition;
    public float HideSpeed;
    private Coroutine moveRoutine;

    // ----------------------------------------------------
    // ---HAND TREMBLING---
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
    // ---MOUSE DRIFTING---
    [Header("Mouse drifting")]
    [Tooltip("How fast it can get to slide")]
    public float DriftAcceleration = 2f;
    [Tooltip("How fast it slows down")]
    public float DriftFriction = 4f;
    private Vector3 driftVelocity;


    public enum State
    {
        FreelyControllable,
        NotControllable,
        BoundToCups
    }

    void Update()
    {
        Cups = SwapManager.Instance.Cups;
        Cups = Cups.OrderBy(cup => cup.transform.position.x).ToList();
        
        switch (MovementState)
        {
            case State.FreelyControllable:
                SetFreePosition();
                break;
            case State.BoundToCups:
                SetCupPosition();
                break;
            default:
                break;
        }
    }

    private void Start()
    {
        transform.position = HidePosition;
        Position = transform.position;
        RevealHand();
        
        
        Cups = SwapManager.Instance.Cups;
        if (Cups.Count <= 0)
        {
            Debug.LogError("No Cupsss????");
            
        }
        else
        {
            if (MovementState is State.BoundToCups)
                SetCupPosition();
        }
    }


    void SetFreePosition()
    {
        Vector2 move = Vector2.zero;

        var keyboard = UnityEngine.InputSystem.Keyboard.current;
        if (keyboard.upArrowKey.isPressed) move.y += Speed;
        if (keyboard.downArrowKey.isPressed) move.y -= Speed;
        if (keyboard.rightArrowKey.isPressed) move.x += Speed;
        if (keyboard.leftArrowKey.isPressed) move.x -= Speed;

        FreeControl(move, IsDrunk);
    }

    void SetCupPosition()
    {
        if (Cups.Count <= 0)
            return;


        if (idx < 0 || idx > Cups.Count-1)
            idx = (Cups.Count - 1) / 2;

        var keyboard = UnityEngine.InputSystem.Keyboard.current;
        if (keyboard.rightArrowKey.wasPressedThisFrame)
        {
            idx++;
            idx = Mathf.Clamp(idx, 0, Cups.Count-1);
            SetMoveRoutineCup(Cups[idx].gameObject.transform.position);
        }

        if (keyboard.leftArrowKey.wasPressedThisFrame)
        {
            idx--;
            idx = Mathf.Clamp(idx, 0, Cups.Count-1);
            SetMoveRoutineCup(Cups[idx].gameObject.transform.position);
        }

        if (keyboard.spaceKey.wasPressedThisFrame)
        {
            if (SwapManager.Instance.IsBallUnderCup(Cups[idx]))
            {
                Debug.Log(Cups[idx].gameObject.name);
            }
        }

        transform.position = Position;
    }


    void FreeControl(Vector2 move, bool is_drunk)
    {
        if (is_drunk)
        {
            // Apply drift
            if (move != Vector2.zero)
            {
                Vector3 dir = new Vector3(move.x, move.y, 0f).normalized / 30;
                driftVelocity += dir * Speed * DriftAcceleration * 1.5f * Time.deltaTime;
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

    IEnumerator MoveTo(Vector2 target, float speed)
    {
        Vector3 start = Position;
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime * speed;

            float eased = Mathf.SmoothStep(0f, 1f, t);

            Position = Vector3.Lerp(start, target, eased);
            yield return null;
        }

        Position = target;
    }

    public void SetMoveRoutineFree(Vector2 pos)
    {
        if (moveRoutine != null)
            StopCoroutine(moveRoutine);

        moveRoutine = StartCoroutine(MoveTo(pos, HideSpeed));
    }

    public void SetMoveRoutineCup(Vector2 pos)
    {
        if (moveRoutine != null)
            StopCoroutine(moveRoutine);

        moveRoutine = StartCoroutine(MoveTo(pos, 10));
    }


    public void HideHand()
    {
        MovementState = State.NotControllable;

        SetMoveRoutineFree(HidePosition);
    }

    public void RevealHand()
    {
        SetMoveRoutineFree(RevealPosition);

        //MovementState = State.FreelyControllable;
    }
}
