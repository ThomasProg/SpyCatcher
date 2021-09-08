using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct RaceData
{
    public System.String race;
    public Date minBirthday;
    public Date maxBirthday;
    public List<string> firstNames;
    public List<string> lastNames;
    public Planet defaultPlanet;
    public Sprite planetsIcon;
    public int minWeight; // inclusive
    public int maxWeight; // exclusive
    public int minHeight; // inclusive
    public int maxHeight; // exclusive
    public List<string> possibleSexes;
    public List<string> cities;
    public List<Sprite> photos;
    public Sprite seal;

    public Animation enterAnim;
    public Animation allowedAnim;
    public Animation deniedAnim;

    public AudioClip enterAudio;
    public AudioClip allowedAudio;
    public AudioClip deniedAudio;

    public string GetRandomName()
    {
        Debug.Assert(firstNames != null && lastNames != null);
        Debug.Assert(firstNames.Count != 0);
        if (lastNames.Count == 0)
            return firstNames[Random.Range(0, firstNames.Count)];
        else 
            return firstNames[Random.Range(0, firstNames.Count)] + " " + lastNames[Random.Range(0, lastNames.Count)];
    }
};


public class ImmigrantRandomizer : MonoBehaviour
{
    [SerializeField]
    Date currentDate;

    [SerializeField]
    Date passportMaxExpirationDate;

    [SerializeField]
    GameObject passportPrefab;

    [SerializeField]
    GameObject docSpawnCanvas;
    [SerializeField]
    Vector3 docSpawnPos;

    [SerializeField]
    //Dictionary<Race, RaceData> allRacesData = new Dictionary<Race, RaceData>();
    List<RaceData> allRacesData = new List<RaceData>();

    Dictionary<System.String, RaceData> allRacesDataDic = new Dictionary<System.String, RaceData>();

    float workerCardRatio = 10f / 100;
    float deliveryCardRatio = 10f / 100;
    float diplomaticLetterRatio = 10f / 100;
    float weaponLicenseRatio = 10f / 100;

    private void Start()
    {
        foreach (RaceData data in allRacesData)
        {
            allRacesDataDic.Add(data.race, data);
        }
    }

    RaceData GetRaceData(System.String race)
    {
        RaceData raceData;
        bool isFound = allRacesDataDic.TryGetValue(race, out raceData);
        Debug.Assert(isFound);
        return raceData;
    }

    public bool IsValidWeight(System.String race, float weight)
    {
        RaceData raceData = GetRaceData(race);

        return raceData.minWeight < weight && weight < raceData.maxWeight;
    }

    public bool IsValidHeight(System.String race, float height)
    {
        RaceData raceData = GetRaceData(race);

        return raceData.minHeight < height && height < raceData.maxHeight;
    }

    string GetRandomName(RaceData raceData)
    {
        return raceData.GetRandomName();
    }

    Planet GetRandomValidPlanet(RaceData raceData)
    {
        return raceData.defaultPlanet;
    }

    int GetRandomValidWeight(RaceData raceData)
    {
        return Random.Range(raceData.minWeight, raceData.maxWeight);
    }
    int GetRandomValidHeight(RaceData raceData)
    {
        return Random.Range(raceData.minHeight, raceData.maxHeight);
    }

    string GetRandomSex(RaceData raceData)
    {
        return raceData.possibleSexes[Random.Range(0, raceData.possibleSexes.Count)];
    }

    static Date GetRandomDate(Date min, Date max)
    {
        Date randDate;
        randDate.year = Random.Range(min.year, max.year + 1);
        
        int minMonth = 1;
        if (randDate.year == min.year)
            minMonth = min.month;
        int maxMonth = 12;
        if (randDate.year == max.year)
            maxMonth = max.month;
        randDate.month = Random.Range(minMonth, maxMonth);

        int minDay = 1;
        if (randDate.year == min.year && randDate.month == min.month)
            minDay = min.day;
        int maxDay = 28;
        if (randDate.year == max.year && randDate.month == max.month)
            maxDay = max.day;
        randDate.day = Random.Range(minDay, maxDay);

        return randDate;
    }

    Sprite GetValidPlanetIcon(RaceData raceData)
    {
        //Sprite icon;
        //bool wasFound = planetsIcon.TryGetValue(planet, out icon);
        //Debug.Assert(wasFound);
        return raceData.planetsIcon;
    }

    string GetRandomPlace(RaceData raceData)
    {
        return raceData.cities[Random.Range(0, raceData.cities.Count)];
    }

    Sprite GetRandomPhoto(RaceData raceData)
    {
        return raceData.photos[Random.Range(0, raceData.photos.Count)];
    }

    Sprite GetValidSeal(RaceData raceData)
    {
        return raceData.seal;
    }

    Passport GetValidRandomPassport(System.String race, RaceData raceData)
    {
        Passport passport = new Passport();
        passport.name = GetRandomName(raceData);
        passport.planet = GetRandomValidPlanet(raceData);
        passport.race = race;
        passport.weight = GetRandomValidWeight(raceData);
        passport.height = GetRandomValidHeight(raceData);
        passport.sex = GetRandomSex(raceData);
        passport.birthdate = GetRandomDate(raceData.minBirthday, raceData.maxBirthday);
        passport.birthPlace = GetRandomPlace(raceData);
        passport.expirationDate = GetRandomDate(currentDate, passportMaxExpirationDate);

        passport.planetIcon = GetValidPlanetIcon(raceData);
        passport.planetSeal = GetValidSeal(raceData);
        passport.photo = GetRandomPhoto(raceData);

        passport.linkerPrefab = passportPrefab;
        docSpawnCanvas.transform.position = docSpawnPos;
        passport.parent = docSpawnCanvas.transform;

        return passport;
    }

    public T GetRandomEnum<T>()
    {
        System.Array values = System.Enum.GetValues(typeof(T));
        return (T) values.GetValue(Random.Range(0, values.Length));
    }

    System.String GetRandomRace()
    {
        return allRacesData[Random.Range(0, allRacesData.Count)].race;
    }

    public void SetRandomData(Immigrant immigrant)
    {
        //Init();
        System.String race = GetRandomRace();
        RaceData raceData = GetRaceData(race);
        immigrant.documents.Add(GetValidRandomPassport(race, raceData));

        
    }
}
