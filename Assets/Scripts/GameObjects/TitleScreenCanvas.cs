using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TitleScreenCanvas : MonoBehaviour
{
    public bool isMuted = false;
    public GameObject muteButton;
    public Sprite muteSprite;
    public Sprite volumeSprite;

    public void OnStartGameOneSloth()
    {
        Resolver.Instance.GetController<StatsManager>().UpdateSetting(GlobalStats.GameMode, GameModeConfiguration.GameMode.OneSloth);
        Resolver.Instance.GetController<GameStateEngine>().ChangeGameState(GameStateEngine.States.Playing);
    }

    public void OnStartGameTwoSloth()
    {
        Resolver.Instance.GetController<StatsManager>().UpdateSetting(GlobalStats.GameMode, GameModeConfiguration.GameMode.TwoSloths);
        Resolver.Instance.GetController<GameStateEngine>().ChangeGameState(GameStateEngine.States.Playing);
    }

    public void OnMute()
    {
        isMuted = !isMuted;

        if (isMuted)
        {
            AudioListener.volume = 0;
            muteButton.GetComponent<Image>().sprite = muteSprite;
        }
        else
        {
            AudioListener.volume = 1;
            muteButton.GetComponent<Image>().sprite = volumeSprite;
        }
    }

    public void OnCredits()
    {
        Resolver.Instance.GetController<GameStateEngine>().ChangeGameState(GameStateEngine.States.Credits);
    }

}

