using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Document
{ 
    public virtual void InstantiateDocument()
    {

    }
};

public class Passport : Document
{
    public string name;
    public Planet planet;
    public string race;
    public int weight;
    public int height;
    public string sex;
    public Date birthdate;
    public Date expirationDate;
    public string birthPlace; // city

    public Sprite planetIcon;
    public Sprite photo;
    public Sprite planetSeal;

    public GameObject linkerPrefab;
    public Transform parent;

    public override void InstantiateDocument()
    {
        PassportLinker linker = GameObject.Instantiate(linkerPrefab, parent).GetComponent<PassportLinker>();
        linker.SetData(this);
    }
};

public class WorkerCard : Document
{
    public Sprite photo;
    public string name;
    public string sex;
    public Date birthdate;
    public Date expirationDate;
    public City birthPlace;
    public string agency;
};

public class DeliveryCard : Document
{
    public Sprite photo;
    public string name;
    public string agency;
    public string cargo;
    public Planet originPlanet;
    public string signature;
};

public class DiplomaticLetter : Document
{
    public string name;
    public Sprite seal;
    public Date creationDate; // The date when the card was made
    public City creationCity; // The city the card was made in
};

public class WeaponLicense : Document
{
    public Sprite sprite;
    public string name;
    public Date birthdate;
    public string sex;
    public Date expirationDate;
    public City birthplace;
    public string category;
};

public class Immigrant : MonoBehaviour
{
    public List<Document> documents = new List<Document>();
    float creationTime;
    bool haveDocumentsSpawned = false;


    public void InstantiateDocuments()
    {
        foreach (Document doc in documents)
        {
            doc.InstantiateDocument();
        }
    }

    private void Start()
    {
        creationTime = 0f;
    }

    private void Update()
    {
        if (Time.time > creationTime + 1f && !haveDocumentsSpawned)
        {
            haveDocumentsSpawned = true;
            InstantiateDocuments();
        }
    }
}

public enum ImmigrantType
{ 
    Transit,
    Worker, 
    Delivery, 
    Diplomatic,
    Immigrant
}
