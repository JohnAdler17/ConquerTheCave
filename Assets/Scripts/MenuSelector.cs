using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuSelector : MonoBehaviour
{
  public void SetSelectedObject(GameObject selectedObject) {
    StartCoroutine(SetFirstSelected(selectedObject));
  }

  IEnumerator SetFirstSelected(GameObject firstSelectedObject) {
    EventSystem.current.SetSelectedGameObject(null);
    yield return new WaitForEndOfFrame();
    EventSystem.current.SetSelectedGameObject(firstSelectedObject);
  }
}
