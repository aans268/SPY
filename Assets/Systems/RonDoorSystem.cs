using System.Collections.Generic;
using UnityEngine;
using FYFY;
using System;
using System.Data;
using UnityEditor.Localization.Plugins.XLIFF.V12;


public class RonDoorSystem : FSystem
{
    private Family f_gameLoaded = FamilyManager.getFamily(new AllOfComponents(typeof(GameLoaded)));
    private readonly Family f_ronDoor = FamilyManager.getFamily(new AllOfComponents(typeof(RonDoorSlot1),typeof(RonDoorSlot2),typeof(RonDoorSlot3), typeof(Position)), new AnyOfTags("RonDoor"));
    public GameObject LevelGO;
    public Dictionary<GameObject, string> ronDoor_equations = new();
    private GameData gameData;

    private bool createRonDoorEquation = true;
    private int previousTotalRon;

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

        if (HasTotalRonChanged())
        {
            UpdateRonDoors();
        }
    }

    private bool HasTotalRonChanged()
    {
        if (gameData.totalRon != previousTotalRon)
        {
            previousTotalRon = gameData.totalRon;
            return true;
        }
        return false;
    }

    private void UpdateRonDoors()
    {
        foreach (GameObject ronDoor in f_ronDoor)
        {
            if (ronDoor_equations.ContainsKey(ronDoor))
            {
                string this_equation = ronDoor_equations[ronDoor];
                this_equation = this_equation.Replace("RON", gameData.totalRon.ToString(), StringComparison.OrdinalIgnoreCase);

                try
                {
                    bool result = EvaluateExpression(this_equation);
                    HandleRonDoor(ronDoor, result);
                    Debug.Log($"L'expression '{this_equation}' est {result}");
                }
                catch (Exception ex)
                {
                    Debug.Log($"Erreur lors de l'évaluation : {ex.Message}");
                }
            }
        }
    }

    private void HandleRonDoor(GameObject ronDoor, bool result)
    {
        var animator = ronDoor.transform.parent.GetComponent<Animator>();
        var audioSource = ronDoor.transform.parent.GetComponent<AudioSource>();

        if (result && !animator.GetCurrentAnimatorStateInfo(0).IsName("DoorOpen"))
        {
            audioSource.Play();
            animator.SetTrigger("Open");
        }
        else if (!result && !animator.GetCurrentAnimatorStateInfo(0).IsName("DoorIdle"))
        {
            audioSource.Play();
            animator.SetTrigger("Close");
        }
        ronDoor.transform.parent.transform.Find("TeleporterCentral").GetComponent<TooltipContent>().text = result ? "Porte ouverte" : "Porte fermée";

        animator.speed = gameData.gameSpeed_current;
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
