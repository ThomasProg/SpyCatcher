using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Document
{
    public GameObject linkerPrefab;
    public Transform parent;
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

    public override void InstantiateDocument()
    {
        PassportLinker linker = GameObject.Instantiate(linkerPrefab, parent).GetComponent<PassportLinker>();
        linker.SetData(this);
    }
};

public class WorkerCard : Document
{
    public string name;
    public Planet planet;
    public string sex;
    public Date expirationDate;
    public Date birthdate;
    public string birthPlace;
    public Sprite photo;
    public Sprite agencySeal;

    public override void InstantiateDocument()
    {
        WorkerCardLinker linker = GameObject.Instantiate(linkerPrefab, parent).GetComponent<WorkerCardLinker>();
        linker.SetData(this);
    }
};

public class DeliveryCard : Document
{
    public Sprite photo;
    public string name;
    public string agency;
    public string cargo;
    public Planet originPlanet;
    public string signature;

    public override void InstantiateDocument()
    {
        DeliveryCardLinker linker = GameObject.Instantiate(linkerPrefab, parent).GetComponent<DeliveryCardLinker>();
        linker.SetData(this);
    }
};

public class DiplomaticLetter : Document
{
    public string name;
    public Sprite seal;
    public Date creationDate; // The date when the card was made
    public string creationCity; // The city the card was made in

    public override void InstantiateDocument()
    {
        DiplomaticLetterLinker linker = GameObject.Instantiate(linkerPrefab, parent).GetComponent<DiplomaticLetterLinker>();
        linker.SetData(this);
    }
};

public class WeaponLicense : Document
{
    public string name;
    public Date birthdate;
    public string birthplace;
    public string sex;
    public Date expirationDate;
    public string category;
    public Sprite photo;

    public override void InstantiateDocument()
    {
        WeaponLicenseLinker linker = GameObject.Instantiate(linkerPrefab, parent).GetComponent<WeaponLicenseLinker>();
        linker.SetData(this);
    }
};

public class Immigrant : MonoBehaviour
{
    public string race;
    public List<Document> documents = new List<Document>();
    float creationTime;
    bool haveDocumentsSpawned = false;

    public Animation anim;
    public AudioSource audioSource;

    public bool isSpy = false;

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

        anim = GetComponent<Animation>();
        audioSource = GetComponent<AudioSource>();

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
