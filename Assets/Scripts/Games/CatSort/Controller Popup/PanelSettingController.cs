using Services;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PanelSettingController : MonoBehaviour
{
    [SerializeField] private UnityEvent<bool> onButtonClose;
    [SerializeField] private Slider sliderMusic;
    [SerializeField] private Slider sliderSound;

    private PlayerService playerService;

    public void SetPlayerService(PlayerService playerService) => this.playerService = playerService;
    public void OffPanelReset()
    {
        onButtonClose?.Invoke(false);
    }
    private void Start()
    {
        InitSetting();
    }
    public void InitSetting()
    {
        float music = playerService.GetMusicVolume();
        float sound = playerService.GetSoundVolume();

        sliderMusic.value = music;
        sliderSound.value = sound;
    }
    public void MusicChange()
    {
        float value = sliderMusic.value;
        playerService.SetMusicVolume(value);
    }
    public void SoundChange()
    {
        float value = sliderSound.value;
        playerService.SetSoundVolume(value);
    }
}
