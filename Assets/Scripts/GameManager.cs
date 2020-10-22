using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Building")]
    public GameObject previewObjectParent;
    [HideInInspector] public GameObject buildObject;
    [HideInInspector] public GameObject buildObjectPreview;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            StopBuilding();
        }
    }

    #region Building 

    public void ChangeBuildObject(GameObject newObject, GameObject previewObject)
    {
        buildObject = newObject;
        buildObjectPreview = previewObject;
        Instantiate(buildObjectPreview.gameObject, transform.position, transform.rotation, previewObjectParent.transform);
    }
        
    public void StopBuilding()
    {
        buildObject = null;
        if (previewObjectParent.transform.childCount != 0) Destroy(previewObjectParent.transform.GetChild(0).gameObject);
        previewObjectParent.transform.position = Vector3.zero;
        previewObjectParent.transform.rotation = Quaternion.identity;
    }

    #endregion
}
