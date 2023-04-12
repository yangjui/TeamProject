using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShirtlessZombieCustomizationBP : MonoBehaviour
{
	
	//private int meshV;
	private int bodyM;
	private int clothesM;
	private AssetsListBP assetsList;
	//private GameObject curSubObject;
	//private GameObject curObject;


	public enum BodyMaterial
	{
		Default,
		VariantA,
		Burned
	}

	public enum ClothesMaterial
	{
		V1,
		V2,
		V3,
		V4,
		V5,
		BurnedV1,
		BurnedV2,
		BurnedV3,
		BurnedV4,
		BurnedV5
	}

	//public MeshVariant meshVar;
	public BodyMaterial bodyMat;
	public ClothesMaterial clothesMat;


	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	public void charCustomize (int bMat, int cMat)
	{
		assetsList = gameObject.GetComponent<AssetsListBP> ();
		Material[] mat;
		Transform curSub = gameObject.transform.Find ("Body_Parts/ArmL");
		Renderer skinRend = curSub.GetComponent<Renderer> ();
		skinRend.material = assetsList.BodyMaterials [bMat];

		curSub = gameObject.transform.Find ("Body_Parts/ArmR");
		skinRend = curSub.GetComponent<Renderer> ();
		skinRend.material = assetsList.BodyMaterials [bMat];

		curSub = gameObject.transform.Find ("Body_Parts/ForeArmR");
		skinRend = curSub.GetComponent<Renderer> ();
		skinRend.material = assetsList.BodyMaterials [bMat];

		curSub = gameObject.transform.Find ("Body_Parts/ForeArmL");
		skinRend = curSub.GetComponent<Renderer> ();
		skinRend.material = assetsList.BodyMaterials [bMat];

		curSub = gameObject.transform.Find ("Body_Parts/HandR");
		skinRend = curSub.GetComponent<Renderer> ();
		skinRend.material = assetsList.BodyMaterials [bMat];

		curSub = gameObject.transform.Find ("Body_Parts/HandL");
		skinRend = curSub.GetComponent<Renderer> ();
		skinRend.material = assetsList.BodyMaterials [bMat];

		curSub = gameObject.transform.Find ("Body_Parts/Head");
		skinRend = curSub.GetComponent<Renderer> ();
		skinRend.material = assetsList.BodyMaterials [bMat];
		curSub = gameObject.transform.Find ("Body_Parts/Ribcage");
		skinRend = curSub.GetComponent<Renderer> ();
		skinRend.material = assetsList.BodyMaterials [bMat];

		if (gameObject.transform.Find ("Body_Parts/Stomach") != null) {
			curSub = gameObject.transform.Find ("Body_Parts/Stomach");
			skinRend = curSub.GetComponent<Renderer> ();
			skinRend.material = assetsList.BodyMaterials [bMat];
			curSub = gameObject.transform.Find ("Body_Parts/Pelvis");
			skinRend = curSub.GetComponent<Renderer> ();
			mat = new Material[2];
			mat [1] = assetsList.BodyMaterials [bMat];
			mat [0] = assetsList.ClothesMaterials [cMat];
			skinRend.materials = mat;

		} else {
			curSub = gameObject.transform.Find ("Body_Parts/Pelvis");
			skinRend = curSub.GetComponent<Renderer> ();
			mat = new Material[2];
			mat [0] = assetsList.BodyMaterials [bMat];
			mat [1] = assetsList.ClothesMaterials [cMat];
			skinRend.materials = mat;



		}



		curSub = gameObject.transform.Find ("Body_Parts/FootL");
		skinRend = curSub.GetComponent<Renderer> ();
		skinRend.material = assetsList.BodyMaterials [bMat];

		curSub = gameObject.transform.Find ("Body_Parts/FootR");
		skinRend = curSub.GetComponent<Renderer> ();
		skinRend.material = assetsList.BodyMaterials [bMat];

		curSub = gameObject.transform.Find ("Body_Parts/LegL");
		skinRend = curSub.GetComponent<Renderer> ();
		skinRend.material = assetsList.ClothesMaterials [cMat];

		curSub = gameObject.transform.Find ("Body_Parts/LegR");
		skinRend = curSub.GetComponent<Renderer> ();
		skinRend.material = assetsList.ClothesMaterials [cMat];




		curSub = gameObject.transform.Find ("Body_Parts/LowerLegL");
		skinRend = curSub.GetComponent<Renderer> ();
		mat = new Material[2];
		mat [1] = assetsList.BodyMaterials [bMat];
		mat [0] = assetsList.ClothesMaterials [cMat];
		skinRend.materials = mat;

		curSub = gameObject.transform.Find ("Body_Parts/LowerLegR");
		skinRend = curSub.GetComponent<Renderer> ();
		mat = new Material[2];
		mat [1] = assetsList.BodyMaterials [bMat];
		mat [0] = assetsList.ClothesMaterials [cMat];
		skinRend.materials = mat;

//		curSub = gameObject.transform.Find ("Body_Parts/FootL");
//		skinRend = curSub.GetComponent<Renderer> ();
//		mat = new Material[2];
//		mat [1] = assetsList.BodyMaterials [bMat];
//		mat [0] = assetsList.ClothesMaterials [cMat];
//		skinRend.materials = mat;
//
//		curSub = gameObject.transform.Find ("Body_Parts/FootR");
//		skinRend = curSub.GetComponent<Renderer> ();
//		mat = new Material[2];
//		mat [1] = assetsList.BodyMaterials [bMat];
//		mat [0] = assetsList.ClothesMaterials [cMat];
//		skinRend.materials = mat;




	}

	void OnValidate ()
	{
		//code for In Editor customize
		//meshV = (int)meshVar;
		bodyM = (int)bodyMat;
		clothesM = (int)clothesMat;

		charCustomize (bodyM, clothesM);

	}

}
