using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Freya;

public class Testbed : MonoBehaviour
{
	[SerializeField]
	private SpriteRenderer sprite;

	[SerializeField]
	private Game.Controller controller;
	[SerializeField]
	private Game.Bucket dirtiness;
	[SerializeField]
	private Game.Bucket energy;

	[SerializeField]
	private Game.Bucket health;
	[SerializeField]
	private IntEvent deathEvent;

	private void Start()
	{
		controller.onDeviceChange += input => Debug.Log(input);
		controller.onAttack += OnAttack;
		controller.onClean += OnClean;
		controller.onDodge += OnDodge;
		controller.Activate();

		deathEvent.onEvent += (_) => Debug.Log("Dead");
	}

	private void OnAttack(Game.ButtonState state)
	{
		if(state == Game.ButtonState.Pressed) {
			dirtiness.Current += 5;
		}
	}

	private void OnClean(Game.ButtonState state)
	{
		if(state == Game.ButtonState.Pressed) {
			dirtiness.Current += -5; 
		}
	}

	private void OnDodge(Game.ButtonState state)
	{
		if(state == Game.ButtonState.Pressed) {
			energy.Current += 1;
		}
	}

	void Update()
    {
		// Move on XZ plane 
		transform.position += controller.Move.X0Y() * Time.deltaTime * 50.0f;
	}

	private void OnGUI()
	{
		// Set style
		GUIStyle layout = new GUIStyle(GUI.skin.box);
		layout.alignment = TextAnchor.MiddleLeft;

		// Set width
		var width = GUILayout.Width(180.0f); // carefully researched value ;) 

		// Remember colour just in case we recolour
		var color = GUI.color;

		if(GUILayout.Button("Block Input\n for 5 Seconds", width)) {
			controller.Block(this, 5.0f);
		}

		GUILayout.Label($"Move\t - {controller.Move}", layout, width);
		GUILayout.Label($"Look\t - {controller.Look}", layout, width);
		GUILayout.Label($"Target\t - {controller.Target}", layout, width);

		DrawColouredDebugLabel("Attack\t", controller.Attack, layout, width);
		DrawColouredDebugLabel("Dodge\t", controller.Dodge, layout, width);
		DrawColouredDebugLabel("Clean\t", controller.Clean, layout, width);
		DrawColouredDebugLabel("Heal\t", controller.Heal, layout, width);

		// Reset colour - Just in case
		GUI.color = color;
	}

	private void DrawColouredDebugLabel(string title, Game.ButtonState button, GUIStyle layout, params GUILayoutOption[] options)
	{
		GUI.color = button == Game.ButtonState.Pressed ? Color.green : Color.red;
		GUILayout.Label($"{title} - {button}", layout, options);
	}
}
