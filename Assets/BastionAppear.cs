using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Not so bastion anymore :( - Team's choice, not mine
public class BastionAppear : MonoBehaviour
{
	MaterialPropertyBlock props;

	float value = 0;
	[SerializeField]
	float appearSpeed = 10f;
	[SerializeField]
	float disappearSpeed = 5f;
	[SerializeField]
	float radius = 12f;
	[SerializeField]
	bool keep = false;
	
	private MeshRenderer renderer;
	[SerializeField]
	float shderValue;
	int shaderID;

	[SerializeField]
	private RuntimeGameObjectReference cameraGO;

	[SerializeField]
	private Vector3Event onPlayerMove;

	[SerializeField]
	private bool disappearsOnClose = true;

	void Start()
	{
		renderer = GetComponent<MeshRenderer>();
		props = new MaterialPropertyBlock();
		renderer.SetPropertyBlock(props);
		shaderID = Shader.PropertyToID("_Moved");
		if(cameraGO.Value != null) {
			var angle = Vector3.Angle(transform.forward, cameraGO.Value.transform.forward);
			if(angle > 90.0f) {
				shderValue = 1.0f;
				props.SetFloat(shaderID, shderValue);
				renderer.SetPropertyBlock(props);
				DestroyImmediate(this);
			} 
		}
	}

	private void OnEnable()
	{
		onPlayerMove.onEvent += OnPlayerMove;
	}

	private void OnDisable()
	{
		onPlayerMove.onEvent -= OnPlayerMove;
	}

	private void OnPlayerMove(Vector3 position)
	{
		Vector3 offset = renderer.transform.position - position;
		float sqrLen = offset.sqrMagnitude;
		var isValid = disappearsOnClose ? sqrLen > radius * radius : sqrLen < radius * radius;
		if(isValid) {
			shderValue = Mathf.Lerp(shderValue, 1, Time.deltaTime * appearSpeed);
		}
		else if(!keep) {
			shderValue = Mathf.Lerp(shderValue, 0, Time.deltaTime * disappearSpeed);
		}
		props.SetFloat(shaderID, shderValue);
		renderer.SetPropertyBlock(props);
	}
}
