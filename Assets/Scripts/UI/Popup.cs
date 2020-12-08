using UnityEngine;
using UnityEngine.UI;

public class Popup : MonoBehaviour
{
    public static Popup Instance;

    public Vector3 DisplayOffset = Vector3.zero;

    private BaseObject selectedObject;
    private GameObject panel;

    [SerializeField] private Button removeButton = null;
    [SerializeField] private Text removeButtonText = null;
    [SerializeField] private Button boostButton = null;
    [SerializeField] private Text boostButtonText = null;
    [SerializeField] private Text titleText = null;
    [SerializeField] private Text descriptionText = null;

    [Header("Sound")]
    [SerializeField] private int volume;
    [SerializeField] private AudioClip appearSound, disappearSound;
    private AudioSource audioSource;

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

        audioSource = GetComponent<AudioSource>();
        panel = gameObject.transform.GetChild(0).gameObject;
        Hide(false);
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
                Hide(false);
            }
        }
    }

    public void Show(BaseObject objectToDisplay)
    {
        // don't show when already open
        if (panel.activeInHierarchy)
        {
            return;
        }

        selectedObject = objectToDisplay;
        
        if (!CheckPathFree(Camera.main.transform.position, selectedObject.transform.position))
        {
            return;
        }

        ShowBoostButton();

        selectedObject.OnFinishedRemovingEvent.AddListener(OnSelectedBuildingRemoved);

        PlaySound(appearSound);

        titleText.text = selectedObject.GetName();
        UpdateDescription();

        SetPosition();

        panel.SetActive(true);

        DisplayRemoveButton();
    }

    private void OnSelectedBuildingRemoved()
    {
        Hide();
    }

    private void UpdateDescription()
    {
        descriptionText.text = selectedObject.GetCurrentDescription();
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

    public void Hide(bool playSound = true)
    {
        panel.SetActive(false);
        
        if (playSound)
        {
            PlaySound(disappearSound);
        }
        
        if (selectedObject != null)
        {
            selectedObject.OnFinishedRemovingEvent.RemoveListener(OnSelectedBuildingRemoved);
        }

        boostButton.gameObject.SetActive(false);
    }

    public void OnRemoveClicked()
    {
        if (!GameManager.Instance.AreWorkersAvailable(selectedObject.HumansRequiredToRemove))
        {
            // todo: tell player he does not have enough humans
            return;
        }
        
        AudioManager.Instance.PlayDemolishSound();
        Hide(false);

        selectedObject.OnRemove();
    }

    void ShowBoostButton()
    {
        if (selectedObject is FactoryScript)
        {
            FactoryScript fs = (FactoryScript)selectedObject;

            boostButton.gameObject.SetActive(true);
            if (fs.BoostOn)
            {
                boostButtonText.text = "Unboost (+2H)";
            }
            else
            {
                boostButtonText.text = "Boost 2x (-2H)";
            }
        }
    }

    public void OnBoostClicked()
    {
        if (selectedObject is FactoryScript)
        {
            FactoryScript fs = (FactoryScript)selectedObject;

            fs.ToggleBoost();
            ShowBoostButton();
        }
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

            // sometimes we hit two objects but not a tile
            if (tile == null)
            {
                return false;
            }

            if(tile.PlacedObjects.Contains(selectedObject.gameObject))
            {
                return true;
            }
        }

        return raycastHits.Length == 1;
    }

    private void PlaySound(AudioClip audioClip)
    {
        audioSource.clip = audioClip;
        audioSource.Play();
    }
}
