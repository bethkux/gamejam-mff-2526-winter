using UnityEngine;

public class AudioController : MonoBehaviour
{
    private static AudioController _Instance;
    public static AudioController Instance { get => _Instance; }

    public AudioSource DeathSpeakingSound;

    public AudioSource SpiritsSpeakingSound;

    public AudioSource DeathLaugh;

    public AudioSource TableSlideIn;

    public AudioSource PlaceCups;

    public AudioSource CupReveal;

    public AudioSource GlassSlidingOnWood;

    public AudioSource HeartBeatSingle;

    public AudioSource HeartBeatLong;

    private void Awake()
    {
        if (_Instance && _Instance != this)
            Destroy(_Instance.gameObject);

        _Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void PlayDeathSpeakingSound()
    {
        DeathSpeakingSound.Play();
    }

    public void StopDeathSpeakingSound()
    {
        DeathSpeakingSound.Stop();
    }

    public void PlaySpiritsSpeakingSound()
    {
        SpiritsSpeakingSound.Play();
    }

    public void StopSpiritsSpeakingSound()
    {
        SpiritsSpeakingSound.Stop();
    }

    public void PlayDeathLaugh()
    {
        DeathLaugh.Play();
    }

    public void PlayTableSlideIn()
    {
        TableSlideIn.Play();
    }

    public void PlayPlaceCups()
    {
        PlaceCups.Play();
    }

    public void PlayCupReveal()
    {
        CupReveal.Play();
    }

    public void PlayGlassSliding()
    {
        GlassSlidingOnWood.Play();
    }

    public void PlayHeartbeatSingle()
    {
        HeartBeatSingle.Play();
    }

    public void StopHeartbeatSingle()
    {
        HeartBeatSingle.Stop();
    }

    public void PlayHeartbeatLong()
    {
        HeartBeatLong.Play();
    }

    public void StopHeartbeatLong()
    {
        HeartBeatLong.Stop();
    }
}
