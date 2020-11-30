using UnityEngine;

public class BaseObject : MonoBehaviour
{
    [Header("Base Object")]
    public bool canBeRemovedByPlayer = true;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        
    }

    public virtual string GetName()
    {
        return gameObject.name;
    }

    public virtual string GetDescription()
    {
        return "No description set up for this object!";
    }

    public virtual void Remove()
    {
        Destroy(gameObject);
    }

    protected virtual void OnMouseDown()
    {
        if (!GameManager.Instance.inBuildMode) { 
            if (GameManager.Instance.IsPointerOverUIElement() == false)
            {
                Popup.Instance.Show(this);
            }
        }
    }
}
