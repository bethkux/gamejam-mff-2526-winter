using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;


[RequireComponent(typeof(CanvasGroup))]
public class ScreenFader : MonoBehaviour
{
    private static ScreenFader _Instance;
    public static ScreenFader Instance { get => _Instance; }

    private CanvasGroup _CanvasGroup;


    private void Awake()
    {
        if (_Instance && _Instance != this)
            Destroy(_Instance.gameObject);

        _Instance = this;
        DontDestroyOnLoad(gameObject);

        _CanvasGroup = GetComponent<CanvasGroup>();
    }


    public void FadeOut(string nextScene, float duration = 1f)
    {
        _CanvasGroup.blocksRaycasts = true;
        _CanvasGroup.DOFade(1, duration).OnComplete(() => {
            SceneManager.LoadScene(nextScene, LoadSceneMode.Single);
            FadeIn(duration);
        });
    }

    public void FadeIn(float duration = 1f)
        => _CanvasGroup.DOFade(0, duration).OnComplete(() => _CanvasGroup.blocksRaycasts = false);
}
