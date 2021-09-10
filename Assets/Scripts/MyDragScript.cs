using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class DragAndDropManager
{
    List<MyDragScript> draggables = new List<MyDragScript>();

    public void SetPriorityOrderOnTop(MyDragScript draggable)
    {
        draggable.image.transform.SetAsLastSibling();
    }

    public void RegisterDraggable(MyDragScript draggable)
    {
        //draggables.Add(draggable);
    }

    public void UnregisterDraggable(MyDragScript draggable)
    {
        //draggables.Remove(draggable);
    }
};


public class MyDragScript : MonoBehaviour
{
    bool isDragged = false;
    Vector3 lastPosition; 

    internal UnityEngine.UI.Image image = null;

    static DragAndDropManager dragAndDropManager = new DragAndDropManager();

    // Areas must have their anchor at their center
    public List<RectTransform> dragArea = new List<RectTransform>();

    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<UnityEngine.UI.Image>();
        dragAndDropManager.RegisterDraggable(this);
    }

    private void OnDestroy()
    {
        dragAndDropManager.UnregisterDraggable(this);
    }

    // Update is called once per frame
    void Update()
    {
        if (isDragged /*&& IsInDraggableArea(Input.mousePosition)*/)
        {
            transform.position = Input.mousePosition + delta; 
        }
    }

    Vector3 delta;

    public void OnMouseDown()
    {
        lastPosition = image.transform.position;
        isDragged = true;
        dragAndDropManager.SetPriorityOrderOnTop(this);

        delta = transform.position - Input.mousePosition;
    }

    bool IsPointInDraggableArea(Vector3 point)
    {
        foreach (RectTransform im in dragArea)
        {
            float left = im.anchoredPosition.x - im.rect.width / 2;
            float right = im.anchoredPosition.x + im.rect.width / 2;
            float top = im.anchoredPosition.y + im.rect.height / 2;
            float bottom = im.anchoredPosition.y - im.rect.height / 2;

            if (point.x >= left 
                && point.x <= right
                && point.y >= bottom
                && point.y <= top)
                return true;
        }
        return false;
    }

    bool IsInDraggableArea(Vector3 imagePos)
    {
        Vector3 p1 = imagePos + new Vector3(- image.rectTransform.rect.width / 2f, - image.rectTransform.rect.height / 2f);
        Vector3 p2 = imagePos + new Vector3(- image.rectTransform.rect.width / 2f, image.rectTransform.rect.height / 2f);
        Vector3 p3 = imagePos + new Vector3(image.rectTransform.rect.width / 2f, - image.rectTransform.rect.height / 2f);
        Vector3 p4 = imagePos + new Vector3(image.rectTransform.rect.width / 2f, image.rectTransform.rect.height / 2f);

        return IsPointInDraggableArea(p1) && IsPointInDraggableArea(p2) && IsPointInDraggableArea(p3) && IsPointInDraggableArea(p4);
    }

    public void OnMouseUp()
    {
        isDragged = false;
        if (!IsInDraggableArea(image.rectTransform.anchoredPosition))
        {
            transform.position = lastPosition;
        }

    }
}
