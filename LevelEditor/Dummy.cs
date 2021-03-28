
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dummy : MonoBehaviour {
	public MeshRenderer Renderer;

	public int CubeType;
	public int CubeData;

	public GameObject selectedOutline;

	public void ChangeCubeData(bool setNewValue, bool up, int newValue) {
		if (!setNewValue) {
			if (up)
				CubeData++;
			else
				CubeData--;
		} else {
			CubeData = newValue;
		}

		if (CubeType == 01) {	
			
			if (CubeData < 00)
				CubeData = CubeData + 06;
			if (CubeData > 05)
				CubeData = CubeData - 06;
			
			if (CubeData == 00)
				Renderer.material.SetColor ("_Color", Color.HSVToRGB (108.0f / 360.0f, 0.5f, 1f));
			else if (CubeData == 01)
				Renderer.material.SetColor ("_Color", Color.HSVToRGB (351.0f / 360.0f, 0.5f, 1f));
			else if (CubeData == 02)
				Renderer.material.SetColor ("_Color", Color.HSVToRGB (63.0f / 360.0f, 0.5f, 1f));
			else if (CubeData == 03)
				Renderer.material.SetColor ("_Color", Color.HSVToRGB (29.0f / 360.0f, 0.5f, 1f));
			else if (CubeData == 04)
				Renderer.material.SetColor ("_Color", Color.HSVToRGB (262.0f / 360.0f, 0.5f, 1f));
			else if (CubeData == 05)
				Renderer.material.SetColor ("_Color", Color.HSVToRGB (215.0f / 360.0f, 0.5f, 1f));
		}
		else if (CubeType == 02 || CubeType == 03) {
			
			if (CubeData < 00)
				CubeData = CubeData + 07;
			if (CubeData > 06)
				CubeData = CubeData - 07;
			
			if (CubeData == 00)
				Renderer.material.SetColor ("_EmissionColor", new Color32 (0, 48, 0, 255));
			else if (CubeData == 01)
				Renderer.material.SetColor ("_EmissionColor", new Color32 (69 /*nice*/, 0, 0, 255));
			else if (CubeData == 02)
				Renderer.material.SetColor ("_EmissionColor", new Color32 (94, 73, 0, 255));
			else if (CubeData == 03)
				Renderer.material.SetColor ("_EmissionColor", new Color32 (101, 37, 0, 255));
			else if (CubeData == 04)
				Renderer.material.SetColor ("_EmissionColor", new Color32 (64, 0, 128, 255));
			else if (CubeData == 05)
				Renderer.material.SetColor ("_EmissionColor", new Color32 (0, 37, 78, 255));
			else if (CubeData == 06)
				Renderer.material.SetColor ("_EmissionColor", new Color32 (0, 0, 0, 255));
		}
		else if (CubeType == 04 || CubeType == 05 || CubeType == 06) {
			
			if (CubeData < 00)
				CubeData = CubeData + 06;
			if (CubeData > 05)
				CubeData = CubeData - 06;
			
			if (CubeData == 00)
				Renderer.material.SetColor ("_EmissionColor", new Color32 (0, 255, 25, 255));
			else if (CubeData == 01)
				Renderer.material.SetColor ("_EmissionColor", new Color32 (255, 0, 0, 255));
			else if (CubeData == 02)
				Renderer.material.SetColor ("_EmissionColor", new Color32 (255, 174, 0, 255));
			else if (CubeData == 03)
				Renderer.material.SetColor ("_EmissionColor", new Color32 (255, 79, 0, 255));
			else if (CubeData == 04)
				Renderer.material.SetColor ("_EmissionColor", new Color32 (128, 0, 255, 255));
			else if (CubeData == 05)
				Renderer.material.SetColor ("_EmissionColor", new Color32 (0, 86, 255, 255));
		} else if (CubeType == 07 || CubeType == 08 || CubeType == 09 || CubeType == 10 || CubeType == 11 || CubeType == 12)
			CubeData = 0;

		else if (CubeType == 98 || CubeType == 99) {
			if (CubeData < 00)
				CubeData = CubeData + 02;
			if (CubeData > 01)
				CubeData = CubeData - 02;

			Vector3 newRotation = Vector3.zero;

			if (CubeData == 00)
				newRotation = new Vector3 (0, 0, 0);
			else if (CubeData == 01)
				newRotation = new Vector3 (0, 180, 0);

			transform.rotation = Quaternion.Euler(newRotation);
		}
	}
}
