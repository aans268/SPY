using UnityEngine;
using FYFY;
using System.Collections;
using FYFY_plugins.TriggerManager;
using UnityEngine.Localization.SmartFormat.Utilities;
using TMPro;



/// <summary>
/// Manage collision between player agents and Coins
/// </summary>
public class RonManager : FSystem
{
    private Family f_robotcollision = FamilyManager.getFamily(new AllOfComponents(typeof(Triggered3D)), new AnyOfTags("Player"));

	private Family f_playingMode = FamilyManager.getFamily(new AllOfComponents(typeof(PlayMode)));
	private Family f_editingMode = FamilyManager.getFamily(new AllOfComponents(typeof(EditMode)));

	private GameData gameData;
    private bool activeRon;

	public TMP_Text ronText;

	protected override void onStart()
    {
		activeRon = false;
		GameObject go = GameObject.Find("GameData");
		if (go != null)
			gameData = go.GetComponent<GameData>();
		f_robotcollision.addEntryCallback(onNewCollision);

		f_playingMode.addEntryCallback(delegate { activeRon = true; });
		f_editingMode.addEntryCallback(delegate { activeRon = false; });
	}

	private void onNewCollision(GameObject robot){
		if(activeRon){
			Triggered3D trigger = robot.GetComponent<Triggered3D>();
			foreach(GameObject target in trigger.Targets){
				//Check if the player collide with a coin
                if(target.CompareTag("1Ron")){
					gameData.totalRon += target.GetComponent<RonValue>().value;
					Debug.Log("gamedata ron : "+gameData.totalRon);
					ronText.text = "Valeur totale des Rons collectés : " + gameData.totalRon.ToString();
					//target.GetComponent<AudioSource>().Play();
					//Debug.Log("gamedata ron : "+gameData.totalRon);
                    //target.GetComponent<AudioSource>().Play();
					target.GetComponent<Collider>().enabled = false;
                    MainLoop.instance.StartCoroutine(ronDestroy(target));					
				}
			}			
		}
    }

	private IEnumerator ronDestroy(GameObject go){
		go.GetComponent<ParticleSystem>().Play();
		go.GetComponent<Renderer>().enabled = false;
		yield return new WaitForSeconds(1f); // let time for animation
		GameObjectManager.setGameObjectState(go, false); // then disabling GameObject
	}
}
