using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class SplashScreen : MonoBehaviour
{

    [SerializeField] RectTransform logoFoolhardy, logoFMOD, blackScreen;

   

    [SerializeField] LeanTweenType enterFoolhardy, enterFMOD, exitFoolhardy, exitFMOD,blackScreenScale;

    [SerializeField] UnityEvent onComplete;

    private void Start()
    {
        StartCoroutine(SplashScreenRoutine());
        DontDestroyOnLoad(gameObject);
    }
    IEnumerator SplashScreenRoutine()
    {
        yield return new WaitForSeconds(1f);
        LeanTween.moveX(logoFoolhardy, 0, 1f).setEase(enterFoolhardy);
        yield return new WaitForSeconds(2f);
        LeanTween.moveX(logoFMOD, 0, 1f).setEase(enterFMOD);
        yield return new WaitForSeconds(2f);
        LeanTween.moveX(logoFoolhardy, 1920, 1f).setEase(exitFoolhardy);
        yield return new WaitForSeconds(.5f);
        LeanTween.moveX(logoFMOD, 1920, 1f).setEase(exitFMOD);
        yield return new WaitForSeconds(1f);
        logoFoolhardy.gameObject.SetActive(false);
        logoFMOD.gameObject.SetActive(false);
        onComplete.Invoke();
        yield return new WaitForSeconds(.1f);
        LeanTween.scale(blackScreen,Vector2.zero, 1f).setEase(blackScreenScale);

    }
}
