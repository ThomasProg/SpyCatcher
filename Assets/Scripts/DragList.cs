using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragList : MonoBehaviour
{
    // Areas must have their anchor at their center
    public List<RectTransform> dragArea = new List<RectTransform>();
    public static string dragListName = "UsableInterface";
}
