using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatsBox : MonoBehaviour
{
    Transform player;
    
    private void Awake()
    {
        player = FindObjectOfType<PlayerMovement>().transform;
    }

    public void Update()
    {
        bool playerLow = (player.position.y < -Camera.main.ScreenToWorldPoint(new Vector2(0, Camera.main.pixelHeight)).y / 2f);
        if (playerLow)
        {

            foreach (Image img in GetComponentsInChildren<Image>())
            {
                img.color = new Color(img.color.r, img.color.g, img.color.b, 0.2f);
            }
            foreach (TextMeshProUGUI textElement in GetComponentsInChildren<TextMeshProUGUI>())
            {
                textElement.color = new Color(textElement.color.r, textElement.color.g, textElement.color.b, 0.2f);
            }
        }
        else
        {
            foreach (Image img in GetComponentsInChildren<Image>())
            {
                img.color = new Color(img.color.r, img.color.g, img.color.b, 1f);
            }
            foreach (TextMeshProUGUI textElement in GetComponentsInChildren<TextMeshProUGUI>())
            {
                textElement.color = new Color(textElement.color.r, textElement.color.g, textElement.color.b, 1f);
            }
        }

    }
}
