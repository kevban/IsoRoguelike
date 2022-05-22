using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HBD : MonoBehaviour
{
    public int curScreen = 0;
    public GameObject sceen1;
    public GameObject sceen2;
    public GameObject sceen3;
    public GameObject[] canvasToHide;
    public GameObject[] particleSystems;
    public AudioManager hbdAudioManager;
    public void NextScreen() {
        if (curScreen == 0)
        {
            for (int i = 0; i < canvasToHide.Length; i++) { 
                canvasToHide[i].SetActive(false);
            }
            sceen1.SetActive(false);
            sceen2.SetActive(true);
        }
        else if (curScreen == 1) {
            sceen2.SetActive(false);
            sceen3.SetActive(true);
            for (int i = 0; i < particleSystems.Length; i++) {
                particleSystems[i].SetActive(true);
            }
            hbdAudioManager.Play("hbd");
        }
        curScreen++;
    }
}
