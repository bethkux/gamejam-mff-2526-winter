using System.Collections;
using DG.Tweening;
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
    private GameObject _SpiritTextbox;


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
        yield return new WaitForSeconds(2);
        ShowTextbox();
    }


    public void UpdateCheatList(Sprite iconSprite)
    {
        GameObject newIcon = Instantiate(_CheatIconPrefab, _CheatContainer.transform);
        newIcon.GetComponent<Image>().sprite = iconSprite;
    }


    public void ShowTextbox()
    {
        _SpiritTextbox.transform.DOScale(Vector3.one, 2f).SetEase(Ease.OutSine);
    }

}
