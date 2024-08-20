using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroller : MonoBehaviour

{
    [SerializeField] float backgroundScrollSpeed;
    Material myMaterial;
    Vector2 offSet;


    // Start is called before the first frame update
    void Start()
    {
        myMaterial = GetComponent<Renderer>().material;
        offSet = new Vector2(0f, backgroundScrollSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        if(LevelManager.GetIsPaused()) { return; }
        //move offset by speed every second
        offSet = new Vector2(0f, backgroundScrollSpeed);
        myMaterial.mainTextureOffset += offSet * Time.deltaTime;
    }
}