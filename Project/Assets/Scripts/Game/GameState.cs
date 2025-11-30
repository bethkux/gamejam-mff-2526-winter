using System.Collections;
using DG.Tweening;
using UnityEngine;

public class GameState : MonoBehaviour
{
    private static GameState _Instance;
    public static GameState Instance { get => _Instance; }

    [SerializeField]
    private SpriteSequenceManager _BonerAnimator;

    [SerializeField]
    private GameObject _Table;

    [SerializeField]
    private GameObject _Boner;

    [SerializeField, Range(0, 1)]
    private float _DeathCheatingStartingProbability;

    private bool _IsInTutorial;

    private bool _IsRoundOne;

    public bool _CalloutOpen = false;

    private float _DeathCheatingProbability;
    private bool _IsDeathCheating = true;



    private void Awake()
    {
        if (_Instance && _Instance != this)
            Destroy(_Instance.gameObject);

        _Instance = this;
        DontDestroyOnLoad(gameObject);

        _DeathCheatingProbability = _DeathCheatingStartingProbability;
    }


    private void Start()
    {
        StartCoroutine(SceneProgression());




        // round two

        // ghosts tell you that they will help you beat death

        // you gain the first cheat

        // they tell you to press it if you want to cheat, but be careful that he doesn't catch.


    }

    private IEnumerator SceneProgression()
    {
        yield return SetupScene();

        yield return new WaitForSeconds(5);

        yield return Tutorial();
    }

    private IEnumerator SetupScene()
    {
        yield return new WaitForSeconds(0.5f);

        // table appears
        AudioController.Instance.PlayTableSlideIn();
        _Table.transform.DOMoveY(-3.55f, 1);
        
        yield return new WaitForSeconds(0.5f);

        // Boner appears
        AudioController.Instance.PlayDeathLaugh();
        _Boner.GetComponent<SpriteRenderer>().DOFade(1, 1);

        yield return new WaitForSeconds(2f);

        UI.Instance.PlayDeathDialogue(Dialgoues.DEATH_GREETING, true);

        yield return new WaitForSeconds(12);

        // cups appear
        SwapManager.Instance.Init();
    }

    private IEnumerator Tutorial()
    {
        _IsInTutorial = true;

        UI.Instance.PlayDeathDialogue(Dialgoues.TUTORIAL_CUPS, true);

        yield return new WaitForSeconds(2);

        SwapManager.Instance.PlaceBall();

        yield return new WaitForSeconds(5);

        yield return SwapManager.Instance.Swap(SwapManager.Instance.Cups.Count);

        UI.Instance.PlayDeathDialogue(Dialgoues.TUTORIAL_CHOOSE_CUP, true);

        yield return new WaitForSeconds(8);

        HandMovement.Instance.Init();

        // show tutorial messages (WASD, space to select)
    }

    private IEnumerator RoundOne()
    {
        _IsRoundOne = true;

        SwapManager.Instance.Init();

        UI.Instance.PlayDeathDialogue(Dialgoues.ROUND_ONE_INTRO, false);
        yield return new WaitForSeconds(8);

        UI.Instance.PlayDeathDialogue(Dialgoues.ROUND_ONE_STAKES_WIN, false);
        yield return new WaitForSeconds(12);

        UI.Instance.PlayDeathDialogue(Dialgoues.ROUND_ONE_STAKES_LOSE, false);
        yield return new WaitForSeconds(12);

        UI.Instance.PlayDeathDialogue("Let's start.", true);
        yield return new WaitForSeconds(2);

        UI.Instance.PlaySpiritDialogue("Psst. Hey! Hey you, fleshface! Yeah you!", false);
        yield return new WaitForSeconds(5);

        UI.Instance.PlaySpiritDialogue("You're new around these parts, eh? Well, there's a couple things you gotta learn. The main one being, that that bastard cheats! Look at his shifty eyes! Don't let yourself get duped kid.", true);
        yield return new WaitForSeconds(10);

        _CalloutOpen = true;
        yield return SwapManager.Instance.Swap(SwapManager.Instance.Cups.Count);
    }


    private IEnumerator RoundTwo()
    {
        yield return null;
    }

    private IEnumerator Round()
    {
        yield return null;
    }


    public IEnumerator CorrectCupSelected()
    {
        if (_IsInTutorial)
        {
            UI.Instance.PlayDeathDialogue(Dialgoues.TUTORIAL_CORRECT_GUESS, false);
            yield return new WaitForSeconds(10);
            _IsInTutorial = false;
            StartCoroutine(RoundOne());
        }
        else
        {
            //UI.Instance.PlayDeathDialogue();
            yield return new WaitForSeconds(5);
            PlayerState.Instance.GetPoints(1);

            if (_IsRoundOne)
                StartCoroutine(RoundTwo());
            else
                StartCoroutine(Round());
        }
    }


    public IEnumerator WrongCupSelected()
    {
        if (_IsInTutorial)
        {
            UI.Instance.PlayDeathDialogue(Dialgoues.TUTORIAL_WRONG_GUESS, false);
            yield return new WaitForSeconds(10);
            _IsInTutorial = false;
            StartCoroutine(RoundOne());
        }
        else
        {
            //UI.Instance.PlayDeathDialogue();
            yield return new WaitForSeconds(5);
            PlayerState.Instance.LoseFinger();

            if (_IsRoundOne)
                StartCoroutine(RoundTwo());
            else
                StartCoroutine(Round());
        }
    }


    public IEnumerator CalledOut()
    {
        if (_IsDeathCheating)
        {
            UI.Instance.PlayDeathDialogue(Dialgoues.CalledOutOnCheating_Correctly[Random.Range(0, Dialgoues.CalledOutOnCheating_Correctly.Length - 1)], true);
            PlayerState.Instance.GetPoints(10);
        }
        else
        {
            UI.Instance.PlayDeathDialogue(Dialgoues.CalledOutOnCheating_Incorrectly[Random.Range(0, Dialgoues.CalledOutOnCheating_Incorrectly.Length - 1)], true);
            PlayerState.Instance.LoseFinger();
        }

        yield return new WaitForSeconds(8);

        if (_IsRoundOne)
            StartCoroutine(RoundTwo());
        else
            StartCoroutine(Round());
    }


    public void SwitchToMinigame()
    {
        HandMovement.Instance.ChangeState(HandMovement.State.FreelyControllable);
        ScreenFader.Instance.FadeOut();

    }


    public void SwitchToBaseScene()
    {
        HandMovement.Instance.ChangeState(HandMovement.State.BoundToCups);
        // switch to normal scene
    }



}
