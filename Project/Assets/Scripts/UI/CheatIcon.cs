using UnityEngine;

public class CheatIcon : MonoBehaviour
{
    public void OnCheatClicked()
    {
        PlayerState.Instance.UseCheat(transform.GetSiblingIndex());
        Destroy(gameObject);
    }
}
