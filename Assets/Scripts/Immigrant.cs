using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Document
{ 

};

public class Passport : Document
{
    public string name;
    public Planet planet;
    public Race race;
    public int weight;
    public int height;
    public string sex;
    public Date birthdate;
    public Date expirationDate;
    public Sprite planetIcon;
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


}

public enum ImmigrantType
{ 
    Transit,
    Worker, 
    Delivery, 
    Diplomatic,
    Immigrant
}
