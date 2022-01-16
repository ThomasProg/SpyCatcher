using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
    public class ConcreteImmigrant : MonoBehaviour
    {
        public ImmigrantData immigrantData;
        public List<InfoData> data = new List<InfoData>();
        public List<Document> documents;

        public bool IsSpy
        {
            get;
            set;
        }    

        public bool GetInfoData(string name, out InfoData dOut)
        {
            foreach (InfoData d in data)
            {
                if (d.name == name)
                {
                    dOut = d;
                    return true;
                }
            }

            dOut = new InfoData();

            return false;
        }

        public void GenerateDocuments()
        {
            documents = immigrantData.GenerateDocuments(this);

            IsSpy = immigrantData.TryFalsify(documents);
        }

        public void SetRandomInfoData()
        {
            data = immigrantData.GetRandomInfoData();
        }

        public void RemoveDocuments()
        {
            foreach (Document doc in documents)
            {
                Destroy(doc.gameObject);
            }
            documents.Clear();
        }

        private Animator anim;
        private AudioSource audioSource;

        private void Awake()
        {
            anim = GetComponent<Animator>();
            audioSource = GetComponent<AudioSource>();
        }


        InfoData FindData(string name)
        {
            return data.Find(data => data.name == name);
        }

        public void OnAllowed()
        {
            anim.SetInteger("decision", 2);
            audioSource.clip = (AudioClip) FindData("enterSound").value;
            audioSource.Play();
        }

        public void OnDenied()
        {
            anim.SetInteger("decision", 1);
            audioSource.clip = (AudioClip)FindData("deniedSound").value;
            audioSource.Play();
        }
    }
}