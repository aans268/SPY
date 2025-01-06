using UnityEngine;
using FYFY;

public class RonManager_wrapper : BaseWrapper
{
	public TMPro.TMP_Text ronText;
	private void Start()
	{
		this.hideFlags = HideFlags.NotEditable;
		MainLoop.initAppropriateSystemField (system, "ronText", ronText);
	}

}
