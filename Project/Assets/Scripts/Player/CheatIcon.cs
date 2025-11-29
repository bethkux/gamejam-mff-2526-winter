using UnityEngine;

public class CheatIcon : MonoBehaviour
{
    public void OnCheatClicked()
    {
        Debug.Log(transform.GetSiblingIndex());
        PlayerState.Instance.UseCheat(transform.GetSiblingIndex());
        Destroy(gameObject);
    }
}
