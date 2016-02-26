using UnityEngine;
using System.Collections;

public class HighScore
{
    public enum HighScoreBoard
    {
        OneSloth,
        TwoSloth
    };

    private int[] oneSlothHighScores;
    private int[] twoSlothHighScores;

    private const string OneSlothPrefix = "OneSloth";
    private const string TwoSlothPrefix = "TwoSloth";

	// Use this for initialization
	public void Load()
    {
        oneSlothHighScores = new int[3];
        twoSlothHighScores = new int[3];

        for (int i = 0; i < 3; i++)
        {
            oneSlothHighScores[i] = PlayerPrefs.GetInt(OneSlothPrefix + i.ToString(), 0);
            twoSlothHighScores[i] = PlayerPrefs.GetInt(TwoSlothPrefix + i.ToString(), 0);
        }
	}
	
	public void SaveHighScores()
    {
        for (int i = 0; i < 3; i++)
        {
            PlayerPrefs.SetInt(OneSlothPrefix + i.ToString(), oneSlothHighScores[i]);
            PlayerPrefs.SetInt(TwoSlothPrefix + i.ToString(), twoSlothHighScores[i]);
        }

        PlayerPrefs.Save();
    }

    public int[] GetHighScores(HighScoreBoard scoreBoard)
    {
        return (scoreBoard == HighScoreBoard.OneSloth) ? oneSlothHighScores : twoSlothHighScores;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="scoreBoard"></param>
    /// <param name="score"></param>
    /// <param name="position">If the function returns true, this will hold the position of the new score on the board.</param>
    /// <returns>Whether or not the score cracked the high score board.</returns>
    public bool AddNewScore(HighScoreBoard scoreBoard, int score, out int position)
    {
        return AddNewScore(scoreBoard == HighScoreBoard.OneSloth ? oneSlothHighScores : twoSlothHighScores, score, out position);
    }

    private bool AddNewScore(int[] scoreboard, int score, out int position)
    {
        bool onBoard = false;
        position = 0;

        int nextBest = 0;
        for (int i = 0; i < scoreboard.Length; i++)
        {
            //if the score is already on the board, just push down the rest of the entries
            if (onBoard)
            {
                int tempCurrent = nextBest;
                nextBest = scoreboard[i];
                scoreboard[i] = tempCurrent;
            }
            else if (score > scoreboard[i])
            {
                nextBest = scoreboard[i];
                scoreboard[i] = score;
                position = i;
                onBoard = true;
            }
        }

        return onBoard;
    }
}
