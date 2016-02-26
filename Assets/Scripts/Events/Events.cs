using System;

//i don't like this....we should find a way to scope them in a way that IDs wont overlap
static class Events
{
    public static class Game
    {
        public static int ScoredPoints = 1;
        public static int PlayerDead = 2;
    }

    public static class Framework
    {
        public static int StatChanged = 3;
    }


    /*
    public static class Dodge
    {
        public static int BadCollectible = 1;
        public static int GoodCollectible = 2;
    }

    public static class Shoot
    {
        public static int BaddieKilled = 3;
        public static int PlayerBaseDestroyed = 4;
    }
    */
    public static class GameStateTransition
    {
        public static int TransitionAway = 5;
        public static int TransitionTo = 6;
    }

    public static class Level
    {
        public static int StartLevel = 7;
        public static int LostLevel = 8;
        public static int WonLevel = 9;
        public static int AdvanceToNextLevel = 10;
        public static int BeatLastLevel = 11;
    }

    public static class PlayingStateTransition
    {
        public static int TransitionAway = 12;
        public static int TransitionTo = 13;
    }

    public static class Pause
    {
        public static int UnPause = 14;
    }

    public static class Retry
    {
        public static int RetryLevel = 15;
        public static int ReturnToMainMenu = 16;
    }
}

