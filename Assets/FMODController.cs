using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
public class FMODController : MonoBehaviour
{
    static FMOD.Studio.EventInstance pauseSnapshot;

    static List<FMOD.Studio.EventInstance> loopingInstances = new List<FMOD.Studio.EventInstance>();

    StudioEventEmitter instance;

    private void Awake()
    {
        instance = GetComponent<StudioEventEmitter>();
        pauseSnapshot = RuntimeManager.CreateInstance("snapshot:/Pause");

    }
    public void ChangeMusicState(int value)
    {
        instance.SetParameter("GameState", value, false);
    }
    public void PlayPauseSnaphot()
    {
        pauseSnapshot.start();
    }
    public void StopPauseSnapshot()
    {
        pauseSnapshot.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }
    public void ChangeDangerParameter(float value)
    {
        instance.SetParameter("Danger", value, false);
    }
    public static void PlaySFX(string val, string param = null, int paramVal = 0)
    {
        var newAudioEvent = RuntimeManager.CreateInstance(val);

        newAudioEvent.start();
        if (param == null) { return; }
        newAudioEvent.setParameterByName(param, paramVal);
    }
    public static void PlaySFXNoParams(string val)
    {
        var newAudioEvent = RuntimeManager.CreateInstance(val);
        newAudioEvent.start();
    }

    public static void StartLoopedSFX(string val)
    {
        var audioEvent = RuntimeManager.CreateInstance(val);
        audioEvent.start();
        loopingInstances.Add(audioEvent);
    }

    public static void StopLoopedSFX(string val)
    {
        Debug.Log("Stopping looped SFX");
        if(loopingInstances.Count == 0) { return; }
        foreach (var audioEvent in loopingInstances)
        {
            audioEvent.getDescription(out FMOD.Studio.EventDescription eventDesc);
            eventDesc.getPath(out string path);
            if (path == val)
            {
                audioEvent.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
                loopingInstances.Remove(audioEvent);
                break;
            }
        }
    }

}