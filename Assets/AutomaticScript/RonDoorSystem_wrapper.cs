using UnityEngine;
using FYFY;

public class RonDoorSystem_wrapper : BaseWrapper
{
	public UnityEngine.GameObject LevelGO;
	private void Start()
	{
		this.hideFlags = HideFlags.NotEditable;
		MainLoop.initAppropriateSystemField (system, "LevelGO", LevelGO);
	}

}
