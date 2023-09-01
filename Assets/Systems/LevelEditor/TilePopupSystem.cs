using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using FYFY;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using FYFY_plugins.PointerManager;
using TMPro;

public class TilePopupSystem : FSystem
{
	private Family f_popups = FamilyManager.getFamily(new AllOfComponents(typeof(Popup)));
	private Family f_activePopups = FamilyManager.getFamily(new AllOfComponents(typeof(Popup)), new AllOfProperties(PropertyMatcher.PROPERTY.ACTIVE_IN_HIERARCHY));
	private Family f_focusedPopups = FamilyManager.getFamily(new AllOfComponents(typeof(Popup), typeof(PointerOver)));

	public static TilePopupSystem instance;
	public GameObject orientationPopup;
	public GameObject inputLinePopup;
	public GameObject rangePopup;
	public GameObject consoleSlotsPopup;
	public GameObject doorSlotPopup;
	public GameObject furniturePopup;

	public PaintableGrid paintableGrid;
	public LevelData levelData;

	private const string FurniturePrefix = "Prefabs/Modern Furniture/Prefabs/";
	private const string PathXmlPrefix = "Modern Furniture/Prefabs/";

	private List<string> furnitureNameToPath = new List<string>();

	public TilePopupSystem()
	{
		instance = this;
	}

	// Use to init system before the first onProcess call
	protected override void onStart()
	{
		hideAllPopups();
		initFurniturePopup();
		paintableGrid.selectedObject = null;
	}

	private void initFurniturePopup()
	{
		// Change the path provided here to load more options
		var prefabs = Resources.LoadAll<GameObject>(FurniturePrefix).ToList();
		var prefabNames = prefabs.GroupBy(p => p.name).Select(g => g.First().name).ToList();
		TMP_Dropdown dropdown = furniturePopup.GetComponentInChildren<TMP_Dropdown>();

		List<string> options = new List<string>();
		foreach (string name in prefabNames)
		{
			furnitureNameToPath.Add(PathXmlPrefix + name);
			options.Add(name);
		}
		furniturePopup.GetComponentInChildren<TMP_Dropdown>().AddOptions(options);
	}

	// Use to process your families.
	protected override void onProcess(int familiesUpdateCount)
	{
		Vector2Int pos = UtilityEditor.mousePosToGridPos(paintableGrid.GetComponent<Tilemap>());
		Tuple<int, int> posTuple = new Tuple<int, int>(pos.y, pos.x);

		if (Input.GetMouseButtonDown(1) && paintableGrid.selectedObject != null && paintableGrid.selectedObject.line == pos.y && paintableGrid.selectedObject.col == pos.x)
		{
			paintableGrid.selectedObject = null;
		}

		if (Input.GetMouseButtonDown(0) && paintableGrid.floorObjects.ContainsKey(posTuple) && paintableGrid.floorObjects[posTuple].selectable)
		{
			paintableGrid.selectedObject = paintableGrid.floorObjects[posTuple];
		}

		if (f_activePopups.Count > 0 && (Input.GetKeyDown(KeyCode.Escape) || paintableGrid.selectedObject == null || (!paintableGrid.floorObjects.ContainsKey(posTuple) && Input.GetMouseButtonDown(0) && f_focusedPopups.Count == 0)))
		{
			hideAllPopups();
			paintableGrid.selectedObject = null; // be sure
		}

		if (Input.GetMouseButtonDown(0) && paintableGrid.selectedObject != null && f_focusedPopups.Count == 0)
		{
			hideAllPopups();
			switch (paintableGrid.selectedObject)
			{
				case Door d:
					// enable popups
					GameObjectManager.setGameObjectState(orientationPopup, true);
					GameObjectManager.setGameObjectState(doorSlotPopup, true);
					// load data
					doorSlotPopup.GetComponentInChildren<TMP_InputField>().text = d.slot;
					break;
				case Console c:
					// enable popups
					GameObjectManager.setGameObjectState(orientationPopup, true);
					GameObjectManager.setGameObjectState(consoleSlotsPopup, true);
					// load data
					consoleSlotsPopup.GetComponentInChildren<TMP_InputField>().text = string.Join(", ", c.slots);
					consoleSlotsPopup.GetComponentInChildren<Toggle>().isOn = c.state;
					break;
				case PlayerRobot pr:
					// enable popups
					GameObjectManager.setGameObjectState(orientationPopup, true);
					GameObjectManager.setGameObjectState(inputLinePopup, true);
					// load data
					inputLinePopup.GetComponentInChildren<TMP_InputField>().text = pr.inputLine;
					break;
				case EnemyRobot er:
					// enable popups
					GameObjectManager.setGameObjectState(orientationPopup, true);
					GameObjectManager.setGameObjectState(inputLinePopup, true);
					GameObjectManager.setGameObjectState(rangePopup, true);
					// load data
					inputLinePopup.GetComponentInChildren<TMP_InputField>().text = er.inputLine;
					rangePopup.GetComponentInChildren<TMP_InputField>().text = er.range.ToString();
					rangePopup.GetComponentInChildren<Toggle>().isOn = !er.selfRange;
					rangePopup.GetComponentInChildren<TMP_Dropdown>().value = (int)er.typeRange;
					break;
				case DecorationObject deco:
					// enable popups
					GameObjectManager.setGameObjectState(orientationPopup, true);
					GameObjectManager.setGameObjectState(furniturePopup, true);
					// load data
					int i = 0;
					foreach (string value in furnitureNameToPath)
					{
						if (value == ((DecorationObject)paintableGrid.selectedObject).path)
							break;
						i++;
					}
					furniturePopup.GetComponentInChildren<TMP_Dropdown>().value = i;
					break;
			}
		}
	}

