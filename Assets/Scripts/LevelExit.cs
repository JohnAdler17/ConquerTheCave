using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; //must include scene management namespace

public class LevelExit : MonoBehaviour
{
    [SerializeField] float levelLoadDelay = 1f;

    [SerializeField] private LevelCompleteScreen levelCompleteScreen;

    [SerializeField] private GameTimer LevelTimer;
    private GameTimer TotalGameTimer;
    private Records records;

    [SerializeField] private PlayerMovement player;

    [SerializeField] private GameObject continueButton;

    [SerializeField] private MenuSelector menuSelector;

    [SerializeField] private AudioSource levelCompleteAudio;

    [SerializeField] private bool isLastLevel = false;


    void Awake() {
      TotalGameTimer = GameObject.FindWithTag("GameTimer").GetComponent<GameTimer>();
      records = GameObject.FindWithTag("Records").GetComponent<Records>();
    }

    IEnumerator LoadNextLevel() {
        yield return new WaitForSecondsRealtime(levelLoadDelay);
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex; //stores current Scene index
        int nextSceneIndex = currentSceneIndex + 1;

        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings) {
          nextSceneIndex = 0;
        }

        FindObjectOfType<ScenePersist>().ResetScenePersist();
        SceneManager.LoadScene(nextSceneIndex);
    }

    public void LoadTheNextLevel() {
      records.IncrementLevelIndex();
      if (!isLastLevel) {
        TotalGameTimer.StartGameTimerWithDelay(0.5f);
      }
      else {
        // Load the end game screen and destroy the game timer from there
        //Destroy(TotalGameTimer.gameObject);
      }
      StartCoroutine(LoadNextLevel());
    }

    void OnTriggerEnter2D(Collider2D other) {
      if (other.tag == "Player" && player.ReturnAliveState()) {

        LevelTimer.StopGameTimer();

        TotalGameTimer.StopGameTimer();

        records.AddLevelTime(LevelTimer.GetGameTime());

        levelCompleteScreen.gameObject.SetActive(true);

        levelCompleteScreen.SetLevelTime(LevelTimer.GetGameTime());

        levelCompleteScreen.SetGameTime(TotalGameTimer.GetGameTime());

        levelCompleteScreen.SetBestTime(records.ReturnLevelTime(records.ReturnLevelIndex()));

        menuSelector.SetSelectedObject(continueButton);

        if (isLastLevel) {
          records.UpdateGameTime(TotalGameTimer.GetGameTime());
        }

        player.DisablePlayerControls();

        levelCompleteAudio.Play();
      }
    }
}
