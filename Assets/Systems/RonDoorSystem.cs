using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FYFY;


public class RonDoorSystem : FSystem
{
	private Family f_ronDoor = FamilyManager.getFamily(new AllOfComponents(typeof(RonDoorSlot1),typeof(RonDoorSlot2),typeof(RonDoorSlot3), typeof(Position)), new AnyOfTags("RonDoor"), new AnyOfProperties(PropertyMatcher.PROPERTY.ACTIVE_IN_HIERARCHY));
    private Family f_gameLoaded = FamilyManager.getFamily(new AllOfComponents(typeof(GameLoaded)));
    private GameData gameData;
	public GameObject LevelGO;
    public Dictionary<GameObject, string> ronDoor_equations = new Dictionary<GameObject, string>();


    protected override void onStart()
    {
        GameObject go = GameObject.Find("GameData");
        if (go != null)
            gameData = go.GetComponent<GameData>();
            createRonDoorEquation();
    }

    protected override void onProcess(int familiesUpdateCount)
    {
        foreach (GameObject ronDoor in f_ronDoor)
        {
            this_equation= ronDoor_equations[ronDoor];
            //on récupère le string : "2 * RON + 3<12"

            this_equation = this_equation.Replace("RON", gameData.totalRon.ToString());


        }
    }

    private void createRonDoorEquation()
    {
        foreach (GameObject ronDoor in f_ronDoor)
        {
            //dictionnaire de toutes les rondoor et leurs équations dico[rondoor]= slot1 + slot2+ slot3
            string this_slot1 = ronDoor.GetComponent<RonDoorSlot1>().equation;
            string this_slot2 = ronDoor.GetComponent<RonDoorSlot2>().operator_sign;
            int this_slot3 = ronDoor.GetComponent<RonDoorSlot3>().result;
            string this_full_equation= this_slot1+this_slot2+this_slot3.ToString();
            ronDoor_equations.Add(ronDoor,this_full_equation);
            Debug.Log("AKKKKKKKKKKKKKKKKKKKKKKKK     "+ronDoor_equations);
        }
    }
}
