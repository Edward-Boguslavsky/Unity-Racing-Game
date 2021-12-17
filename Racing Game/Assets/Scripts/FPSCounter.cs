using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FPSCounter : MonoBehaviour
{

    public int avgFrameRate;
    public Text display_Text;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float current = 0;
        current = (int)(1f / Time.unscaledDeltaTime);
        avgFrameRate = (int)current;
        display_Text.text = avgFrameRate.ToString() + " FPS";
        if (avgFrameRate >= 60)
            display_Text.color = new Color(0f, 1f, 0f);
        else if (avgFrameRate >= 30)
            display_Text.color = new Color(1f, 1f, 0f);
        else
            display_Text.color = new Color(1f, 0f, 0f);
    }
}
