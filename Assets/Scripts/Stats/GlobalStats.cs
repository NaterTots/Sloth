using UnityEngine;
using System.Collections;

public class GlobalStats : MonoBehaviour, IController
{
    public GameModeConfiguration.GameMode gameMode;
    public GameStats gameStats;

    public static string GameStats = "GlobalStats.GameStats";
    public static string GameMode = "GlobalStats.GameMode";

    private StatsManager statsManager;
    private EventHandler eventHandler;

    void Awake()
    {
        statsManager = Resolver.Instance.GetController<StatsManager>();

        statsManager.AddSetting<GameStats>(GameStats, gameStats);
        statsManager.AddSetting<GameModeConfiguration.GameMode>(GameMode, gameMode);

        eventHandler = Resolver.Instance.GetController<EventHandler>();
        eventHandler.Register(Events.Game.ScoredPoints, ScoredPoints);
        eventHandler.Register(Events.GameStateTransition.TransitionTo, OnGameStart);
    }

    public void ScoredPoints(int id, object data)
    {
        Setting gameStats = statsManager.GetSetting(GameStats);
        ((GameStats)gameStats.Value).score += (int)data;

        eventHandler.Post(Events.Framework.StatChanged, new StatChangeEventArgs(GameStats));
    }

    void OnGameStart(int id, object data)
    {
        Setting gameStats = statsManager.GetSetting(GameStats);
        ((GameStats)gameStats.Value).score = 0;
    }


    public void Cleanup()
    {
        eventHandler.UnRegister(Events.Game.ScoredPoints, ScoredPoints);
        eventHandler.UnRegister(Events.GameStateTransition.TransitionTo, OnGameStart);
    }
}


