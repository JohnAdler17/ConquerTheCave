using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Records : MonoBehaviour
{
    static Records instance; //static variables persist through all instances of a class

    [SerializeField] private int numLevels = 16;
    private int levelIndex = 0;

    [SerializeField] List<float> levelTimes;

    [SerializeField] private float gameTime = -1f;

    private int highScore = 0;

    [SerializeField] private int thisLevelIndex = -1;

    void Awake() {
      ManageSingleton();
      for (int i = 0; i < numLevels; i++) {
        levelTimes.Add(-1f);
      }
    }

    void ManageSingleton() {
      if (instance != null)
      {
        gameObject.SetActive(false);
        Destroy(gameObject);
      }
      else {
        instance = this;
        DontDestroyOnLoad(gameObject);
      }
    }

    void Start() {
      if (thisLevelIndex != -1) {
        levelIndex = thisLevelIndex;
      }
    }

    public void AddLevelTime(float time) {
      if (levelIndex >= numLevels) {
        levelIndex = 0;
      }
      if (time < levelTimes[levelIndex] || levelTimes[levelIndex] == -1f) {
        levelTimes[levelIndex] = time;
      }
    }

    public void UpdateGameTime(float time) {
      if (time < gameTime || gameTime == -1) {
        gameTime = time;
      }
    }

    public float ReturnGameTime() {
      return gameTime;
    }

    public void ResetLevelIndex() {
      levelIndex = 0;
    }

    public float ReturnLevelTime(int levelIndex) {
      //Debug.Log(levelIndex);
      return levelTimes[levelIndex];
    }

    public void IncrementLevelIndex() {
      levelIndex += 1;
    }

    public int ReturnLevelIndex() {
      return levelIndex;
    }

    public int ReturnNumLevels() {
      return numLevels;
    }

    public void SetHighScore(int score) {
      if (score > highScore) {
        highScore = score;
      }
    }

    public int ReturnHighScore() {
      return highScore;
    }
}
