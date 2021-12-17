using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlackoutTransition : MonoBehaviour
{

    public Image blackImage;

    public void startTransition(System.Action callback)
    {
        StartCoroutine(transition(callback));
    }

    IEnumerator transition(System.Action callback)
    {
        blackImage.gameObject.SetActive(true);
        blackImage.GetComponent<Animator>().SetTrigger("BlackoutIn");

        yield return new WaitForSeconds(0.5f);
        callback();

        yield return new WaitForSeconds(1f / 60f);
        blackImage.GetComponent<Animator>().SetTrigger("BlackoutOut");

        yield return new WaitForSeconds(0.5f);
        blackImage.gameObject.SetActive(false);

    }
}
