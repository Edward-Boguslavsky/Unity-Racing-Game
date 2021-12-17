using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GarageController : MonoBehaviour
{
    public ProgressInfoController progressCont;
    public ScrollController scrollCont;

    public Text carNameTag;

    public Button buttonSelect;

    public GameObject customOptions;
    public GameObject customOptionsColors;

    public Image[] customOptionsColorPreviews;

    Animator[] carAnimators = new Animator[3];

    // Start is called before the first frame update
    void Start()
    {
        progressCont.setMoney(GameManager.player.money);

        for (int i = 0; i < scrollCont.cars.Length; i++)
            carAnimators[i] = scrollCont.cars[i].GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        carNameTag.text = ResourceLoader.carNames[scrollCont.focusIndex];
    }

    public void setOptionsState(int stateIndex)
    {
        switch(stateIndex)
        {
            case 0:
                {
                    buttonSelect.gameObject.SetActive(true);
                    customOptions.SetActive(false);

                    scrollCont.scrollRect.horizontal = true;

                    for (int i = 0; i < scrollCont.cars.Length; i++)
                    {
                        if (i != scrollCont.focusIndex)
                            scrollCont.cars[i].gameObject.SetActive(true);
                        else
                        {
                            carAnimators[i].enabled = true;
                            carAnimators[i].SetTrigger("FocusOut");
                            scrollCont.cars[i].color = ResourceLoader.carColors[55];

                            int index = i;
                            void disableAnimator() { carAnimators[index].enabled = false; }
                            StartCoroutine(DelayFunction(disableAnimator, 1f / 5f));
                        }
                    }

                    break;
                }
            case 1:
                {
                    buttonSelect.gameObject.SetActive(false);
                    customOptions.SetActive(true);
                    customOptionsColors.SetActive(false);

                    scrollCont.scrollRect.horizontal = false;
                    scrollCont.setFocus();

                    for (int i = 0; i < scrollCont.cars.Length; i++)
                    {
                        if (i != scrollCont.focusIndex)
                            scrollCont.cars[i].gameObject.SetActive(false);
                        else
                        {
                            if (!carAnimators[i].enabled)
                            {
                                carAnimators[i].enabled = true;
                                carAnimators[i].SetTrigger("FocusIn");
                            }
                        }
                    }

                    for (int i = 0; i < customOptionsColorPreviews.Length; i++)
                        customOptionsColorPreviews[i].color = ResourceLoader.getStockColorByIndex(scrollCont.focusIndex, i);

                    break;
                }
            case 2:
                {
                    customOptions.SetActive(false);
                    customOptionsColors.SetActive(true);
                    break;
                }
        }
    }

    public void setColor(int colorIndex)
    {
        scrollCont.cars[scrollCont.focusIndex].color = customOptionsColorPreviews[colorIndex].color;
    }

    public void loadScene()
    {
        GameManager.player.carData.chassisIndex = scrollCont.focusIndex;
        GameManager.player.carData.chassisColor = scrollCont.cars[scrollCont.focusIndex].color;
        GameSceneManager.sceneManager.loadScene("GameScene");
    }

    IEnumerator DelayFunction(System.Action function, float delay)
    {
        yield return new WaitForSeconds(delay);
        function();
    }
}
