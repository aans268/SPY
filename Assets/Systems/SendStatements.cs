﻿using UnityEngine;
using FYFY;
using System.Collections;
using Newtonsoft.Json;
using UnityEngine.Networking;

public class SendStatements : FSystem {

    private Family f_actionForLRS = FamilyManager.getFamily(new AllOfComponents(typeof(ActionPerformedForLRS)));
    private Family f_saveProgression = FamilyManager.getFamily(new AllOfComponents(typeof(SendUserData)));

    public static SendStatements instance;

    private GameData gameData;
    private UserData userData;

    public SendStatements()
    {
        instance = this;
    }
	
	protected override void onStart()
    {
        GameObject gd = GameObject.Find("GameData");
        if (gd != null)
        {
            gameData = gd.GetComponent<GameData>();
            userData = gd.GetComponent<UserData>();
        }

        f_saveProgression.addEntryCallback(saveUserData);
    }

    // Use to process your families.
    protected override void onProcess(int familiesUpdateCount) {
        if (gameData.sendStatementEnabled)
        {
            // Do not use callbacks because in case in the same frame actions are removed on a GO and another component is added in another system, family will not trigger again callback because component will not be processed
            foreach (GameObject go in f_actionForLRS)
            {
                ActionPerformedForLRS[] listAP = go.GetComponents<ActionPerformedForLRS>();
                int nb = listAP.Length;
                ActionPerformedForLRS ap;
                if (!this.Pause)
                {
                    for (int i = 0; i < nb; i++)
                    {
                        ap = listAP[i];
                        //If no result info filled
                        if (!ap.result)
                        {
                            GBL_Interface.SendStatement(ap.verb, ap.objectType, ap.activityExtensions);
                        }
                        else
                        {
                            bool? completed = null, success = null;

                            if (ap.completed > 0)
                                completed = true;
                            else if (ap.completed < 0)
                                completed = false;

                            if (ap.success > 0)
                                success = true;
                            else if (ap.success < 0)
                                success = false;

                            GBL_Interface.SendStatementWithResult(ap.verb, ap.objectType, ap.activityExtensions, ap.resultExtensions, completed, success, ap.response, ap.score, ap.duration);
                        }
                    }
                }
                for (int i = nb - 1; i > -1; i--)
                {
                    GameObjectManager.removeComponent(listAP[i]);
                }
            }
        }
    }

    private void saveUserData (GameObject go)
    {
        if (gameData.sendStatementEnabled)
        {
            MainLoop.instance.StartCoroutine(PostUserData());
            foreach (SendUserData sp in go.GetComponents<SendUserData>())
                GameObjectManager.removeComponent(sp);
        }
    }

    private IEnumerator PostUserData()
    {
        string progession = JsonConvert.SerializeObject(userData.progression);
        string highScore = JsonConvert.SerializeObject(userData.highScore);
        Debug.Log(progession + "_" + highScore);
        UnityWebRequest www = UnityWebRequest.Post("https://spy.lip6.fr/ServerREST_LIP6/", "{\"idSession\":\"" + GBL_Interface.playerName + "\",\"class\":\""+userData.schoolClass+"\",\"isTeacher\":\""+(userData.isTeacher ? 1 : 0)+"\",\"progression\":" + progession + ",\"highScore\":" + highScore + "}");

        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
            Debug.LogWarning(www.error);
    }
}