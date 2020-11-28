using UnityEngine;
using UnityEngine.UI;

public class Popup : MonoBehaviour
{
    public static Popup Instance;

    public Vector3 DisplayOffset = Vector3.zero;

    private BaseObject selectedObject;

    [SerializeField] private Button removeButton = null;
    [SerializeField] private Text removeButtonText = null;
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

            UpdateDescription();
            DisplayRemoveButton();

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
        selectedObject.OnFinishedRemovingEvent.AddListener(OnSelectedBuildingRemoved);

        titleText.text = selectedObject.GetName();
        UpdateDescription();

        SetPosition();

        gameObject.SetActive(true);

        DisplayRemoveButton();
    }

    private void OnSelectedBuildingRemoved()
    {
        Hide();
    }

    private void UpdateDescription()
    {
        if (selectedObject.IsBeingBuild)
        {
            descriptionText.text = selectedObject.GetWhileBeingBuildDescription();
        }
        else if(selectedObject.IsBeingRemoved)
        {
            descriptionText.text = selectedObject.GetRemoveDescription();
        }
        else
        {
            descriptionText.text = selectedObject.GetDescription();
        }
    }

    private void DisplayRemoveButton()
    {
        if (selectedObject.IsBeingBuild || selectedObject.IsBeingRemoved)
        {
            removeButton.gameObject.SetActive(false);
        }
        else
        {
            var showButton = selectedObject.CanBeRemovedByPlayer;

            removeButton.gameObject.SetActive(showButton);

            if (showButton)
            {
                removeButtonText.text = $"Remove ({selectedObject.HumansRequiredToRemove} H)";
            }
        }
    }

    private void SetPosition()
    {
        transform.position = Camera.main.WorldToScreenPoint(selectedObject.transform.position) + DisplayOffset;
    }

    public void Hide()
    {
        gameObject.SetActive(false);

        if (selectedObject != null)
        {
            selectedObject.OnFinishedRemovingEvent.RemoveListener(OnSelectedBuildingRemoved);
        }
    }

    public void OnRemoveClicked()
    {
        if (GameManager.Instance.GetPopulationAmount() < selectedObject.HumansRequiredToRemove)
        {
            // todo: tell player he does not have enough humans
            return;
        }

        selectedObject.OnRemove();
        Hide();
    }

    private bool CheckPathFree(Vector3 position, Vector3 target)
    {
        var direction = target - position;
        var distance = Vector3.Distance(position, target);

        var raycastHits = Physics.RaycastAll(position, direction, distance);

        // sometimes the ray also finds the tile the building is on
        if (raycastHits.Length == 2)
        {
            var tile = raycastHits[0].transform.GetComponent<BaseTileScript>() != null ? raycastHits[0].transform.GetComponent<BaseTileScript>() : raycastHits[1].transform.GetComponent<BaseTileScript>();

            //This causes a bug for some reason
            if(tile.PlacedObjects.Contains(selectedObject.gameObject))
            {
                return true;
            }
        }

        return raycastHits.Length == 1;
    }
}
