using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShirtlessZombieInstantiate : MonoBehaviour
{
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

	public Transform prefabObject;
	public MeshVariant meshVar;
	public BodyMaterial bodyMat;
	public ClothesMaterial clothesMat;

	// Use this for initialization
	void Start ()
	{
		Transform pref = Instantiate (prefabObject, gameObject.transform.position, gameObject.transform.rotation);
		meshV = (int)meshVar;
		bodyM = (int)bodyMat;
		clothesM = (int)clothesMat;

		pref.gameObject.GetComponent<ShirtlessZombieCustomization> ().charCustomize (meshV, bodyM, clothesM);

	}
	

}
