using System.Collections.Generic;
using UnityEngine;
using FYFY;
using System;
using System.Data;


public class RonDoorSystem : FSystem
{
    private Family f_gameLoaded = FamilyManager.getFamily(new AllOfComponents(typeof(GameLoaded)));
    private readonly Family f_ronDoor = FamilyManager.getFamily(new AllOfComponents(typeof(RonDoorSlot1),typeof(RonDoorSlot2),typeof(RonDoorSlot3), typeof(Position)), new AnyOfTags("RonDoor"));
    public GameObject LevelGO;
    public Dictionary<GameObject, string> ronDoor_equations = new();
    private GameData gameData;

    private bool createRonDoorEquation = true;

    protected override void onStart()
    {
        GameObject go = GameObject.Find("GameData");
        if (go != null)
            
            gameData = go.GetComponent<GameData>();
            //CreateRonDoorEquation();
    }

    protected override void onProcess(int familiesUpdateCount)
    {

        if (createRonDoorEquation)
        {
            CreateRonDoorEquation();
            createRonDoorEquation = false;
        }
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

                    if (result)
                    {
                        // display door
                        ronDoor.transform.parent.GetComponent<AudioSource>().Play();
                        ronDoor.transform.parent.GetComponent<Animator>().SetTrigger("Open");
                        ronDoor.transform.parent.GetComponent<Animator>().speed = gameData.gameSpeed_current;
                    }
                    else
                    {
                        // display door
                        ronDoor.transform.parent.GetComponent<AudioSource>().Play();
                        ronDoor.transform.parent.GetComponent<Animator>().SetTrigger("Close");
                        ronDoor.transform.parent.GetComponent<Animator>().speed = gameData.gameSpeed_current;
                    }

                    Debug.Log($"L'expression '{this_equation}' est {result}");
                }
                catch (Exception ex)
                {
                    Debug.Log($"Erreur lors de l'évaluation : {ex.Message}");
                }
                
            }
            else
            {
                //Debug.LogWarning($"La clé pour le GameObject '{ronDoor.name}' n'a pas été trouvée dans le dictionnaire.");
            }
        }
    }


     static bool EvaluateExpression(string expression)
    {
        expression = expression.Replace("!=", "<>");
        // Utiliser DataTable pour évaluer l'expression
        DataTable table = new();

        // Évaluer l'expression et convertir le résultat en booléen
        var result = table.Compute(expression, string.Empty);
        return Convert.ToBoolean(result);
    }

    private void CreateRonDoorEquation()
    {
        Debug.Log(f_ronDoor);
        foreach (GameObject ronDoor in f_ronDoor)
        {
            //dictionnaire de toutes les rondoor et leurs équations dico[rondoor]= slot1 + slot2+ slot3
            string this_slot1 = ronDoor.GetComponent<RonDoorSlot1>().equation;
            string this_slot2 = ronDoor.GetComponent<RonDoorSlot2>().operator_sign;
            int this_slot3 = ronDoor.GetComponent<RonDoorSlot3>().result;
            string this_full_equation= this_slot1+this_slot2+this_slot3.ToString();
            ronDoor_equations.Add(ronDoor,this_full_equation);
            Debug.Log(ronDoor_equations);
        }
    }
}
