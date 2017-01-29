using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public delegate void GameMasterEvents(object value);

public class GameManager : MonoBehaviour
{

    public struct Score
    {
        public string scoreName;
        public int scoreVal;
        public bool newHighscoreFlag;
    }

    List<Score> highScoreList;

    public static GameManager theManager;
    public Stat scoreStat;

    public PlayerController playerControl;
    public Stat playerLives;

    public GameObject clearObject;
    public CircleCollider2D burstWipeCollider;

    public GameObject playScreen;
    public GameObject gameOverScreen;
    public GameObject UI_PauseScreen;

    bool isPaused = false;

    public bool isMainMenu = false;

    // Use this for initialization
    void Start()
    {
        
    }

    void Init()
    {
        highScoreList = new List<Score>();
        
        if (playerControl)
        {
            playerControl.PlayerDeath += new PlayerEventHandler(OnPlayerDeath);
        }

        isPaused = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void StartLevel()
    {
        gameOverScreen.SetActive(false);
        playScreen.SetActive(true);
        ClearScreenOfParticles();
        scoreStat.SetToZero();
        playerLives.SetToMax();

        SpawnPlayerDelayed(0.5f);
    }

    void ClearScreenOfParticles()
    {
        clearObject.SetActive(true);
        StartCoroutine(DisableClearScreen(1.0f));
    }

    IEnumerator DisableClearScreen(float fWait)
    {
        yield return new WaitForSeconds(fWait);
        clearObject.SetActive(false);
        yield return null;
    }

    void OnPlayerDeath()
    {
        playerControl.gameObject.SetActive(false);
        playerLives.IncrementBy(-1);
        print(playerLives.current.ToString() + " lives remaining");


        if (playerLives.current == 0)
        {
            GameOver();
        }
        else
        {
            BurstWipe(playerControl.gameObject.transform.position);
            StartCoroutine(SpawnPlayerDelayed(1.0f));
        }
    }

    void GameOver()
    {
        BurstWipe(playerControl.gameObject.transform.position);
        playScreen.SetActive(false);

        StartCoroutine(AnimatedGameOver());
    }

    IEnumerator AnimatedGameOver()
    {
        yield return new WaitForSeconds(1.5f);
        
        GetHighscores();

        IsNewHighscore((int)scoreStat.current);

        gameOverScreen.SetActive(true);
    }

    public List<Score> GetHighList()
    {
        return highScoreList;
    }

    public void SetNewHighscoreName(string namePR)
    {
        for(int i = 0; i < highScoreList.Count; i++)
        {
            if(highScoreList[i].newHighscoreFlag)
            {
                //this is the new entry, hope we don't have more than one of these
                Score modScore = highScoreList[i];
                modScore.newHighscoreFlag = false;
                modScore.scoreName = namePR;
                highScoreList[i] = modScore;
                SetHighscores();
                break;
            }
        }
    }

    void GetHighscores()
    {
        highScoreList.Clear();
        for (int i = 0; i < 10; i++)
        {
            if (PlayerPrefs.HasKey("ScoreName" + i.ToString()))
            {
                string scoreName = PlayerPrefs.GetString("ScoreName" + i.ToString());
                int scoreVal = PlayerPrefs.GetInt("ScoreVal" + i.ToString());
                Score nextScore = new Score();
                nextScore.scoreName = scoreName;
                nextScore.scoreVal = scoreVal;
                nextScore.newHighscoreFlag = false;

                highScoreList.Add(nextScore);
            }
            else
                break;
        }
    }

    void SetHighscores()
    {
        for(int i = 0; i < highScoreList.Count; i++)
        {
            Score nextScore = highScoreList[i];
            PlayerPrefs.SetString("ScoreName" + i.ToString(), nextScore.scoreName);
            PlayerPrefs.SetInt("ScoreVal" + i.ToString(), nextScore.scoreVal);
        }
    }

    bool IsNewHighscore(int score)
    {
        Score newHighscore = new Score();
        newHighscore.scoreVal = score;
        newHighscore.scoreName = "";
        newHighscore.newHighscoreFlag = true;
        //Check higschore table and do a sorted insertion, set the bool to true for new highscore to grab the initial information
        if(highScoreList.Count == 0)
        {
            highScoreList.Insert(0, newHighscore);
            return true;
        }
        for (int i = 0; i < highScoreList.Count; i++)
        {
            if(highScoreList[i].scoreVal < score)
            {
                //insert new score
                
                highScoreList.Insert(i, newHighscore);

                return true;
            }
            if(i == highScoreList.Count-1 && i < 9)
            {
                //at the end of the list, and the list is less than 10, if we didn't break before now, then we need to add it to the list and break
                highScoreList.Add(newHighscore);
                return true;
            }
        }
        return false;
    }

    IEnumerator SpawnPlayerDelayed(float fWait)
    {
        yield return new WaitForSeconds(fWait);

        playerControl.gameObject.SetActive(true);
        playerControl.ResetRespawn();

        yield return null;
    }

    void BurstWipe(Vector3 positionToBurst)
    {
        //Reset radius to 1, enable, then lerp the radius of the collider over a few seconds to wipe the screen, then disable
        burstWipeCollider.radius = 1;
        burstWipeCollider.gameObject.transform.position = positionToBurst;
        burstWipeCollider.enabled = true;

        //Animate it
        StartCoroutine(AnimateBurst(1.0f, 0.1f));
    }

    IEnumerator AnimateBurst(float fDuration, float intervalSmoothing)
    {
        float radiusInterval = 80 * (intervalSmoothing / fDuration);
        while(fDuration > 0.0f)
        {
            fDuration -= intervalSmoothing;
            burstWipeCollider.radius += radiusInterval;
            yield return new WaitForSeconds(intervalSmoothing);
        }
        burstWipeCollider.enabled = false;
        yield return null;
    }

    public void IncrementScore(int amount)
    {
        scoreStat.IncrementBy(amount);
    }

    public void RestartCurrentScene()
    {
        if (isPaused)
        {
            Time.timeScale = 1.0f;
            isPaused = false;
        }
        Debug.Log("Restarting scene: " + SceneManager.GetActiveScene().name.ToString());
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OnMainMenu()
    {
        if (isPaused)
        {
            Time.timeScale = 1.0f;
            isPaused = false;
        }
        SceneManager.LoadScene("MainMenu");
    }

    public void SetPause(bool val)
    {   
        if (val)
        {
            Time.timeScale = 0.0f;
            UI_PauseScreen.SetActive(true);
        }
        else
        {
            UI_PauseScreen.SetActive(false);
            Time.timeScale = 1.0f;
        }
        isPaused = val;
    }

    void Awake()
    {
        if (theManager != null)
            GameObject.Destroy(theManager);
        
        theManager = this;
        theManager.Init();
        if(isMainMenu)
        {
            GetHighscores();
        }
    }
}
