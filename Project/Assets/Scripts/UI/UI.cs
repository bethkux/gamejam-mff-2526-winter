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


    private void Awake()
    {
        if (_Instance == null)
            _Instance = this;
        else
            Destroy(this);
        
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        StartCoroutine(Test());
    }


    private IEnumerator Test()
    {
        yield return new WaitForSeconds(1);
        ShowDeathTextbox("Hello this is a test message. YAP YAP YAP YAP");
    }


    public void UpdateCheatList(Sprite iconSprite)
    {
        GameObject newIcon = Instantiate(_CheatIconPrefab, _CheatContainer.transform);
        newIcon.GetComponent<Image>().sprite = iconSprite;
    }


    public void ShowSpiritTextbox(string text)
    {
        _SpiritTextbox.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutSine).OnComplete(() => StartCoroutine(TypeText(_SpiritText, text)));
    }


    public void HideSpiritTextbox()
    {
        _SpiritTextbox.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InSine);
    }


    public void ShowDeathTextbox(string text)
    {
        _DeathTextbox.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutSine).OnComplete(() => StartCoroutine(TypeText(_DeathText, text)));;
    }


    public void HideDeathTextbox()
    {
        _DeathTextbox.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InSine);
    }


    public IEnumerator TypeText(TMP_Text textbox, string message)
    {
        string originalText = message;
        int alphaIndex = 0;

        yield return new WaitForSeconds(0.5f);

        foreach (char c in message)
        {
            alphaIndex++;
            textbox.text = originalText;
            textbox.text = textbox.text.Insert(alphaIndex, "<color=#00000000>");
            yield return new WaitForSeconds(0.05f);
        }

        yield return new WaitForSeconds(3);
        HideDeathTextbox();
    }
}
