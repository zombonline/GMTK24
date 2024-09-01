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

        if (PositionOnScreen.IsAbove(player.position, Camera.main.pixelHeight / 4f))
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
        else
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
    }
}
