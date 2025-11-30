using DG.Tweening;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private string _GameSceneName = "MainScene";

    [SerializeField]
    private CanvasGroup _MenuContainer;

    [SerializeField]
    private CanvasGroup _CreditsContainer;

    [SerializeField]
    private GameObject _StartCupHighlight;

    [SerializeField]
    private GameObject _CreditsCupHighlight;

    [SerializeField]
    private GameObject _QuitCupHighlight;



    public void HighlightStartCup(bool isOn)
        => _StartCupHighlight.SetActive(isOn);

    public void HighlightCreditsCup(bool isOn)
        => _CreditsCupHighlight.SetActive(isOn);

    public void HighlightQuitCup(bool isOn)
        => _QuitCupHighlight.SetActive(isOn);



    public void StartGame()
    {
        ScreenFader.Instance.FadeOut(_GameSceneName);
    }


    public void ShowCredits()
    {
        _MenuContainer.DOFade(0, 0.5f).OnComplete(() =>
        {
            _MenuContainer.gameObject.SetActive(false);
            _CreditsContainer.gameObject.SetActive(true);     
            _CreditsContainer.DOFade(1, 0.5f);
        });
    }


    public void QuitGame() => Application.Quit();
}
