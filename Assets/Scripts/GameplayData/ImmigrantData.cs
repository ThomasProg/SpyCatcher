using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
    public struct InfoData
    {
        public string name;   // Unique name
        public object value; // used to compare
        public string category; // used to compare info with other data / if null, only compares to same name

        public InfoData(string name, string value, string category = null)
        {
            this.name = name;
            this.value = value;
            this.category = category;
        }

        public InfoData(string name, object value, string category = null)
        {
            this.name = name;
            this.value = value;
            this.category = category;
        }
    };


    public class ImmigrantData : MonoBehaviour
    {
        [SerializeField] string sex;
        [SerializeField] string race;

        [SerializeField] int minHeight;
        [SerializeField] int maxHeight;

        [SerializeField] int minWeight;
        [SerializeField] int maxWeight;

        [SerializeField] Date minBirthday;
        [SerializeField] Date maxBirthday;

        [SerializeField] Sprite photo;
        [SerializeField] Sprite sprite;

        [SerializeField] AudioClip enterSound;
        [SerializeField] AudioClip deniedSound;

        internal ImmigrantManager manager;

        public virtual List<Document> GenerateDocuments(Gameplay.ConcreteImmigrant immigrant)
        {
            return manager.GenerateDefaultDocuments(immigrant);
        }

        public bool TryFalsify(List<Document> documents)
        {
            return manager.TryFalsify(documents);
        }

        public List<InfoData> GetRandomInfoData()
        {
            List<InfoData> data = new List<InfoData>();

            data.Add(new InfoData("sex", sex));
            data.Add(new InfoData("race", race));
            data.Add(new InfoData("height", Random.Range(minHeight, maxHeight)/*.ToString()*/));
            data.Add(new InfoData("weight", Random.Range(minWeight, maxWeight)/*.ToString()*/));
            data.Add(new InfoData("birthday", /*DataConversions.ToString(*/Date.GetRandomDate(minBirthday, maxBirthday))/*)*/);
            data.Add(new InfoData("photo", photo));
            data.Add(new InfoData("sprite", sprite));
            data.Add(new InfoData("enterSound", enterSound));
            data.Add(new InfoData("deniedSound", deniedSound));

            return data;
        }
    }
}