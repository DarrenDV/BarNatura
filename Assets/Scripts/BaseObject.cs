using Assets.Scripts;
using UnityEngine;
using UnityEngine.Events;

public class BaseObject : MonoBehaviour
{
    #region Building & Removing

    [Header("Base Object")]
    public bool CanBeRemovedByPlayer = true;
    public int HumansRequiredToRemove = 5;
    protected bool canBeClikced;

    /// <summary>
    /// The time it takes to remove this object.
    /// </summary>
    [SerializeField] private float removeTime = 5f;
    public UnityEvent OnFinishedRemovingEvent;

    /// <summary>
    /// The tile this object is placed on.
    /// </summary>
    [HideInInspector] public BaseTileScript parentTile;

    /// <summary>
    /// Is this object currently getting removed.
    /// </summary>
    [HideInInspector] public bool IsBeingRemoved;

    private Vector3 originalScale;
    protected float buildProgress = 1f; // 1 = fully build, 0 = fully removed

    #endregion

    protected virtual void Start()
    {
        originalScale = transform.localScale;
    }

    protected virtual void Update()
    {
        if (IsBeingRemoved)
        {
            canBeClikced = false;
            
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
        else
        {
            if (canBeClikced == false)
            {
                canBeClikced = true;
            }
        }
    }

    protected void UpdateScale()
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
    /// Called when this object is starting to get removed.
    /// </summary>
    public virtual void OnRemove(bool instant = false)
    {
        if (!instant)
        {
            GameManager.Instance.AddWorkers(HumansRequiredToRemove);
        AudioManager.Instance.PlayDemolishSound();
            IsBeingRemoved = true;
        }
    }

    /// <summary>
    /// Called when the object is finished being removed.
    /// </summary>
    public virtual void OnFinishedRemoving()
    {
        GameManager.Instance.RemoveWorkers(HumansRequiredToRemove);
        AudioManager.Instance.PlayDisappearSound();
        parentTile.DeletePlacedObject(gameObject);
        OnFinishedRemovingEvent.Invoke();
    }

    protected virtual void OnMouseDown()
    {
        // we can't select objects in the main menu
        if (GameManager.Instance.CurrentGameState == Enums.GameState.MainMenu)
        {
            return;
        }

        if(GameManager.Instance.CurrentGameState == Enums.GameState.SelectLocation)
        {
            if (this is BaseTileScript)
            {
                // we are good to go
            }
            else
            {
                AudioManager.Instance.PlayBuildFaliedSound();
                SelectStartingLocationUiManager.Instance.ShowBuildOnFreeSpaceMessage();
            }

            return;
        }

        if (!GameManager.Instance.inBuildMode && !Utils.IsPointerOverUIElement() && canBeClikced)
        {
            Popup.Instance.Show(this);
        }
    }
}
