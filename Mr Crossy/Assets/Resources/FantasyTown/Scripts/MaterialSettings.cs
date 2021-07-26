using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Material settings. Attached to the gameobject MaterialSettings inside of each building/logic+lights. Enables you to attach materials to a whole building.
/// </summary>
public class MaterialSettings : MonoBehaviour {

	public Transform Building;
	public Material WallMaterial;
	public Material RoofMaterial;
	public Material WoodMaterial;


	private Transform [] childTransforms;

	[SerializeField] [HideInInspector]
	private Material currentWallMaterial;

	[SerializeField] [HideInInspector]
	private Material currentRoofMaterial;

	[SerializeField] [HideInInspector]
	private Material currentWoodMaterial;


	/// <summary>
	/// Raises the validate event. Check if a material has changed and apply it to appropriate meshes of the bulding attached to the variable Building.
	/// </summary>
	void OnValidate() {

		if (Building == null)
			return;


		// Change the materials for the walls. Name of the material has to start with "_Wall_"
		if (WallMaterial != null && WallMaterial.name.StartsWith("_Wall_")) {
			
			// continue only if the current material is null or it is not the same as the already asigned material
			if (currentWallMaterial == null || !(WallMaterial.name.Equals (currentWallMaterial.name))) {
				
				currentWallMaterial = WallMaterial;

				childTransforms = Building.GetComponentsInChildren<Transform> ();

				for (int i = 0; i < childTransforms.Length; i++) {
					Transform t = childTransforms [i];

					if (t.GetComponent<Renderer> () != null) {
						if (t.GetComponent<Renderer> ().sharedMaterial.name.StartsWith ("_Wall_")) {
							t.GetComponent<Renderer> ().sharedMaterial = WallMaterial;
						}
					}
				}
			}
		}

		// Change the materials for the roof
		if (RoofMaterial !=null  && RoofMaterial.name.StartsWith("_Roof_")) {
			
			// continue only if the current material is null or it is not the same as the already asigned material
			if (currentRoofMaterial == null || !(RoofMaterial.name.Equals (currentRoofMaterial.name))) {
				
				currentRoofMaterial = RoofMaterial;

				childTransforms = Building.GetComponentsInChildren<Transform> ();

				for (int i = 0; i < childTransforms.Length; i++) {
					Transform t = childTransforms [i];

					if (t.GetComponent<Renderer> () != null) {
						if (t.GetComponent<Renderer> ().sharedMaterial.name.StartsWith ("_Roof_")) {
							t.GetComponent<Renderer> ().sharedMaterial = RoofMaterial;
						}
					}
				}
			}
		}


		// Change the materials for the wood
		if (WoodMaterial !=null  && WoodMaterial.name.StartsWith("_Wood_Base_01_")) {

			// continue only if the current material is null or it is not the same as the already asigned material
			if (currentWoodMaterial == null || !(WoodMaterial.name.Equals (currentWoodMaterial.name))) {

				currentWoodMaterial = WoodMaterial;

				childTransforms = Building.GetComponentsInChildren<Transform> ();

				for (int i = 0; i < childTransforms.Length; i++) {
					Transform t = childTransforms [i];

					if (t.GetComponent<Renderer> () != null) {
						if (t.GetComponent<Renderer> ().sharedMaterial.name.StartsWith ("_Wood_Base_01_")) {
							t.GetComponent<Renderer> ().sharedMaterial = WoodMaterial;
						}
					}
				}
			}
		}






	}
}
