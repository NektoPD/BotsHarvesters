using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Base : MonoBehaviour
{
    [SerializeField] private float _scanRadius;
    [SerializeField] private LayerMask _resourceLayer;
    [SerializeField] private UnityEvent _gotResource;
    private List<Unit> _units = new List<Unit>();
    private Queue<Resource> _resources = new Queue<Resource>();
    private int _resourceCount = 0;

    public void IncreaseResourceCount()
    {
        _resourceCount++;
    }

    private void Start()
    {
        _units.AddRange(GetComponentsInChildren<Unit>());
    }

    private void Update()
    {
        ScanForResources();
        AssignResourcesToUnits();
        Debug.Log("Resources obtained: " + _resourceCount);
    }

    private void ScanForResources()
    {
        Collider[] resourceColliders = Physics.OverlapSphere(transform.position, _scanRadius, _resourceLayer);

        foreach (var collider in resourceColliders)
        {
            Resource resource = collider.GetComponent<Resource>();
            if (resource != null && !resource.IsAssigend)
            {
                _resources.Enqueue(resource);
                resource.Assign();
            }
        }
    }

    private void AssignResourcesToUnits()
    {
        while (_resources.Count > 0 && _units.Exists(unit => !unit.IsBusy))
        {
            Resource resource = _resources.Peek();
            Unit availableUnit = _units.Find(unit => !unit.IsBusy);
            if (availableUnit != null)
            {
                availableUnit.AssignCurrentResource(resource);
                _resources.Dequeue(); 
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<Resource>(out Resource resource))
        {
            _gotResource.Invoke();
        }
    }
}
