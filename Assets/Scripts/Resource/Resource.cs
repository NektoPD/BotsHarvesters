using UnityEngine;

public class Resource : MonoBehaviour
{
    public bool IsAssigend { get; private set; } = false;

    public void Assign()
    {
        IsAssigend = true;
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
}
