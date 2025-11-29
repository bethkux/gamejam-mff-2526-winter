using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject _MenuContainer;

    [SerializeField]
    private GameObject _CreditsContainer;

    [SerializeField]
    private GameObject _StartButton;

    [SerializeField]
    private GameObject _CreditsButton;

    [SerializeField]
    private GameObject _QuitButton;



    public void StartGame()
    {
        
    }


    public void ShowCredits()
    {
        
    }


    public void QuitButton() => Application.Quit();
}
