using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameSession : MonoBehaviour
{
    [SerializeField] int playerLives = 3;
    [SerializeField] int score = 0;

    [SerializeField] TextMeshProUGUI livesText;
    [SerializeField] TextMeshProUGUI scoreText;

    GameObject gameTimer;

    Records records;

    private int numDeaths = 0;

    void Awake()
    {
        int numGameSessions = FindObjectsOfType<GameSession>().Length;
        if (numGameSessions > 1) {
          Destroy(gameObject);
        }
        else {
          DontDestroyOnLoad(gameObject);
        }
    }

    void Start()
    {
        livesText.text = playerLives.ToString();
        scoreText.text = score.ToString();
        gameTimer = GameObject.FindWithTag("GameTimer");
        records = GameObject.FindWithTag("Records").GetComponent<Records>();
    }

    public void ProcessPlayerDeath() {
      if (playerLives > 1) {
        TakeLife();
      }
      else {
        ResetGameSession();
      }
    }

    public void AddToScore(int pointsToAdd) {
      score += pointsToAdd;
      scoreText.text = score.ToString();
    }

    public int ReturnScore() {
      return score;
    }

    public void AddToLives(int livesToAdd) {
      playerLives += livesToAdd;
      livesText.text = playerLives.ToString();
    }

    public int ReturnNumLives() {
      return playerLives;
    }

    public int ReturnNumDeaths() {
      return numDeaths;
    }

    void TakeLife() {
      playerLives--;
      numDeaths += 1;
      int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
      livesText.text = playerLives.ToString();
      StartCoroutine(LoadCurrentScene(currentSceneIndex));
    }

    IEnumerator LoadCurrentScene(int index) {
      yield return new WaitForSeconds(1f);
      SceneManager.LoadScene(index);
    }

    IEnumerator ReloadGame(int index) {
      yield return new WaitForSeconds(1f);
      if (FindObjectOfType<ScenePersist>() != null) {
        FindObjectOfType<ScenePersist>().ResetScenePersist();
      }
      Destroy(gameTimer);
      SceneManager.LoadScene(index);
      records.ResetLevelIndex();
      Destroy(gameObject);
    }

    public void ResetGameSession() {
      StartCoroutine(ReloadGame(0));
    }

}
