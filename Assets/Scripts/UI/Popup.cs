using UnityEngine;
using UnityEngine.UI;

public class Popup : MonoBehaviour
{
    public static Popup Instance;

    public Vector3 DisplayOffset = Vector3.zero;

    private BaseObject selectedObject;

    [SerializeField] private GameObject removeButton = null;
    [SerializeField] private Text titleText = null;
    [SerializeField] private Text descriptionText = null;

    public void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.Log("Popup instance already set!");
        }

        Hide();
    }

    void Update()
    {
        if (selectedObject != null)
        {
            SetPosition();

            descriptionText.text = selectedObject.GetDescription();

            if (!CheckPathFree(Camera.main.transform.position, selectedObject.transform.position))
            {
                Hide();
            }
        }
    }

    public void Show(BaseObject objectToDisplay)
    {
        // don't show when already open
        if (isActiveAndEnabled)
        {
            return;
        }

        selectedObject = objectToDisplay;

        titleText.text = selectedObject.GetName();
        descriptionText.text = selectedObject.GetDescription();

        SetPosition();

        gameObject.SetActive(true);
        removeButton.SetActive(selectedObject.canBeRemovedByUser);
    }

    private void SetPosition()
    {
        transform.position = Camera.main.WorldToScreenPoint(selectedObject.transform.position) + DisplayOffset;
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void OnRemoveClicked()
    {
        selectedObject.Remove();
        Hide();
    }

    private bool CheckPathFree(Vector3 position, Vector3 target)
    {
        var direction = target - position;
        var distance = Vector3.Distance(position, target);

        var rhit = Physics.RaycastAll(position, direction, distance);

        // sometimes the ray also finds the tile the building is on
        if (rhit.Length == 2)
        {
            var tile = rhit[0].transform.GetComponent<BaseTileScript>() != null ? rhit[0].transform.GetComponent<BaseTileScript>() : rhit[1].transform.GetComponent<BaseTileScript>();

            //This causes a bug for some reason
            if(tile.PlacedObjects.Contains(selectedObject.gameObject))
            {
                return true;
            }
        }

        //Debug.Log("rhit: " + string.Join(", ", rhit.Select(x => x.transform.name)));

        return rhit.Length == 1;
    }
}
