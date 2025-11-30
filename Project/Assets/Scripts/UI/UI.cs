using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    private static UI _Instance;
    public static UI Instance { get => _Instance; }

    [SerializeField]
    private VerticalLayoutGroup _CheatContainer;

    [SerializeField]
    private GameObject _CheatIconPrefab;

    [SerializeField]
    private GameObject _DeathTextbox;

    [SerializeField]
    private TMP_Text _DeathText;

    [SerializeField]
    private GameObject _SpiritTextbox;

    [SerializeField]
    private TMP_Text _SpiritText;

    [SerializeField]
    private Slider _PointsSlider;

    [SerializeField]
    private CanvasGroup _RedFlash;


    private void Awake()
    {
        if (_Instance == null)
            _Instance = this;
        else
            Destroy(this);
        
        DontDestroyOnLoad(this);
    }

    public void UpdateCheatList(Sprite iconSprite)
    {
        GameObject newIcon = Instantiate(_CheatIconPrefab, _CheatContainer.transform);
        newIcon.GetComponent<Image>().sprite = iconSprite;
    }

    public void ShowSlider()
    {
        _PointsSlider.transform.DOScale(Vector3.one, 0.5f);
    }

    public void AddPoints(int amount)
    {
        _PointsSlider.value += amount;
    }


    public void FlashRed()
    {
    }

    public void PulsingRed()
    {
        
    }


    public void PlaySpiritDialogue(string text, bool closeTextbox)
    {
        _SpiritTextbox.transform.DOScale(Vector3.one, 0.5f).OnComplete( () => 
            StartCoroutine(TypeText(_SpiritText, text, closeTextbox))
        );
    }


    public void PlayDeathDialogue(string text, bool closeTextbox)
    {
        _DeathTextbox.transform.DOScale(Vector3.one, 0.5f).OnComplete( () => 
            StartCoroutine(TypeText(_DeathText, text, closeTextbox))
        );
    }


    public IEnumerator TypeText(TMP_Text textbox, string message, bool closeTextbox)
    {
        int alphaIndex = 0;

        yield return new WaitForSeconds(0.5f);

        if (textbox == _DeathText)
            AudioController.Instance.PlayDeathSpeakingSound();
        if (textbox == _SpiritText)
            AudioController.Instance.PlaySpiritsSpeakingSound();

        foreach (char c in message)
        {
            alphaIndex++;
            textbox.text = message;
            textbox.text = textbox.text.Insert(alphaIndex, "<color=#00000000>");
            yield return new WaitForSeconds(0.05f);
        }

        if (textbox == _DeathText)
            AudioController.Instance.StopDeathSpeakingSound();
        if (textbox == _SpiritText)
            AudioController.Instance.StopSpiritsSpeakingSound();

        yield return new WaitForSeconds(3);
        if (closeTextbox)
            _DeathTextbox.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InSine).OnComplete(() => _DeathText.text = "");
        else
            _DeathText.text = "";
    }
}
