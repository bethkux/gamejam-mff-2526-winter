using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
public class ObservingMinigame : MonoBehaviour
{
    [Range(0.6f, 1.2f)]
    public float Difficulty;

    [Header("Eye Sprites")]
    public Sprite OpenEye;
    public Sprite Alert;
    public Sprite CloseEye;
    private SpriteRenderer sr;

    [Header("Hand reference")]
    public HandMovement HandMovement;

    [Header("Observing timing")]
    public float MinObserveTime = 2f;
    public float MinRestTime = 2f;
    public float MaxExtraRandom = 3f;
    private float pMinObserveTime;
    private float pMinRestTime;
    private float pMaxExtraRandom;

    [Header("Alert settings")]
    public float AlertDuration = 1f;

    [Header("Movement tolerance")]
    public float AllowedMovementSpeed = 0.1f;

    private Vector3 lastHandPos;

    public enum State
    {
        Observing,
        NotObserving,
        Alert
    }

    public State ObservingState { get; private set; }

    private Coroutine loopRoutine;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        if (HandMovement == null)
            Debug.LogError("ObservingMinigame: Missing Hand reference!");

        ObservingState = State.NotObserving;
        UpdateTimers();
    }

    private void Start()
    {
        StartObservingMinigame();
    }

    // --------------------------------------------------
    //  CONTROL
    // --------------------------------------------------

    public void StartObservingMinigame()
    {
        if (loopRoutine != null)
            StopCoroutine(loopRoutine);

        loopRoutine = StartCoroutine(ObservationLoop());
    }

    public void StopObservingMinigame()
    {
        if (loopRoutine != null)
            StopCoroutine(loopRoutine);

        loopRoutine = null;

        // Reset
        ObservingState = State.NotObserving;
        sr.sprite = CloseEye;
    }

    // --------------------------------------------------
    //  MAIN LOOP
    // --------------------------------------------------

    IEnumerator ObservationLoop()
    {
        // initial REST phase — safe
        StopObservingVisual();
        float firstRest = pMinRestTime + 0.5f;
        yield return new WaitForSeconds(firstRest);

        while (true)
        {
            // ---- ALERT ----
            ShowAlert();
            yield return new WaitForSeconds(AlertDuration);

            // ---- OBSERVE ----
            StartObserving();
            lastHandPos = HandMovement.Position;

            float observeDuration = pMinObserveTime + Random.Range(0f, pMaxExtraRandom);
            float timer = 0f;

            while (timer < observeDuration)
            {
                timer += Time.deltaTime;

                float dist = Vector3.Distance(lastHandPos, HandMovement.Position);
                float speed = dist / Time.deltaTime;

                if (speed > AllowedMovementSpeed)
                {
                    OnPlayerMovedWhileObserving();
                }

                lastHandPos = HandMovement.Position;
                yield return null;
            }

            // ---- REST ----
            StopObservingVisual();
            float restDuration = pMinRestTime + Random.Range(0f, pMaxExtraRandom);
            yield return new WaitForSeconds(restDuration);
        }
    }

    // --------------------------------------------------
    //  SPRITE + STATE
    // --------------------------------------------------

    void ShowAlert()
    {
        ObservingState = State.Alert;
        sr.sprite = Alert;
    }

    void StartObserving()
    {
        ObservingState = State.Observing;
        sr.sprite = OpenEye;
        transform.localScale = Vector3.one * 0.8f;
    }

    void StopObservingVisual()
    {
        ObservingState = State.NotObserving;
        sr.sprite = CloseEye;
        transform.localScale = Vector3.one * 0.6f;
    }

    void OnPlayerMovedWhileObserving()
    {
        Debug.Log("Player moved while being observed!");
        // punishment / fail / restart logic here
    }

    public void IncreaseDifficulty()
    {
        Difficulty -= 0.7f;
        Difficulty = Mathf.Clamp(Difficulty, 0.6f, 1.2f);

        UpdateTimers();
    }

    private void UpdateTimers()
    {
        pMaxExtraRandom = MaxExtraRandom * Difficulty;
        pMinObserveTime = MinObserveTime * Difficulty;
        pMinRestTime = MinRestTime * Difficulty;   
    }
}
