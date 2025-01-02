using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FYFY;
using System;
using System.Data;


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
            // Vérifier si l'objet 'ronDoor' existe déjà dans le dictionnaire
            if (ronDoor_equations.ContainsKey(ronDoor))
            {
                string this_equation = ronDoor_equations[ronDoor];
                //on récupère le string : "2 * RON + 3<12"

                this_equation = this_equation.Replace("RON", gameData.totalRon.ToString());
                try
                {
                    // Évaluer l'expression
                    bool result = EvaluateExpression(this_equation);

                    Debug.Log($"L'expression '{this_equation}' est {result}");
                }
                catch (Exception ex)
                {
                    Debug.Log($"Erreur lors de l'évaluation : {ex.Message}");
                }
            }
            else
            {
                Debug.LogWarning($"La clé pour le GameObject '{ronDoor.name}' n'a pas été trouvée dans le dictionnaire.");
            }
        }
    }


     static bool EvaluateExpression(string expression)
    {
        // Utiliser DataTable pour évaluer l'expression
        DataTable table = new DataTable();
        object result = table.Compute(expression, "");

        // Convertir le résultat en booléen
        return Convert.ToBoolean(result);
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
            //Debug.Log("AKKKKKKKKKKKKKKKKKKKKKKKK     "+ronDoor_equations);
        }
    }
}
