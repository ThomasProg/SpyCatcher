using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct RaceData
{
    public Race race;
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

    public string GetRandomName()
    {
        Debug.Assert(firstNames != null && lastNames != null);
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
    //Dictionary<Race, RaceData> allRacesData = new Dictionary<Race, RaceData>();
    List<RaceData> allRacesData = new List<RaceData>();

    Dictionary<Race, RaceData> allRacesDataDic = new Dictionary<Race, RaceData>();

    private void Init()
    {
        foreach (RaceData data in allRacesData)
        {
            allRacesDataDic.Add(data.race, data);
            Debug.Log(data.race.ToString());
        }
    }

    RaceData GetRaceData(Race race)
    {
        RaceData raceData;
        bool isFound = allRacesDataDic.TryGetValue(race, out raceData);
        Debug.Assert(isFound);
        return raceData;
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
        
        int minMonth = 0;
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

    Passport GetValidRandomPassport(Race race, RaceData raceData)
    {
        Passport passport = new Passport();
        passport.name = GetRandomName(raceData);
        passport.planet = GetRandomValidPlanet(raceData);
        passport.race = race;
        passport.weight = GetRandomValidWeight(raceData);
        passport.height = GetRandomValidHeight(raceData);
        passport.sex = GetRandomSex(raceData);
        passport.birthdate = GetRandomDate(raceData.minBirthday, raceData.maxBirthday);
        passport.expirationDate = GetRandomDate(currentDate, passportMaxExpirationDate);
        passport.planetIcon = GetValidPlanetIcon(raceData);
        return passport;
    }

    public T GetRandomEnum<T>()
    {
        System.Array values = System.Enum.GetValues(typeof(T));
        return (T) values.GetValue(Random.Range(0, values.Length));
    }

    Race GetRandomRace()
    {
        return GetRandomEnum<Race>();
    }

    public void SetRandomData(Immigrant immigrant)
    {
        Init();
        Race race = GetRandomRace();
        race = Race.Human;
        RaceData raceData = GetRaceData(race);
        GetValidRandomPassport(race, raceData);

        
    }
}
