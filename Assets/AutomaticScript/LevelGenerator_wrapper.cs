using UnityEngine;
using FYFY;

public class LevelGenerator_wrapper : BaseWrapper
{
	public UnityEngine.GameObject LevelGO;
	public UnityEngine.GameObject editableCanvas;
	public UnityEngine.GameObject scriptContainer;
	public UnityEngine.GameObject library;
	public TMPro.TMP_Text levelName;
	public UnityEngine.GameObject buttonExecute;
	public UnityEngine.Material ronMaterial1;
	public UnityEngine.Material ronMaterial2;
	public UnityEngine.Material ronMaterial3;
	private void Start()
	{
		this.hideFlags = HideFlags.NotEditable;
		MainLoop.initAppropriateSystemField (system, "LevelGO", LevelGO);
		MainLoop.initAppropriateSystemField (system, "editableCanvas", editableCanvas);
		MainLoop.initAppropriateSystemField (system, "scriptContainer", scriptContainer);
		MainLoop.initAppropriateSystemField (system, "library", library);
		MainLoop.initAppropriateSystemField (system, "levelName", levelName);
		MainLoop.initAppropriateSystemField (system, "buttonExecute", buttonExecute);
		MainLoop.initAppropriateSystemField (system, "ronMaterial1", ronMaterial1);
		MainLoop.initAppropriateSystemField (system, "ronMaterial2", ronMaterial2);
		MainLoop.initAppropriateSystemField (system, "ronMaterial3", ronMaterial3);
	}

}
