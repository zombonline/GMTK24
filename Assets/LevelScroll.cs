//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;

//public class LevelScroll : MonoBehaviour
//{
//    [SerializeField] private float initialScrollSpeed, scrollSpeedIncreaseAmount, scrollSpeedIncreaseFrequency;
//    private float scrollSpeed;
//    private float normalScrollSpeed;
//    private float scrollSpeedIncreaseTimer;

//    private Transform player;
//    [SerializeField] private float playerAheadScrollSpeed;
//    private bool playerAhead = false, playerAheadLastFrame;   
//    LTDescr tween = null;

//    bool canScroll = true;
//    [SerializeField] Image dangerImage;
//    private void Awake()
//    {
//        player = GameObject.FindGameObjectWithTag("Player").transform;
//    }

//    private void Start()
//    {
//        normalScrollSpeed = initialScrollSpeed;
//        scrollSpeed = normalScrollSpeed;
//        scrollSpeedIncreaseTimer = scrollSpeedIncreaseFrequency;
//    }

//    void Update()
//    {
//        if (!canScroll) {return; }
//        if (LevelManager.GetIsPaused()) { return; }
//        transform.position = Vector3.MoveTowards(transform.position, 
//            transform.position + Vector3.down, scrollSpeed * Time.deltaTime);

//        scrollSpeedIncreaseTimer -= Time.deltaTime;
//        if(scrollSpeedIncreaseTimer <= 0)
//        {
//            normalScrollSpeed += scrollSpeedIncreaseAmount;
//            scrollSpeedIncreaseTimer = scrollSpeedIncreaseFrequency;
//        }

//        playerAhead = (player.position.y > Camera.main.ScreenToWorldPoint(new Vector2(0, Camera.main.pixelHeight)).y / 2f);
//        if(playerAhead && !playerAheadLastFrame)
//        {
//            if (normalScrollSpeed > playerAheadScrollSpeed) { return; }
//            if (tween != null) { LeanTween.cancel(tween.id); }
//            tween = LeanTween.value(scrollSpeed, playerAheadScrollSpeed, 3f).setOnUpdate(SetScrollSpeed);
//        }
//        else if (!playerAhead && playerAheadLastFrame)
//        {
//            if (tween != null) { LeanTween.cancel(tween.id); }
//            tween = LeanTween.value(scrollSpeed, normalScrollSpeed, .25f).setOnUpdate(SetScrollSpeed);
//        }

//        playerAheadLastFrame = playerAhead;
//        if (PositionOnScreen.IsBetween(player.position, new Vector2(0, -6), new Vector2(0, -9)))
//        {
//            //float intensity = PositionOnScreen.GetScaleBetweenTwoPoints(player.position, )
//            //FindObjectOfType<FMODController>().ChangeDangerParameter(PositionOnScreen.get);
//        }
//        //get a scale of where player is between -6 and -9 (-6 is a scale of 0, -9 is a scale of 1)
//        float playerLowScale = Mathf.Clamp((-6 - player.position.y) / 3, 0, 1);
//        FindObjectOfType<FMODController>().ChangeDangerParameter(playerLowScale);
//        dangerImage.color = new Color(dangerImage.color.r, dangerImage.color.g, dangerImage.color.g, playerLowScale);
//    }
//    public void SetScrollSpeed(float val)
//    {
//        scrollSpeed = val;
//    }
//    public float GetCurrentScrollSpeed()
//    {
//        return scrollSpeed;
//    }
//    public void StopScrolling()
//    {
//        canScroll = false;
//    }


//}
