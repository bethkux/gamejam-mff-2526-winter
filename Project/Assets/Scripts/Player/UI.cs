using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    private static UI _Instance;
    public static UI Instance { get => _Instance; }

    [SerializeField]
    private VerticalLayoutGroup _CheatContainer;

    [SerializeField]
    private GameObject _CheatIconPrefab;


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
}
