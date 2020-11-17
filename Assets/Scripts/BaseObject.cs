using UnityEngine;

public class BaseObject : MonoBehaviour
{
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
        Popup.Instance.Show(this);
    }
}
