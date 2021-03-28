using UnityEngine;
using System.Collections;

public class BallColor : MonoBehaviour {
	[Header ("Settings")]
	public bool lightEnabled;
	[Header ("Current")]
	public int sphereType;
	public int lastType;
	private float HUE;
	public float speedHUE = 50;
	public bool snowed;
	[Header ("Colors and Gradients")]
	public Color desiredColor;
	private Color startColor;
	private Gradient gradient = new Gradient();
	private GradientColorKey colorKey1 = new GradientColorKey(Color.white, 0.0f);
	private GradientColorKey colorKey2 = new GradientColorKey(Color.white, 1.0f);
	private GradientColorKey[] colorKeys = new GradientColorKey[2];
	private GradientAlphaKey alphaKey1 = new GradientAlphaKey(0.25f, 0.0f);
	private GradientAlphaKey alphaKey2 = new GradientAlphaKey(0, 1.0f);
	private GradientAlphaKey[] alphaKeys = new GradientAlphaKey[2];
	[Header ("Sphere Components")]
	public Light SphereLight;
	public MeshRenderer SphereRenderer;
	public Material SphereMaterial;
	public Material SnowMaterial;
	public TrailRenderer SphereTrail;
	[Header ("Default Variables")]
	public int defSphereType;

	void Start() {
		defSphereType = sphereType;

		alphaKeys[0] = alphaKey1;
		alphaKeys[1] = alphaKey2;

		if (!lightEnabled)
			SphereLight.enabled = false;
	}

	void Update() {
		if (snowed)
			return;

		if (HUE > 360)
			HUE = HUE - 360;
		switch (sphereType) {
		case 0: 
			HUE = 108;
			break;
		case 1:
			HUE = 351;
			break;
		case 2:
			HUE = 63;
			break;
		case 3:
			HUE = 29;
			break;
		case 4:
			HUE = 262;
			break;
		case 5:
			HUE = 223;
			break;
		case 6:
			HUE += speedHUE * Time.deltaTime;
			break;
		} 

		if (SphereMaterial.GetColor("_Color") != new Color (1, 1, 1)) { 
			startColor = SphereMaterial.GetColor ("_Color");
			desiredColor = Color.Lerp (startColor, Color.HSVToRGB (HUE / 360, 0.5f, 1f), 6.0f * Time.deltaTime);
		} else 
			desiredColor = Color.HSVToRGB (HUE / 360, 0.5f, 1f);
		
		SphereMaterial.SetColor("_Color", desiredColor);
		SphereLight.color = desiredColor;
		colorKey1.color = desiredColor;
		colorKey2.color = desiredColor;
		colorKeys [0] = colorKey1;
		colorKeys [1] = colorKey2;
		gradient.SetKeys(colorKeys, alphaKeys);
		SphereTrail.colorGradient = gradient;
	}

	public void DisableLighting() {
		lightEnabled = false;
		SphereLight.enabled = false;
	}

	public void Snow() {
		snowed = true;
		SphereRenderer.material = SnowMaterial;
		if(lightEnabled)
			SphereLight.enabled = false;
		SphereTrail.enabled = false;
	}

	public void Unsnow() {
		snowed = false;
		SphereRenderer.material = SphereMaterial;
		if(lightEnabled)
			SphereLight.enabled = true;
		SphereTrail.enabled = true;
	}

	public void DestroyObject() {
		if(lightEnabled)
			SphereLight.enabled = false;
		SphereTrail.enabled = false;
	}

	public void DefaultObject() {
		sphereType = defSphereType;
		Unsnow ();
		SphereTrail.Clear ();
		SphereTrail.enabled = true;
	}
}