using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct AgencyData
{
    public string name;
    public Sprite seal;
    public string sign; // signature
}


[System.Serializable]
public struct InvalidDataParams
{
    public int minWeight; // inclusive
    public int maxWeight; // exclusive
    public int minHeight; // inclusive
    public int maxHeight; // exclusive
    public Date minBirthday;
    public Date maxBirthday;
    public Date minExpirationDate;
    public Date minDocCreationDate;
    public Date maxDocCreationDate;
}


[System.Serializable]
public struct RaceData
{
    public InvalidDataParams invalidDataParams;

    public string race;
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
    //public List<Sprite> agencySeals;
    public List<AgencyData> agencies;
    public Sprite seal;

    public AnimationClip enterAnim;
    public AnimationClip allowedAnim;
    public AnimationClip deniedAnim;

    public string GetRandomName()
    {
        Debug.Assert(firstNames != null && firstNames.Count != 0);
        if (lastNames == null || lastNames.Count == 0)
            return firstNames[Random.Range(0, firstNames.Count)];
        else 
            return firstNames[Random.Range(0, firstNames.Count)] + " " + lastNames[Random.Range(0, lastNames.Count)];
    }
};

[System.Serializable]
public struct CharacterPhoto
{
    public Sprite photo;
    public Sprite real;
    public AudioClip enterSound;
    public AudioClip deniedSound;
};


[System.Serializable]
public struct PlanetData
{
    public Planet planet;
    public Sprite icon;
    public Sprite seal;
};


public class ImmigrantRandomizer : MonoBehaviour
{
    [SerializeField]
    public Date currentDate;

    [SerializeField]
    Date passportMaxExpirationDate;
    [SerializeField]
    Date workerCardMaxExpirationDate;

    [SerializeField]
    GameObject passportPrefab;
    [SerializeField]
    GameObject workerCardPrefab;
    [SerializeField]
    GameObject deliveryCardPrefab;
    [SerializeField]
    GameObject diplomaticLetterPrefab;
    [SerializeField]
    GameObject weaponLicensePrefab;

    [SerializeField]
    float errorPercent = 14;

    [SerializeField]
    GameObject docSpawnCanvas;
    [SerializeField]
    Vector3 docSpawnPos;

    [SerializeField]
    List<string> cargoList;

    [SerializeField]
    //Dictionary<Race, RaceData> allRacesData = new Dictionary<Race, RaceData>();
    List<RaceData> allRacesData = new List<RaceData>();

    Dictionary<System.String, RaceData> allRacesDataDic = new Dictionary<System.String, RaceData>();

    public List<CharacterPhoto> characterAndPhoto;
    public List<PlanetData> planetsData;

    public PlanetData GetPlanetData(Planet planet)
    {
        System.Predicate<PlanetData> predicate = (PlanetData p) => p.planet == planet;
        PlanetData data = planetsData.Find(predicate);
        return data;
    }

    [SerializeField] float workerCardPercent = 20;
    [SerializeField] float deliveryCardPercent = 20;
    //[SerializeField] float diplomaticLetterPercent = 7;
    [SerializeField] float weaponLicensePercent = 7;

    private void Awake()
    {
        foreach (RaceData data in allRacesData)
        {
            allRacesDataDic.Add(data.race, data);
        }
    }

    public RaceData GetRaceData(string race)
    {
        RaceData raceData;
        bool isFound = allRacesDataDic.TryGetValue(race, out raceData);
        Debug.Assert(isFound);
        return raceData;
    }

    public CharacterPhoto GetCharacterData(Sprite photo)
    {
        System.Predicate<CharacterPhoto> predicate = (CharacterPhoto p) => p.photo == photo;
        return characterAndPhoto.Find(predicate);
    }

    public bool IsValidWeight(string race, float weight)
    {
        RaceData raceData = GetRaceData(race);

        return raceData.minWeight < weight && weight < raceData.maxWeight;
    }

