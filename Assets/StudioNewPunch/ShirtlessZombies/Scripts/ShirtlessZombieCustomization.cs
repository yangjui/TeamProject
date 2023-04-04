using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShirtlessZombieCustomization : MonoBehaviour
{

	// Use this for initialization
	private int meshV;
	private int bodyM;
	private int clothesM;
	private AssetsList assetsList;
	//private GameObject curSubObject;
	//private GameObject curObject;

	public enum MeshVariant
	{
		V1,
		V2,
		V3,
		V4,
		V5
	}

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

	public MeshVariant meshVar;
	public BodyMaterial bodyMat;
	public ClothesMaterial clothesMat;


    private void Awake()
    {
		MeshVariant randomMeshVar = (MeshVariant)Random.Range(0, 4);
		BodyMaterial randomBodyMat = (BodyMaterial)Random.Range(0, 2);
		ClothesMaterial randomClothesMat = (ClothesMaterial)Random.Range(0, 4);
		charCustomize((int)randomMeshVar, (int)randomBodyMat, (int)randomClothesMat);
	}

    void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	public void charCustomize (int mVar, int bMat, int cMat)
	{
		assetsList = gameObject.GetComponent<AssetsList> ();
		for (int i = 0; i < assetsList.modelsMain.Length; i++) {
			assetsList.modelsMain [i].SetActive (false);
		}
		//assetsList.modelsMain [mVar].SetActive (true);
		GameObject curObject = assetsList.modelsMain [mVar];
		curObject.SetActive (true);
		string curN = mVar.ToString ();
		if (curN == "0") {
			for (int i = 0; i < 3; i++) {


				Transform curSub = curObject.transform.Find ("Body_LOD" + i);
				Renderer skinRend = curSub.GetComponent<Renderer> ();
				skinRend.material = assetsList.v1BodyMaterials [bMat];

			}

		} else {
			for (int i = 0; i < 3; i++) {
			
				
				Transform curSub = curObject.transform.Find ("Body_LOD" + i + " " + curN);
				Renderer skinRend = curSub.GetComponent<Renderer> ();
				if (curN == "1") {
					skinRend.material = assetsList.v2BodyMaterials [bMat];

				}
				if (curN == "2") {
					skinRend.material = assetsList.v3BodyMaterials [bMat];

				}

				if (curN == "3") {
					skinRend.material = assetsList.v4BodyMaterials [bMat];

				}
				if (curN == "4") {
					skinRend.material = assetsList.v5BodyMaterials [bMat];

				}


			}
		}
		if (curN == "0") {
			for (int i = 0; i < 3; i++) {


				Transform curSub = curObject.transform.Find ("Clothes_LOD" + i);
				Renderer skinRend = curSub.GetComponent<Renderer> ();
				skinRend.material = assetsList.ClothesMaterials [cMat];

			}

		} else {
			for (int i = 0; i < 3; i++) {

				Transform curSub = curObject.transform.Find ("Clothes_LOD" + i + " " + curN);
				Renderer skinRend = curSub.GetComponent<Renderer> ();
				skinRend.material = assetsList.ClothesMaterials [cMat];



			}
		}

	}

	void OnValidate ()
	{
		//code for In Editor customize
		meshV = (int)meshVar;
		bodyM = (int)bodyMat;
		clothesM = (int)clothesMat;

		charCustomize (meshV, bodyM, clothesM);

	}
}
