using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(CanvasGroup))]
public class Tuto : MonoBehaviour
{
    CanvasGroup image;

    [SerializeField]
    float fadeDuration = 3;

    [SerializeField]
    float waitTime = 3;

    [SerializeField]
    InspectionManager inspectionManager;


    private void Awake()
    {
        image = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        StartCoroutine(FadeOut());
    }

    IEnumerator FadeOut()
    {
        yield return new WaitForSeconds(waitTime);

        for (float i = 0; i < fadeDuration; i += Time.deltaTime)
        {
            image.alpha = 1f - i / fadeDuration;
            yield return null;
        }

        inspectionManager.GenerateNewImmigrant();
        Destroy(image.gameObject);
    }

}
