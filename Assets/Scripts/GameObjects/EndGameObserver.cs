using UnityEngine;
using System.Collections;

public class EndGameObserver : MonoBehaviour
{
    public GameObject playingCanvas;
    public GameObject gameOverCanvas;
    public GameObject grayOutSheet; //used to make the game blurry to put the focus on the game over screen

    private int slothCount;

	// Use this for initialization
	void Start ()
    {
        Resolver.Instance.GetController<EventHandler>().Register(Events.Game.PlayerDead, OnPlayerDead);

        switch ((GameModeConfiguration.GameMode)Resolver.Instance.GetController<StatsManager>().GetSettingValue(GlobalStats.GameMode))
        {
            case GameModeConfiguration.GameMode.OneSloth:
                slothCount = 1;
                break;
            case GameModeConfiguration.GameMode.TwoSloths:
                slothCount = 2;
                break;
        }
    }

    void OnDestroy()
    {
        Resolver.Instance.GetController<EventHandler>().UnRegister(Events.Game.PlayerDead, OnPlayerDead);
    }

    void OnPlayerDead(int id, object data)
    {
        --slothCount;

        if (slothCount <= 0)
        {
            //game over
            Debug.Log("Game Over");

            Invoke("DisplayGameOverScreen", 3f);
        }
    }

    private void DisplayGameOverScreen()
    {
        playingCanvas.SetActive(false);
        gameOverCanvas.SetActive(true);
        grayOutSheet.SetActive(true);
    }
}
