using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class GameOverCanvas : MonoBehaviour
{
    public Text firstPlace;
    public Text secondPlace;
    public Text thirdPlace;

	// Use this for initialization
	void Start ()
    {
        StatsManager statsManager = Resolver.Instance.GetController<StatsManager>();

        GameStats gameStats = (GameStats)statsManager.GetSettingValue(GlobalStats.GameStats);
        int score = gameStats.score;

        GameModeConfiguration.GameMode gameMode = (GameModeConfiguration.GameMode)statsManager.GetSettingValue(GlobalStats.GameMode);

        HighScore.HighScoreBoard highScoreBoard = (gameMode == GameModeConfiguration.GameMode.OneSloth ? HighScore.HighScoreBoard.OneSloth : HighScore.HighScoreBoard.TwoSloth);

        HighScore highScore = new HighScore();
        highScore.Load();
        int position;
        if (highScore.AddNewScore(highScoreBoard, score, out position))
        {
            //the score cracked the high score board!
            highScore.SaveHighScores();
        }

        int[] highScores = highScore.GetHighScores(highScoreBoard);
        firstPlace.text = highScores[0].ToString();
        secondPlace.text = highScores[1].ToString();
        thirdPlace.text = highScores[2].ToString();

    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnRestart()
    {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Resolver.Instance.GetController<GameStateEngine>().ChangeGameState(GameStateEngine.States.Playing);
    }

    public void OnHome()
    {
        Resolver.Instance.GetController<GameStateEngine>().ChangeGameState(GameStateEngine.States.Title);
    }
}
