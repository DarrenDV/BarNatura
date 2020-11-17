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
        return "No name set up for this object!";
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
        //print(name);
        Popup.Instance.Show(this);
    }
}
