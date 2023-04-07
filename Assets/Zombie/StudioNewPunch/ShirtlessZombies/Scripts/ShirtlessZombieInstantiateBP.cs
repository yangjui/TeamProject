using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShirtlessZombieInstantiateBP : MonoBehaviour
{
	
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

	public Transform prefabObject;

	public BodyMaterial bodyMat;
	public ClothesMaterial clothesMat;

	// Use this for initialization
	void Start ()
	{
		Transform pref = Instantiate (prefabObject, gameObject.transform.position, gameObject.transform.rotation);

		bodyM = (int)bodyMat;
		clothesM = (int)clothesMat;

		pref.gameObject.GetComponent<ShirtlessZombieCustomizationBP> ().charCustomize (bodyM, clothesM);

	}

}