    public bool IsValidHeight(string race, float height)
    {
        RaceData raceData = GetRaceData(race);

        return raceData.minHeight < height && height < raceData.maxHeight;
    }

    public bool HasValidAge(string race, Date birthday)
    {
        RaceData raceData = GetRaceData(race);

        return raceData.maxBirthday.IsLessThan(birthday);
    }

    public bool HasValidPhoto(string race, string photoStr)
    {
        System.Predicate<Sprite> predicate = (Sprite s) => DataConversions.ToString(s) == photoStr;
        return GetRaceData(race).photos.Find(predicate) != null;
    }

    public bool IsCityOnPlanet(string city, string planet)
    {
        foreach (RaceData race in allRacesData)
        {
            if (DataConversions.ToString(race.defaultPlanet) == planet)
            {
                if (race.cities.Contains(city))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        return false;
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

    Passport GetValidRandomPassport(RaceData raceData)
    {
        Passport passport = new Passport();
        passport.name = GetRandomName(raceData);
        passport.planet = GetRandomValidPlanet(raceData);
        passport.race = raceData.race;
        passport.weight = GetRandomValidWeight(raceData);
        passport.height = GetRandomValidHeight(raceData);
        passport.sex = GetRandomSex(raceData);
        passport.birthdate = GetRandomDate(raceData.minBirthday, raceData.maxBirthday);
        passport.birthPlace = GetRandomPlace(raceData);
        passport.expirationDate = GetRandomDate(currentDate, passportMaxExpirationDate);

        passport.planetIcon = passport.planet;
        passport.planetSeal = passport.planet; // GetValidSeal(raceData);
        passport.photo = GetRandomPhoto(raceData);

        passport.linkerPrefab = passportPrefab;
        docSpawnCanvas.transform.position = docSpawnPos;
        passport.parent = docSpawnCanvas.transform;

        return passport;
    }

    AgencyData GetRandomAgency(RaceData raceData)
    {
        return raceData.agencies[Random.Range(0, raceData.agencies.Count)];
    }

    //Sprite GetRandomAgencySeal(RaceData raceData)
    //{
    //    return raceData.agencySeals[Random.Range(0, raceData.agencySeals.Count)];
    //}

    WorkerCard GetValidRandomWorkerCard(RaceData raceData, Passport passport, AgencyData agencyData)
    {
        WorkerCard workerCard = new WorkerCard();
        workerCard.name = passport.name;
        workerCard.planet = passport.planet;
        workerCard.sex = passport.sex;
        workerCard.birthdate = passport.birthdate;
        workerCard.birthPlace = passport.birthPlace;
        workerCard.expirationDate = GetRandomDate(currentDate, workerCardMaxExpirationDate);

        workerCard.agencySeal = agencyData.seal;
        workerCard.photo = passport.photo;

        workerCard.linkerPrefab = workerCardPrefab;
        docSpawnCanvas.transform.position = docSpawnPos;
        workerCard.parent = docSpawnCanvas.transform;

        return workerCard;
    }

    string GetRandomValidCargo(RaceData raceData)
    {
        Debug.Assert(cargoList != null && cargoList.Count != 0);
        return cargoList[Random.Range(0, cargoList.Count)];
    }

    DeliveryCard GetValidRandomDeliveryCard(RaceData raceData, Passport passport, AgencyData agencyData)
    {
        DeliveryCard deliveryCard = new DeliveryCard();
        deliveryCard.name = passport.name;
        deliveryCard.originPlanet = passport.planet;

        deliveryCard.agency = agencyData.name;
        deliveryCard.cargo = GetRandomValidCargo(raceData);
        deliveryCard.expirationDate = GetRandomDate(currentDate, workerCardMaxExpirationDate);
        //deliveryCard.signature = agencyData.sign;

        //deliveryCard.photo = passport.photo;

        deliveryCard.linkerPrefab = deliveryCardPrefab;
        docSpawnCanvas.transform.position = docSpawnPos;
        deliveryCard.parent = docSpawnCanvas.transform;

        return deliveryCard;
    }

    //DiplomaticLetter GetValidRandomDiplomaticLetter(RaceData raceData, Passport passport, AgencyData agencyData)
    //{
    //    DiplomaticLetter diplomaticLetter = new DiplomaticLetter();
    //    diplomaticLetter.name = passport.name;
    //    diplomaticLetter.seal = passport.planetSeal;
    //    diplomaticLetter.creationDate = GetRandomDate(passport.birthdate, currentDate);
    //    diplomaticLetter.creationCity = GetRandomPlace(raceData); // TODO : VERIFY VALIDITY

    //    diplomaticLetter.linkerPrefab = diplomaticLetterPrefab;
    //    docSpawnCanvas.transform.position = docSpawnPos;
    //    diplomaticLetter.parent = docSpawnCanvas.transform;

    //    return diplomaticLetter;
    //}

    WeaponLicense GetValidRandomWeaponLicense(RaceData raceData, Passport passport, AgencyData agencyData)
    {
        WeaponLicense weaponLicense = new WeaponLicense();
        weaponLicense.name = passport.name;
        weaponLicense.birthdate = passport.birthdate;
        weaponLicense.birthplace = passport.birthPlace;
        weaponLicense.sex = passport.sex;
        weaponLicense.expirationDate = GetRandomDate(currentDate, workerCardMaxExpirationDate);

        weaponLicense.photo = passport.photo;
        weaponLicense.originPlanet = passport.planet;

        weaponLicense.linkerPrefab = weaponLicensePrefab;
        docSpawnCanvas.transform.position = docSpawnPos;
        weaponLicense.parent = docSpawnCanvas.transform;

        return weaponLicense;
    }

    public T GetRandomEnum<T>()
    {
        System.Array values = System.Enum.GetValues(typeof(T));
        return (T) values.GetValue(Random.Range(0, values.Length));
    }

    // Get a random race, but uses weight based on how many characters there are in each race
    string GetRandomRace()
    {
        int total = 0;
        int[] weights = new int[allRacesData.Count];
        for (int i = 0; i < allRacesData.Count; i++)
        {
            weights[i] = allRacesData[i].photos.Count;
            total += weights[i];
        }
        int r = Random.Range(0, total);
        int j = 0;
        while (r > weights[j])
        {
            r -= weights[j];
            j++;
        }
        return allRacesData[j].race;
    }

    // Race relative
    void SetInvalidHeight(RaceData raceData, Passport passport)
    {
        passport.height = Random.Range(raceData.invalidDataParams.minHeight, raceData.invalidDataParams.maxHeight);
    }

    // Race relative
    void SetInvalidWeight(RaceData raceData, Passport passport)
    {
        passport.weight = Random.Range(raceData.invalidDataParams.minWeight, raceData.invalidDataParams.maxWeight);
    }

    // Race relative
    Date GetInvalidBirthdate(RaceData raceData)
    {
        return GetRandomDate(raceData.invalidDataParams.minBirthday, raceData.invalidDataParams.maxBirthday);
    }

    Date GetInvalidExpirationDate(RaceData raceData)
    {
        Date max = currentDate;
        max.RemoveDays(1);
        return GetRandomDate(raceData.invalidDataParams.minExpirationDate, max);
    }

    void SetInvalidPassport(RaceData raceData, Passport passport)
    {
        int randomParameter = Random.Range(0, 4);
        switch (randomParameter)
        {
            case 0:
                SetInvalidWeight(raceData, passport);
                break;
            case 1:
                SetInvalidHeight(raceData, passport);
                break;
            case 2:
                passport.birthdate = GetInvalidBirthdate(raceData);
                break;
            case 3:
                passport.expirationDate = GetInvalidExpirationDate(raceData);
                break;
        }
    }

    string GetRandomInvalidName(RaceData raceData, string currentName)
    {
        Debug.Assert(raceData.firstNames.Count > 1);
        string invalidName;
        do
        {
            invalidName = GetRandomName(raceData);
        } while (invalidName == currentName);
        return invalidName;
    }

    Planet GetRandomInvalidPlanet(RaceData raceData, Planet currentPlanet)
    {
        Debug.Assert(allRacesData.Count > 1);
        int index = Random.Range(1, allRacesData.Count);
        if (allRacesData[index].defaultPlanet == currentPlanet)
            index = 0;
        return allRacesData[index].defaultPlanet;
    }

    RaceData GetRandomInvalidRaceData(string currentRace)
    {
        Debug.Assert(allRacesData.Count > 1);
        int index = Random.Range(1, allRacesData.Count);
        if (allRacesData[index].race == currentRace)
            index = 0;
        return allRacesData[index];
    }

    string GetInvalidRandomSex(RaceData raceData, string currentSex)
    {
        Debug.Assert(raceData.possibleSexes.Count > 1);
        int index = Random.Range(1, raceData.possibleSexes.Count);
        if (raceData.possibleSexes[index] == currentSex)
            index = 0;
        return raceData.possibleSexes[index];
    }

    void SetInvalidWorkerCard(RaceData raceData, WorkerCard workerCard)
    {
        int max = 7;
        if (raceData.possibleSexes.Count == 1)
            max--;

        int randomParameter = Random.Range(0, max);
        switch (randomParameter)
        {
            case 0:
                workerCard.name = GetRandomInvalidName(raceData, workerCard.name);
                break;
            case 1:
                workerCard.planet = GetRandomInvalidPlanet(raceData, raceData.defaultPlanet);
                break;
            case 2:
                workerCard.birthdate = GetInvalidBirthdate(raceData);
                break;
            case 3:
                workerCard.expirationDate = GetInvalidExpirationDate(raceData);
                break;
            case 4:
                workerCard.birthPlace = GetRandomPlace(GetRandomInvalidRaceData(raceData.race));
                break;
            case 5:
                workerCard.photo = GetRandomPhoto(GetRandomInvalidRaceData(raceData.race));
                break;
            //case 6:
            //    workerCard.agencySeal = GetRandomAgency(GetRandomInvalidRaceData(raceData.race)).seal;
            //    break;
            case 6:
                workerCard.sex = GetInvalidRandomSex(raceData, workerCard.sex);
                break;
        }
    }
    

    void SetInvalidDeliveryCard(RaceData raceData, DeliveryCard deliveryCard)
    {
        int randomParameter = Random.Range(0, 2);
        switch (randomParameter)
        {
            case 0:
                deliveryCard.name = GetRandomInvalidName(raceData, deliveryCard.name);
                break;
            case 1:
                deliveryCard.originPlanet = GetRandomInvalidPlanet(raceData, raceData.defaultPlanet);
                break;
            //case 3:
            //    deliveryCard.photo = GetRandomPhoto(GetRandomInvalidRaceData(raceData.race));
            //    break;
            case 2:
                deliveryCard.agency = GetRandomAgency(GetRandomInvalidRaceData(raceData.race)).name;
                break;
            //case 4:
            //    deliveryCard.signature = GetRandomAgency(GetRandomInvalidRaceData(raceData.race)).sign;
            //    break;
            // TODO : Invalid cargo ?
        }
    }

    Date GetRandomInvalidCreationDate(RaceData raceData)
    {
        return GetRandomDate(raceData.invalidDataParams.minDocCreationDate, raceData.invalidDataParams.maxDocCreationDate);
    }

    void SetInvalidDiplomaticLetter(RaceData raceData, DiplomaticLetter diplomaticLetter)
    {
        int randomParameter = Random.Range(0, 4);
        switch (randomParameter)
        {
            case 0:
                diplomaticLetter.name = GetRandomInvalidName(raceData, diplomaticLetter.name);
                break;
            case 1:
                diplomaticLetter.seal = GetRandomInvalidRaceData(raceData.race).seal;
                break;
            case 2:
                diplomaticLetter.creationCity = GetRandomPlace(GetRandomInvalidRaceData(raceData.race));
                break;
            case 3:
                diplomaticLetter.creationDate = GetRandomInvalidCreationDate(GetRandomInvalidRaceData(raceData.race));
                break;
        }
    }

    void SetInvalidWeaponLicense(RaceData raceData, WeaponLicense weaponLicense)
    {
        int max = 5;
        if (raceData.possibleSexes.Count == 1)
            max--;

        int randomParameter = Random.Range(0, max);
        switch (randomParameter)
        {
            case 0:
                weaponLicense.name = GetRandomInvalidName(raceData, weaponLicense.name);
                break;
            case 1:
                weaponLicense.birthdate = GetInvalidBirthdate(raceData);
                break;
            //case 2:
            //    weaponLicense.birthplace = GetRandomPlace(GetRandomInvalidRaceData(raceData.race));
            //    break;
            case 2:
                weaponLicense.expirationDate = GetInvalidExpirationDate(raceData);
                break;
            case 3:
                weaponLicense.photo = GetRandomPhoto(GetRandomInvalidRaceData(raceData.race));
                break;
            case 4:
                weaponLicense.sex = GetInvalidRandomSex(raceData, weaponLicense.sex);
                break;

                // TODO : Invalid Weaopn Category ?
                //case 5:
                //    weaponLicense.category = GetInvalidExpirationDate(raceData);
                //    break;
        }
    }

    public void SetRandomData(Immigrant immigrant)
    {
        string race = GetRandomRace();
        RaceData raceData = GetRaceData(race);

        immigrant.race = race;

        Passport passport = GetValidRandomPassport(raceData);

        System.Predicate<CharacterPhoto> predicate = (CharacterPhoto p) => p.photo == passport.photo;
        Sprite real = characterAndPhoto.Find(predicate).real;
        if (real == null)
            Debug.LogError("invalid photos or sprites set");
        immigrant.SetSprite(real);
        immigrant.SetPhoto(passport.photo);
        immigrant.SetRace(passport.race);

        AgencyData agencyData = GetRandomAgency(raceData);

        if (Random.Range(0, 100) < errorPercent)
        {
            SetInvalidPassport(raceData, passport);
            immigrant.isSpy = true;
        }

        immigrant.documents.Add(passport);

        if (Random.Range(0, 100) < workerCardPercent)
        {
            WorkerCard workerCard = GetValidRandomWorkerCard(raceData, passport, agencyData);

            if (Random.Range(0, 100) < errorPercent)
            {
                SetInvalidWorkerCard(raceData, workerCard);
                immigrant.isSpy = true;
            }

            immigrant.documents.Add(workerCard);
        }

        if (Random.Range(0, 100) < deliveryCardPercent)
        {
            DeliveryCard deliveryCard = GetValidRandomDeliveryCard(raceData, passport, agencyData);

            if (Random.Range(0, 100) < errorPercent)
            {
                SetInvalidDeliveryCard(raceData, deliveryCard);
                immigrant.isSpy = true;
            }

            immigrant.documents.Add(deliveryCard);
        }

        //if (Random.Range(0, 100) < diplomaticLetterPercent)
        //{
        //    DiplomaticLetter diplomaticLetter = GetValidRandomDiplomaticLetter(raceData, passport, agencyData);

        //    if (Random.Range(0, 100) < errorPercent)
        //    {
        //        SetInvalidDiplomaticLetter(raceData, diplomaticLetter);
        //        immigrant.isSpy = true;
        //    }

        //    immigrant.documents.Add(diplomaticLetter);
        //}

        if (Random.Range(0, 100) < weaponLicensePercent)
        {
            WeaponLicense weaponLicense = GetValidRandomWeaponLicense(raceData, passport, agencyData);

            if (Random.Range(0, 100) < errorPercent)
            {
                SetInvalidWeaponLicense(raceData, weaponLicense);
                immigrant.isSpy = true;
            }

            immigrant.documents.Add(weaponLicense);
        }
    }
}
