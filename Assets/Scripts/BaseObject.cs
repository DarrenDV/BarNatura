using Assets.Scripts;
using UnityEngine;
using UnityEngine.Events;

public class BaseObject : MonoBehaviour
{
    #region Building & Removing

    [Header("Base Object")]
    public bool CanBeRemovedByPlayer = true;
    public int HumansRequiredToBuild = 5;
    public int HumansRequiredToRemove = 5;

    [SerializeField] private float buildTime = 5f;
    [SerializeField] private float removeTime = 5f;

    public UnityEvent OnFinishedBuildingEvent;
    public UnityEvent OnFinishedRemovingEvent;

    /// <summary>
    /// The tile this object is placed on.
    /// </summary>
    [HideInInspector] public BaseTileScript parentTile;

    /// <summary>
    /// Is this object currently getting build.
    /// </summary>
    [HideInInspector] public bool IsBeingBuild;

    /// <summary>
    /// Is this object currently getting removed.
    /// </summary>
    [HideInInspector] public bool IsBeingRemoved;

    private Vector3 originalScale;
    private float buildProgress = 1f; // 1 = fully build, 0 = fully removed

    #endregion

    protected virtual void Start()
    {
        originalScale = transform.localScale;
    }

    protected virtual void Update()
    {
        if (IsBeingBuild)
        {
            buildProgress += Time.deltaTime / buildTime;

            if (buildProgress >= 1)
            {
                buildProgress = 1f;
                IsBeingBuild = false;

                OnFinishedBuilding();
            }

            UpdateScale();
        }
        else if (IsBeingRemoved)
        {
            buildProgress -= Time.deltaTime / removeTime; // 5 sec remove time

            if (buildProgress <= 0)
            {
                buildProgress = 0f;
                IsBeingRemoved = false;

                OnFinishedRemoving();
            }
            else
            {
                UpdateScale();
            }
        }
    }

    private void UpdateScale()
    {
        transform.localScale = Vector3.Lerp(Vector3.zero, originalScale, buildProgress);
    }

    public virtual string GetName()
    {
        return gameObject.name;
    }

    /// <summary>
    /// Get the description for when this object is in use.
    /// </summary>
    /// <returns></returns>
    public virtual string GetDescription()
    {
        // if not being build or destroyed, get description of own object, to be overwritten by object.
        return "No description set up for this object!";
    }

    /// <summary>
    /// Get the description when the player wants to build this object.
    /// </summary>
    /// <returns></returns>
    public virtual string GetBuildDescription()
    {
        return $"Humans required:\n{HumansRequiredToBuild}";
    }

    /// <summary>
    /// Get the description when this object is getting build.
    /// </summary>
    /// <returns></returns>
    public virtual string GetWhileBeingBuildDescription()
    {
        return $"This {GetName()} is being build.";
    }

    /// <summary>
    /// Get the description when this object is getting removed.
    /// </summary>
    /// <returns></returns>
    public virtual string GetRemoveDescription()
    {
        return $"This {GetName()} is being removed.";
    }

    public virtual void OnBuild()
    {
        GameManager.Instance.RemovePopulation(HumansRequiredToBuild);
        buildProgress = 0f;
        IsBeingBuild = true;
    }

    /// <summary>
    /// Called when this object is starting to get removed.
    /// </summary>
    public virtual void OnRemove()
    {
        GameManager.Instance.RemovePopulation(HumansRequiredToRemove);
        IsBeingRemoved = true;
    }

    /// <summary>
    /// Called when the object is finished being build.
    /// </summary>
    public virtual void OnFinishedBuilding()
    {
        GameManager.Instance.AddPopulation(HumansRequiredToBuild);
        OnFinishedBuildingEvent.Invoke();
    }

    /// <summary>
    /// Called when the object is finished being removed.
    /// </summary>
    public virtual void OnFinishedRemoving()
    {
        GameManager.Instance.AddPopulation(HumansRequiredToRemove);
        OnFinishedRemovingEvent.Invoke();
    }

    protected virtual void OnMouseDown()
    {
        if (Utils.IsPointerOverUIElement() == false)
        {
            Popup.Instance.Show(this);
        }
    }
}
