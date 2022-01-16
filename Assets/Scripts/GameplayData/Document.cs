using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay
{
    [System.Serializable]
    struct TargetData
    {
        public GameObject target;
        public string name; // key
    }

    public class Document : MonoBehaviour
    {
        [SerializeField] List<TargetData> targets = new List<TargetData>();

        internal ConcreteImmigrant immigrant;

        public void AssignTextValue(Text text, object obj)
        {
            {
                string value = obj as string;
                if (value != null)
                {
                    text.text = value;
                }
            }

            if (obj is Date)
            {
                Date d = (Date) obj;
                text.text = DataConversions.ToString(d);
            }

            if (obj is int)
            {
                int d = (int)obj;
                text.text = DataConversions.ToString(d);
            }
        }

        public void ToTarget()
        {
            foreach (TargetData targetData in targets)
            {
                Text text = targetData.target.GetComponent<Text>();
                Image image = targetData.target.GetComponent<Image>();

                InfoData d;
                bool found = immigrant.GetInfoData(targetData.name, out d);
                if (!found)
                    continue;

                if (text != null)
                {
                    AssignTextValue(text, d.value);
                }

                if (image != null)
                {
                    Sprite value = d.value as Sprite;
                    if (value != null)
                        image.sprite = value;
                }
            }
        }

    }
}