using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{
    public Animator transitions;
    public float transitionTime = 1f;

    // Start is called before the first frame update
    void Start()
    {
    }

    public void GoToMapSelect() {
        StartCoroutine(LoadScene(1));
    }

    public void GoToCharSelect() {
        StartCoroutine(LoadScene(2));
    }

    public void GoToMap1() {
        StartCoroutine(LoadScene(2));
    }

    IEnumerator LoadScene(int buildIndex) {
        transitions.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(buildIndex);
    }
}
