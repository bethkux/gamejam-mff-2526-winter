using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;



public class Test : MonoBehaviour
{
    public string scene;

    void Update()
    {
        if (Pointer.current == null) return;

        if (Pointer.current.press.wasPressedThisFrame)
        {
            Vector2 mouseScreenPos = Pointer.current.position.ReadValue();
            Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(mouseScreenPos);

            Collider2D col = Physics2D.OverlapPoint(mouseWorldPos);
            if (col != null && col.gameObject == gameObject)
            {     
                if(scene == "Minigame")
                    SceneManager.LoadScene("Minigame", LoadSceneMode.Additive);
                else
                {
                    Scene minigame = SceneManager.GetSceneByName("Minigame");
                    if (minigame == SceneManager.GetActiveScene())
                    {
                        SceneManager.SetActiveScene(SceneManager.GetSceneByName("MainScene"));
                    }
                    SceneManager.UnloadSceneAsync(minigame);
                }

            }
        }
    }
}
