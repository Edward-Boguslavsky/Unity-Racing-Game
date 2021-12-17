using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollController : MonoBehaviour
{

    public ScrollRect scrollRect;

    public Image[] cars = new Image[3];

    public int focusIndex = 0;

    void Start()
    {
        for (int i = 0; i < cars.Length; i++)
            cars[i] = transform.Find("Content_Wrapper/Car_Wrapper_" + (i + 1).ToString() + "/Car").GetComponent<Image>();
    }

    void Update()
    {
        focusIndex = Mathf.RoundToInt(Mathf.Clamp01(scrollRect.normalizedPosition.x) * (cars.Length - 1));

        setScrollEffects();
    }

    public void updateFocus(Vector2 scrollPos)
    {
        if (!Input.GetMouseButton(0) && Mathf.Abs(scrollRect.velocity.x) < 100f)
            setFocus();
    }

    public void setFocus()
    {
        scrollRect.StopMovement();
        scrollRect.normalizedPosition = new Vector2((float)focusIndex / (cars.Length - 1), 0);
    }

    void setScrollEffects()
    {
        foreach (Image car in cars)
        {
            float alpha = 1 - Mathf.Clamp(Mathf.Abs(car.transform.position.x), 0f, 8f) / 8f;
            setAlpha(car, alpha);
            car.transform.localScale = new Vector3(alpha, alpha, 1f);
        }
    }

    void setAlpha(Image img, float a)
    {
        img.color = new Color(img.color.r, img.color.g, img.color.b, a);
    }
}
