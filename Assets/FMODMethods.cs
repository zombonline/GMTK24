using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FMODMethods : MonoBehaviour
{
    public void ChangeMusicState(int value)
    {
        FindObjectOfType<FMODController>().ChangeMusicState(value);
    }
    public void PlaySFX(string name)
    {
        FMODController.PlaySFX(name);
    }
    public void UpdateMusicVolume(float val)
    {
        FMODController.SetMusicVolume(val);
    }
    public void UpdateSFXVolume(float val)
    {
        FMODController.SetSFXVolume(val);
    }
}