using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    [HideInInspector]
    public Sound curTrack;
    [HideInInspector]
    public Sound nextTrack;

    [HideInInspector]
    public List<Sound> shuffleOrder = new List<Sound>();
    [HideInInspector]
    public int trackNum;
    // Start is called before the first frame update
    private void Awake()
    {
        foreach (Sound s in sounds) { 
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    
    public void Play(string name) {
        foreach (Sound s in sounds) {
            if (s.name == name) {
                if (s.source != null)
                {
                    s.source.Play();
                    curTrack = s;
                }
               
            }
        }
    }

    public void Stop(string name)
    {
        foreach (Sound s in sounds)
        {
            if (s.name == name)
            {
                s.source.Stop();
            }
        }
    }

    public void StopAll() {
        foreach (Sound s in sounds)
        {
                s.source.Stop();
        }
    }

    public void  ChangeTrackAfterFinish(string name) {
        foreach (Sound s in sounds)
        {
            if (s.source.isPlaying) {
                curTrack = s;
                curTrack.source.loop = false;
            }
            if (s.name == name)
            {
                nextTrack = s;
            }
        }
        Invoke("ChangeTrack", 3f);
    }

    //play nextTrack after curTrack is finished
    void ChangeTrack() {
        if (curTrack.source.isPlaying)
        {
            Invoke("ChangeTrack", 3f);
        }
        else {
            curTrack = nextTrack;
            nextTrack.source.Play();
            Invoke("ChangeTrack", 3f);
        }
    }

    //play the next track in the queue after curTrack is finished.
    void NextTrack() {
        if (curTrack.source.isPlaying)
        {
            Invoke("NextTrack", 3f);
        }
        else
        {
            trackNum++;
            if (trackNum >= shuffleOrder.Count) { 
                trackNum = 0;
            }
            curTrack = shuffleOrder[trackNum];
            shuffleOrder[trackNum].source.Play();
            Invoke("NextTrack", 3f);
        }
        
    }

    //shuffle through all the tracks in the audioManager
    public void Shuffle() {
        List<Sound> temp = new List<Sound>();
        shuffleOrder = new List<Sound>();
        for (int i = 0; i < sounds.Length; i++) {
            temp.Add(sounds[i]);
        }
        for (int i = 0; i < sounds.Length; i++) {
            int randNum = Random.Range(0, temp.Count);
            shuffleOrder.Add(temp[randNum]);
            temp.RemoveAt(randNum);
        }
        trackNum = 0;
        Play(shuffleOrder[trackNum].name);
        NextTrack();
    }


}
