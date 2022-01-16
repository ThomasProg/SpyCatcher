using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImmigrantManager : MonoBehaviour
{
    [SerializeField]
    List<Gameplay.ImmigrantData> immigrants;

    [SerializeField]
    List<Gameplay.PlanetData> planets;


    Queue<int> immigrantsIndices = new Queue<int>();

    Gameplay.ConcreteImmigrant currentImmigrant;

    void SetRandomImmigrantsList()
    {
        List<int> immigrantsIndicesList = new List<int>();

        // Set indices to immigrants
        for (int i = 0; i < immigrants.Count; i++)
        {
            immigrantsIndicesList.Add(i);
        }

        // Shuffles indices
        for (int i = 0; i < immigrantsIndicesList.Count; i++)
        {
            int temp = immigrantsIndicesList[i];
            int randomIndex = Random.Range(i, immigrantsIndicesList.Count);
            immigrantsIndicesList[i] = immigrantsIndicesList[randomIndex];
            immigrantsIndicesList[randomIndex] = temp;
        }

        foreach (int index in immigrantsIndicesList)
        {
            immigrantsIndices.Enqueue(index);
        }

        Debug.Assert(immigrantsIndicesList.Count != 0);
    }

    void Start()
    {
        SetRandomImmigrantsList();

        currentImmigrant = GetConcreteImmigrant();
    }

    Gameplay.ImmigrantData GetNewImmigrantData()
    {
        int index = immigrantsIndices.Dequeue();
        immigrantsIndices.Enqueue(index);
        return immigrants[index];
    }


    [SerializeField]
    GameObject concreteImmigrantPrefab;
    [SerializeField]
    GameObject immigrantParent;
    Gameplay.ConcreteImmigrant GetConcreteImmigrant()
    {
        Gameplay.ConcreteImmigrant concrete = Instantiate(concreteImmigrantPrefab, immigrantParent.transform).GetComponent<Gameplay.ConcreteImmigrant>(); // new Gameplay.ConcreteImmigrant();

        concrete.immigrantData = GetNewImmigrantData();
        concrete.immigrantData.manager = this;

        concrete.SetRandomInfoData();

        concrete.GenerateDocuments();


        return concrete;
    }


    [SerializeField]
    GameObject doc1Prefab;
    [SerializeField]
    GameObject parent;
    public List<Gameplay.Document> GenerateDefaultDocuments(Gameplay.ConcreteImmigrant immigrant)
    {
        List<Gameplay.Document> list = new List<Gameplay.Document>();
        GameObject instance = Instantiate(doc1Prefab, parent.transform);
        Gameplay.Document comp = instance.GetComponent<Gameplay.Document>();
        comp.immigrant = immigrant;
        comp.ToTarget();
        list.Add(comp);
        return list;
    }


    public bool TryFalsify(List<Gameplay.Document> documents)
    {
        if (Random.Range(0, 100) < 50)
        {
            foreach (Gameplay.Document doc in documents)
            {
                doc.falsify();
            }

            return true;
        }
        return false;
    }

    [SerializeField]
    UnityEngine.UI.Text scoreText;
    int currentScore = 0;
    int CurrentScore
    {
        get
        {
            return currentScore;
        }
        set
        {
            currentScore = value;
            scoreText.text = value.ToString();
        }
    }


    [SerializeField]
    Lasers lasers;
    bool lockButtons = false;

    public void AllowAccess()
    {
        if (lockButtons || currentImmigrant == null)
            return;

        if (currentImmigrant.IsSpy)
        {
            CurrentScore -= 1;
        }
        else
            CurrentScore += 1;

        //RaceData raceData = immigrantRandomizer.GetRaceData(currentImmigrant.race);
        //currentImmigrant.anim.SetInteger("decision", 2);

        //currentImmigrant.audioSource.clip = immigrantRandomizer.GetCharacterData(currentImmigrant.photo).enterSound;

        currentImmigrant.OnAllowed();

        StartCoroutine(OnImmigrantLeave());
    }

    public void DenyAccess()
    {
        if (lockButtons || currentImmigrant == null)
            return;

        lasers.ActivateLasers();

        if (currentImmigrant.IsSpy)
        {
            CurrentScore += 1;
        }
        else
        {
            CurrentScore -= 1;
        }

        //RaceData raceData = immigrantRandomizer.GetRaceData(currentImmigrant.race);
        //currentImmigrant.anim.SetInteger("decision", 1);

        //currentImmigrant.audioSource.clip = immigrantRandomizer.GetCharacterData(currentImmigrant.photo).deniedSound;

        currentImmigrant.OnDenied();

        StartCoroutine(OnImmigrantLeave());
    }

    IEnumerator OnImmigrantLeave()
    {
        lockButtons = true;
        currentImmigrant.RemoveDocuments();
        //currentImmigrant.audioSource.Play();

        yield return new WaitForSeconds(3);

        currentImmigrant.RemoveDocuments();
        Destroy(currentImmigrant.gameObject);

        yield return new WaitForSeconds(0.5f);

        lasers.DeactivateLasers();

        yield return new WaitForSeconds(0.5f);

        //GenerateNewImmigrant();
        currentImmigrant = GetConcreteImmigrant();

        lockButtons = false;
    }
}
