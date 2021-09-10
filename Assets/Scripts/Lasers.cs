using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Lasers : MonoBehaviour
{
    [SerializeField]
    List<Sprite> laserSprites;

    [SerializeField]
    List<Sprite> laserIdleSprites;

    [SerializeField]
    float spriteDelay = 0.1f;
    
    Image image;


    Coroutine coroutine;

    AudioSource audioSource;

    IEnumerator LasersAnim()
    {
        foreach (Sprite sprite in laserSprites)
        {
            image.sprite = sprite;
            yield return new WaitForSeconds(spriteDelay);
        }

        while (true)
        {
            foreach (Sprite sprite in laserIdleSprites)
            {
                image.sprite = sprite;
                yield return new WaitForSeconds(spriteDelay);
            }
        }
    }

    IEnumerator LasersAnimOut()
    {
        //foreach (Sprite sprite in laserSprites)
        for (int i = laserSprites.Count - 1; i >= 0; i--)
        {
            image.sprite = laserSprites[i];
            yield return new WaitForSeconds(spriteDelay);
        }
        gameObject.SetActive(false);
    }


    // Start is called before the first frame update
    void Awake()
    {
        image = GetComponent<Image>();
        audioSource = GetComponent<AudioSource>();
    }

    public void ActivateLasers()
    {
        gameObject.SetActive(true);
        if (coroutine != null)
            StopCoroutine(coroutine);
        coroutine = StartCoroutine(LasersAnim());
        audioSource.Play();
    }

    public void DeactivateLasers()
    {
        gameObject.SetActive(true);
        if (coroutine != null)
            StopCoroutine(coroutine);
        coroutine = StartCoroutine(LasersAnimOut());
    }
}

