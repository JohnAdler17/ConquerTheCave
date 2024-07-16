using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{

  public void LoadSceneWithIndex(int index) {
    StartCoroutine(LoadSceneIndex(index));
  }

  public void SetSelectedObject(GameObject selectedObject) {
    StartCoroutine(SetFirstSelected(selectedObject));
  }

  IEnumerator SetFirstSelected(GameObject firstSelectedObject) {
    EventSystem.current.SetSelectedGameObject(null);
    yield return new WaitForEndOfFrame();
    EventSystem.current.SetSelectedGameObject(firstSelectedObject);
  }

  IEnumerator LoadSceneIndex(int index) {
    yield return new WaitForSeconds(0.5f);
    SceneManager.LoadScene(index);
  }
}
