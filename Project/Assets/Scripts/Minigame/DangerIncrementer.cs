using UnityEngine;

public class DangerIncrementer : MonoBehaviour
{
    public ObservingMinigame observingMinigame;
    public GameData gameData;

    void Awake()
    {
        gameData.minigameCount++;
        Debug.Log(gameData.minigameCount);
        for (int i = 0; i < gameData.minigameCount; i++)
        {
            observingMinigame.IncreaseDifficulty();
        }
        Debug.Log(observingMinigame.Difficulty);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
