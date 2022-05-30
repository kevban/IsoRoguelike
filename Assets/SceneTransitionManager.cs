using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class SceneTransitionManager : MonoBehaviour
{
    public Animator transitions;
    public float transitionTime = 1f;

    //only used in char select screen
    public Canvas charInfoCanvas;
    public Image charSprite;
    public TextMeshProUGUI charName;
    public TextMeshProUGUI charSkillDescription;

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
        StartCoroutine(LoadScene(3));
    }

    IEnumerator LoadScene(int buildIndex) {
        transitions.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(buildIndex);
    }

    public void SelectChar(int ceoNum) {
        GameManager.ceo = ceoNum;
        switch (ceoNum) {
            case 1:
                charName.text = "Mud Giant";
                charSprite.sprite = Resources.LoadAll<Sprite>("Char1")[0];
                charSkillDescription.text = "Active Skill: \n";
                break;
            case 2:
                charName.text = "Old man";
                charSprite.sprite = Resources.LoadAll<Sprite>("Char2")[0];
                charSkillDescription.text = "Active Skill: \nYour next 3 rolls are (1)";
                break;
            default:
                break;
        }
        charInfoCanvas.gameObject.SetActive(true);
    }
    public void DeselectChar() { 
        charInfoCanvas.gameObject.SetActive(false);
    }

}
