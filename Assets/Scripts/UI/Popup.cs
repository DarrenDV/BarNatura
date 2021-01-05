using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Popup : MonoBehaviour
{
    public static Popup Instance;

    public Vector3 DisplayOffset = Vector3.zero;

    private BaseObject selectedObject;
    private GameObject panel;

    [HideInInspector] public bool CanOpen = true;

    [SerializeField] private Button removeButton = null;
    [SerializeField] private Text removeButtonText = null;
    [SerializeField] private Button boostButton = null;
    [SerializeField] private TextMeshProUGUI boostButtonText = null;
    [SerializeField] private Text titleText = null;
    [SerializeField] private TextMeshProUGUI descriptionText = null;

    [Header("ProgesSlider")]
    [SerializeField] private Slider progressSlider;
    [SerializeField] private TextMeshProUGUI rDrain, rGain;

    [Header("Sound")]
    [SerializeField] private float volume = 1f;
    [SerializeField] private AudioClip appearSound, disappearSound;
    private AudioSource audioSource;

    #region Defaults

    private void Awake()
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
        audioSource.volume = volume;

        panel = gameObject.transform.GetChild(0).gameObject;
    }

    public void Start()
    {
        // hide the popup when the game starts
        Hide(false);
    }

    void Update()
    {
        if (selectedObject != null)
        {
            SetPosition();

            UpdateDescription();
            DisplayRemoveButton();

            // check of the path between the main camera and the selected object is free.
            if (!CheckPathFree(Camera.main.transform.position, selectedObject.transform.position))
            {
                // if not, we hide the popup.
                Hide(false);
            }
        }
    }

    #endregion

    /// <summary>
    /// Show the popup for the selected object.
    /// </summary>
    public void Show(BaseObject objectToDisplay)
    {
        if(!CanOpen || panel.activeInHierarchy)
        {
            return;
        }

        selectedObject = objectToDisplay;
        
        if (!CheckPathFree(Camera.main.transform.position, selectedObject.transform.position))
        {
            return;
        }

        selectedObject.OnFinishedRemovingEvent.AddListener(OnSelectedBuildingRemoved);

        PlaySound(appearSound);
  
        UpdateDescription();

        SetPosition();

        panel.SetActive(true);

        DisplayRemoveButton();
        DisplayBoostButton();
    }

    /// <summary>
    /// If the selected object gets removed, we hide the popup.
    /// </summary>
    private void OnSelectedBuildingRemoved()
    {
        Hide();
    }

    private void UpdateDescription()
    {
        titleText.text = selectedObject.GetName();
        descriptionText.text = selectedObject.GetDescription();
    }

    /// <summary>
    /// Show the remove button and update the amount of humans needed.
    /// </summary>
    private void DisplayRemoveButton()
    {
        var showButton = selectedObject.CanBeRemovedByPlayer;

        removeButton.gameObject.SetActive(showButton);

        if (showButton)
        {
            removeButtonText.text = $"Remove ({selectedObject.HumansRequiredToRemove} H)";
        }
    }

    /// <summary>
    /// Make sure the popup follows the selected object.
    /// </summary>
    private void SetPosition()
    {
        transform.position = Camera.main.WorldToScreenPoint(selectedObject.transform.position) + DisplayOffset;
    }

    /// <summary>
    /// Hide the popup.
    /// </summary>
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

        selectedObject = null;
        HideProgressBar();
        boostButton.gameObject.SetActive(false);
    }

    /// <summary>
    /// Update the progress bar, used to show factory progress.
    /// </summary>
    public void SetProgressBar(float maxValue, float currentTime, string drainSprite, string gainSprite)
    {
        progressSlider.gameObject.SetActive(true);
        progressSlider.maxValue = maxValue;
        progressSlider.value = currentTime;

        rDrain.text = drainSprite;
        rGain.text = gainSprite;   
    }

    /// <summary>
    /// Hide the progress bar
    /// </summary>
    public void HideProgressBar()
    {
        progressSlider.gameObject.SetActive(false);
    }

    /// <summary>
    /// Used when the player wants to remove the selected object.
    /// </summary>
    public void OnRemoveClicked()
    {
        if (!GameManager.Instance.AreWorkersAvailable(selectedObject.HumansRequiredToRemove))
        {
            // todo: tell player he does not have enough humans
            return;
        }

        selectedObject.OnRemove();

        Hide(false);
    }

    /// <summary>
    /// Check if we have to show the boost button, and do so
    /// </summary>
    private void DisplayBoostButton()
    {
        if (selectedObject is FactoryScript fs)
        {
            boostButton.gameObject.SetActive(true);

            if (fs.boostOn)
            {
                boostButtonText.text = $"Unboost (+2{HudManager.GetIcon("Human")})";
            }
            else
            {
                boostButtonText.text = $"Boost 2x (-2{HudManager.GetIcon("Human")})";
            }
        }
        else
        {
            boostButton.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Used when the player wants to boost or un-boost the selected object.
    /// </summary>
    public void OnBoostClicked()
    {
        if (selectedObject is FactoryScript fs)
        {
            fs.ToggleBoost();
            DisplayBoostButton();
        }
    }

    /// <summary>
    /// Check if the path between position and target is free.
    /// </summary>
    private bool CheckPathFree(Vector3 position, Vector3 target)
    {
        var direction = target - position;
        var distance = Vector3.Distance(position, target);

        var raycastHits = Physics.RaycastAll(position, direction, distance);
        var filteredList = raycastHits.Where(raycastHit => !raycastHit.transform.gameObject.CompareTag("NatureDecal")).ToArray(); // we don't care about decals

        // sometimes the ray also finds the tile the building is on
        if (filteredList.Length == 2)
        {
            var tile = filteredList[0].transform.GetComponent<BaseTileScript>() != null ? filteredList[0].transform.GetComponent<BaseTileScript>() : filteredList[1].transform.GetComponent<BaseTileScript>();

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

        return filteredList.Length == 1;
    }

    private void PlaySound(AudioClip audioClip)
    {
        audioSource.clip = audioClip;
        audioSource.Play();
    }
}
