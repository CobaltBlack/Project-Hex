using UnityEngine;
using System.Collections;

public class Character
{
    public enum CharacterClass
    {
        Bard,
        Hunter,
        Captain,
    }

    public string characterName;

    public int currentHp;
    public int maxHp;
    public int actionPoints;

    public int morality;
    public int sanity;
    public int moralityFlux;
    public int sanityFlux;

    public int attack;
    public int crit;
    public int defense;
    public int dodge;

    public Character(CharacterClass characterClass)
    {
        if (characterClass == CharacterClass.Bard)
        {
            characterName = "Bard";

            currentHp = 60;
            maxHp = 60;
            actionPoints = 120;

            morality = 50;
            sanity = 30;
            moralityFlux = 0;
            sanityFlux = 0;

            attack = 10;
            crit = 20;
            defense = 0;
            dodge = 20;
        }

        if (characterClass == CharacterClass.Hunter)
        {
            characterName = "Hunter";

            currentHp = 90;
            maxHp = 90;
            actionPoints = 90;

            morality = 70;
            sanity = 50;
            moralityFlux = 0;
            sanityFlux = 0;

            attack = 15;
            crit = 20;
            defense = 10;
            dodge = 20;
        }

        if (characterClass == CharacterClass.Captain)
        {
            characterName = "Captain";

            currentHp = 110;
            maxHp = 110;
            actionPoints = 100;

            morality = 80;
            sanity = 80;
            moralityFlux = 0;
            sanityFlux = 0;

            attack = 25;
            crit = 0;
            defense = 25;
            dodge = 0;
        }
    }
}