	private void hideAllPopups()
	{
		foreach (GameObject popup in f_popups)
			GameObjectManager.setGameObjectState(popup, false);
	}

	public void rotateObject(int newOrientation)
	{
		var newpos = coordsToGridCoords(paintableGrid.selectedObject.col, paintableGrid.selectedObject.line);
		var quat = Quaternion.Euler(0, 0, orientationToInt((ObjectDirection)newOrientation));

		paintableGrid.GetComponent<Tilemap>().SetTransformMatrix(newpos, Matrix4x4.Rotate(quat));
		paintableGrid.selectedObject.orientation = (ObjectDirection)newOrientation;
	}

	private Vector3Int coordsToGridCoords(int col, int line)
	{
		return new Vector3Int(col - UtilityEditor.gridMaxSize / 2,
			UtilityEditor.gridMaxSize / 2 - line, -1);
	}

	private int orientationToInt(ObjectDirection orientation)
	{
		return orientation switch
		{
			ObjectDirection.Up => 0,
			ObjectDirection.Right => 270,
			ObjectDirection.Down => 180,
			ObjectDirection.Left => 90,
			_ => throw new ArgumentOutOfRangeException(nameof(orientation), orientation, "Impossible orientation")
		};
	}

	// see InputLinePopup GameObject
	public void popUpInputLine(string newData)
	{
		if (paintableGrid.selectedObject != null)
			((Robot)paintableGrid.selectedObject).inputLine = newData;
	}

	// see rangePopup GameObject
	public void popupRangeInputField(string newData)
	{
		if (paintableGrid.selectedObject != null)
			((EnemyRobot)paintableGrid.selectedObject).range = int.TryParse(newData, out int x) ? x : 0;
	}

	// see rangePopup GameObject
	public void popupRangeToggle(bool newData)
	{
		if (paintableGrid.selectedObject != null)
			((EnemyRobot)paintableGrid.selectedObject).selfRange = !newData;
	}

	// see rangePopup GameObject
	public void popupRangeDropDown(int newData)
	{
		if (paintableGrid.selectedObject != null)
			((EnemyRobot)paintableGrid.selectedObject).typeRange = (EnemyTypeRange)newData;
	}

	// see consoleSlotsPopup GameObject
	public void popupConsoleSlots(string newData)
	{
		if (paintableGrid.selectedObject != null)
		{
			string trimmed = String.Concat(newData.Where(c => !Char.IsWhiteSpace(c)));
			int[] ints = Array.ConvertAll(trimmed.Split(','), s => int.TryParse(s, out int x) ? x : -1);
			((Console)paintableGrid.selectedObject).slots = trimmed.Split(',');
		}
	}

	// see consoleSlotsPopup GameObject
	public void popupConsoleToggle(bool newData)
	{
		if (paintableGrid.selectedObject != null)
			((Console)paintableGrid.selectedObject).state = newData;
	}

	// see doorSlotPopup GameObject
	public void popupDoorSlot(string newData)
	{
		if (paintableGrid.selectedObject != null)
			((Door)paintableGrid.selectedObject).slot = newData;
	}

	// see furniturePopup GameObject
	public void popupFurnitureDropDown(int newData)
	{
		if (paintableGrid.selectedObject != null)
			((DecorationObject)paintableGrid.selectedObject).path = furnitureNameToPath[newData];
	}
}