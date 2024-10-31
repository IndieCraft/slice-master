using UnityEngine;
#if UNITY_ANDROID
using GooglePlayGames;
using GooglePlayGames.BasicApi;
#else
using UnityEngine.SocialPlatforms;
#endif

public class VPGPlayGames : MonoBehaviour
{
    public static VPGPlayGames instance;

#if UNITY_IOS
    static ILeaderboard m_Leaderboard;

    private string leaderboardName = "Leaderboard";
    private string leaderboardID = "vp.slicemaster.leaderboard";

    private string achnewbie = "vp.slicemaster.newbie";
    private string achlv5 = "vp.slicemaster.level5";
    private string achlv10 = "vp.slicemaster.level10";
    private string achlv15 = "vp.slicemaster.level15";
    private string achlv20 = "vp.slicemaster.level20";
    private string achlv25 = "vp.slicemaster.level25";
    private string achmaster = "vp.slicemaster.master";
    private string achlv40 = "vp.slicemaster.level40";
    private string achgmaster = "vp.slicemaster.gmaster";
    private string achlv60 = "vp.slicemaster.level60";
    private string achlv70 = "vp.slicemaster.level70";
    private string achlv80 = "vp.slicemaster.level80";
    private string achlv90 = "vp.slicemaster.level90";
    private string achlegendary = "vp.slicemaster.legendary";
#endif

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != null)
            Destroy(gameObject);
    }

    void Start()
    {
#if UNITY_ANDROID
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().Build();
        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.Activate();
        SignIn();
#else
        Social.localUser.Authenticate (ProcessAuthentication);
        DoLeaderboard();
#endif

    }
#if UNITY_ANDROID
    public void SignIn()
    {

        Social.localUser.Authenticate(success =>
        {
            if (success)
            {
                Debug.Log("Authentication successful");
                /*                string userInfo = "Username: " + Social.localUser.userName +
                                    "\nUser ID: " + Social.localUser.id +
                                    "\nIsUnderage: " + Social.localUser.underage;
                                Debug.Log(userInfo);*/
            }
            else
                Debug.Log("Authentication failed");
        });
    }
#else

    void ProcessAuthentication(bool success)
    {
        if (success)
        {
            Debug.Log("Authenticated, checking achievements");

            // MAKE REQUEST TO GET LOADED ACHIEVEMENTS AND REGISTER A CALLBACK FOR PROCESSING THEM
            Social.LoadAchievements(ProcessLoadedAchievements); // ProcessLoadedAchievements FUNCTION CAN BE FOUND BELOW

            Social.LoadScores(leaderboardName, scores => {
                if (scores.Length > 0)
                {
                    // SHOW THE SCORES RECEIVED
                    Debug.Log("Received " + scores.Length + " scores");
                    string myScores = "Leaderboard: \n";
                    foreach (IScore score in scores)
                        myScores += "\t" + score.userID + " " + score.formattedValue + " " + score.date + "\n";
                    Debug.Log(myScores);
                }
                else
                    Debug.Log("No scores have been loaded.");
            });
        }
        else
            Debug.Log("Failed to authenticate with Game Center.");
    }

    void ProcessLoadedAchievements(IAchievement[] achievements)
    {
        if (achievements.Length == 0)
            Debug.Log("Error: no achievements found");
        else
            Debug.Log("Got " + achievements.Length + " achievements");
        //Social.ShowAchievementsUI();
    }
