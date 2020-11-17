using UnityEngine;
using UnityEngine.UI;

public class Popup : MonoBehaviour
{
    public static Popup Instance;

    public Vector3 DisplayOffset;

    private BaseObject selectedObject;

    [SerializeField] private Text titleText;
    [SerializeField] private Text descriptionText;

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

        transform.position = Camera.main.WorldToScreenPoint(selectedObject.transform.position) + DisplayOffset;

        gameObject.SetActive(true);
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
}
