using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class News : MonoBehaviour
{
    public DateRule dateRule;

    public GameObject firstPageData;
    public List<Sprite> pages;
    int currentIndex = 0;

    Image image;

    private void Start()
    {
        image = GetComponent<Image>();
        UpdateNews();
    }

    private void UpdateNews()
    {
        Debug.Assert(pages.Count > 0);
        image.sprite = pages[currentIndex];

        firstPageData.SetActive(currentIndex == 0);
    }

    public void Next()
    {
        currentIndex++;
        if (currentIndex >= pages.Count)
            currentIndex = pages.Count - 1;
        UpdateNews();
    }

    public void Prev()
    {
        currentIndex--;
        if (currentIndex < 0)
            currentIndex = 0;
        UpdateNews();
    }
}