#endif
    public void UpdateScore()
    {
        long totalScore = PlayerPrefs.GetInt("TotScore");

#if UNITY_ANDROID
        //Debug.Log("Submitting score " + totalScore + " to leaderboard " + leaderboardID);
        Social.ReportScore(totalScore, GPGSIds.leaderboard_leaderboard, (bool success) =>
        {
            if (success)
            {
                Debug.Log("Done");
            }
            else
            {
                Debug.Log("Couldn't submit the score.");
            }
        });
#else
        Debug.Log("Reporting score " + totalScore + " on leaderboard " + leaderboardID);
        Social.ReportScore(totalScore, leaderboardID, success => {
            Debug.Log(success ? "Reported score to leaderboard successfully" : "Failed to report score");
        });
#endif

    }

    public void UnlockLevelAch()
    {
        int currentLevel = PlayerPrefs.GetInt("LevCount");
        if (currentLevel >= 1)
        {
            Social.ReportProgress(GPGSIds.achievement_newbie, 100.0f, (bool success) =>
            {
                if (success)
                {
                    Debug.Log("Achievement unlocked");
                }
            });
        }
        if (currentLevel >= 5)
        {
            Social.ReportProgress(GPGSIds.achievement_level_5, 100.0f, (bool success) =>
            {
                if (success)
                {
                    Debug.Log("Achievement unlocked");
                }
            });
        }
        if (currentLevel >= 10)
        {
            Social.ReportProgress(GPGSIds.achievement_level_10, 100.0f, (bool success) =>
            {
                if (success)
                {
                    Debug.Log("Achievement unlocked");
                }
            });
        }
        if (currentLevel >= 15)
        {
            Social.ReportProgress(GPGSIds.achievement_level_15, 100.0f, (bool success) =>
            {
                if (success)
                {
                    Debug.Log("Achievement unlocked");
                }
            });
        }
        if (currentLevel >= 20)
        {
            Social.ReportProgress(GPGSIds.achievement_level_20, 100.0f, (bool success) =>
            {
                if (success)
                {
                    Debug.Log("Achievement unlocked");
                }
            });
        }
        if (currentLevel >= 25)
        {
            Social.ReportProgress(GPGSIds.achievement_level_25, 100.0f, (bool success) =>
            {
                if (success)
                {
                    Debug.Log("Achievement unlocked");
                }
            });
        }
        if (currentLevel >= 30)
        {
            Social.ReportProgress(GPGSIds.achievement_master, 100.0f, (bool success) =>
            {
                if (success)
                {
                    Debug.Log("Achievement unlocked");
                }
            });
        }
        if (currentLevel >= 40)
        {
            Social.ReportProgress(GPGSIds.achievement_level_40, 100.0f, (bool success) =>
            {
                if (success)
                {
                    Debug.Log("Achievement unlocked");
                }
            });
        }
        if (currentLevel >= 50)
        {
            Social.ReportProgress(GPGSIds.achievement_grand_master, 100.0f, (bool success) =>
            {
                if (success)
                {
                    Debug.Log("Achievement unlocked");
                }
            });
        }
        if (currentLevel >= 60)
        {
            Social.ReportProgress(GPGSIds.achievement_level_60, 100.0f, (bool success) =>
            {
                if (success)
                {
                    Debug.Log("Achievement unlocked");
                }
            });
        }
        if (currentLevel >= 70)
        {
            Social.ReportProgress(GPGSIds.achievement_level_70, 100.0f, (bool success) =>
            {
                if (success)
                {
                    Debug.Log("Achievement unlocked");
                }
            });
        }
        if (currentLevel >= 80)
        {
            Social.ReportProgress(GPGSIds.achievement_level_80, 100.0f, (bool success) =>
            {
                if (success)
                {
                    Debug.Log("Achievement unlocked");
                }
            });
        }
        if (currentLevel >= 90)
        {
            Social.ReportProgress(GPGSIds.achievement_level_90, 100.0f, (bool success) =>
            {
                if (success)
                {
                    Debug.Log("Achievement unlocked");
                }
            });
        }
        if (currentLevel >= 100)
        {
            Social.ReportProgress(GPGSIds.achievement_legendary, 100.0f, (bool success) =>
            {
                if (success)
                {
                    Debug.Log("Achievement unlocked");
                }
            });
        }

    }

    public void ShowLeaderboard()
    {
        Social.ShowLeaderboardUI();
    }

#if UNITY_IPHONE
    void DoLeaderboard()
    {
        m_Leaderboard = Social.CreateLeaderboard();
        m_Leaderboard.id = leaderboardID;
        m_Leaderboard.LoadScores(result => DidLoadLeaderboard(result));
    }

    void DidLoadLeaderboard(bool result)
    {
        Debug.Log("Received " + m_Leaderboard.scores.Length + " scores");
        foreach (IScore score in m_Leaderboard.scores)
        {
            Debug.Log(score);
        }
        //Social.ShowLeaderboardUI();
    }
#endif
}
