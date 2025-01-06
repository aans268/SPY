using UnityEngine;
using FYFY;

public class TilePopupSystem_wrapper : BaseWrapper
{
	public UnityEngine.GameObject orientationPopup;
	public UnityEngine.GameObject inputLinePopup;
	public UnityEngine.GameObject rangePopup;
	public UnityEngine.GameObject consoleSlotsPopup;
	public UnityEngine.GameObject doorSlotPopup;
	public UnityEngine.GameObject ronDoorSlot1Popup;
	public UnityEngine.GameObject ronDoorSlot2Popup;
	public UnityEngine.GameObject ronDoorSlot3Popup;
	public UnityEngine.GameObject ronValuePopup;
	public UnityEngine.GameObject furniturePopup;
	public PaintableGrid paintableGrid;
	public UnityEngine.GameObject selection;
	private void Start()
	{
		this.hideFlags = HideFlags.NotEditable;
		MainLoop.initAppropriateSystemField (system, "orientationPopup", orientationPopup);
		MainLoop.initAppropriateSystemField (system, "inputLinePopup", inputLinePopup);
		MainLoop.initAppropriateSystemField (system, "rangePopup", rangePopup);
		MainLoop.initAppropriateSystemField (system, "consoleSlotsPopup", consoleSlotsPopup);
		MainLoop.initAppropriateSystemField (system, "doorSlotPopup", doorSlotPopup);
		MainLoop.initAppropriateSystemField (system, "ronDoorSlot1Popup", ronDoorSlot1Popup);
		MainLoop.initAppropriateSystemField (system, "ronDoorSlot2Popup", ronDoorSlot2Popup);
		MainLoop.initAppropriateSystemField (system, "ronDoorSlot3Popup", ronDoorSlot3Popup);
		MainLoop.initAppropriateSystemField (system, "ronValuePopup", ronValuePopup);
		MainLoop.initAppropriateSystemField (system, "furniturePopup", furniturePopup);
		MainLoop.initAppropriateSystemField (system, "paintableGrid", paintableGrid);
		MainLoop.initAppropriateSystemField (system, "selection", selection);
	}

	public void rotateObject(System.Int32 newOrientation)
	{
		MainLoop.callAppropriateSystemMethod (system, "rotateObject", newOrientation);
	}

	public void popUpInputLine(System.String newData)
	{
		MainLoop.callAppropriateSystemMethod (system, "popUpInputLine", newData);
	}

	public void popupRangeInputField(System.String newData)
	{
		MainLoop.callAppropriateSystemMethod (system, "popupRangeInputField", newData);
	}

	public void popupRangeToggle(System.Boolean newData)
	{
		MainLoop.callAppropriateSystemMethod (system, "popupRangeToggle", newData);
	}

	public void popupRangeDropDown(System.Int32 newData)
	{
		MainLoop.callAppropriateSystemMethod (system, "popupRangeDropDown", newData);
	}

	public void popupConsoleSlots(System.String newData)
	{
		MainLoop.callAppropriateSystemMethod (system, "popupConsoleSlots", newData);
	}

	public void popupConsoleToggle(System.Boolean newData)
	{
		MainLoop.callAppropriateSystemMethod (system, "popupConsoleToggle", newData);
	}

	public void popupRonDoorSlot1(System.String newData)
	{
		MainLoop.callAppropriateSystemMethod (system, "popupRonDoorSlot1", newData);
	}

	public void popupRonDoorSlot2(System.Int32 newData)
	{
		MainLoop.callAppropriateSystemMethod (system, "popupRonDoorSlot2", newData);
	}

	public void popupRonDoorSlot3(System.String newData)
	{
		MainLoop.callAppropriateSystemMethod (system, "popupRonDoorSlot3", newData);
	}

	public void popupRonValue(System.Int32 newData)
	{
		MainLoop.callAppropriateSystemMethod (system, "popupRonValue", newData);
	}

	public void popupFurnitureDropDown(System.Int32 newData)
	{
		MainLoop.callAppropriateSystemMethod (system, "popupFurnitureDropDown", newData);
	}

}
