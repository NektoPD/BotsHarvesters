using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    private void Awake()
    {
        _units.AddRange(FindObjectsOfType<Unit>());

        foreach (var unit in _units)
        {
            unit.OnAvailable += HandleUnitsAvailable;
        }
    }

    private void OnDestroy()
    {
        foreach (var unit in _units)
        {
            unit.OnAvailable -= HandleUnitsAvailable;
        }
    }

    public void HandleUnitsAvailable()
    {
        StartCoroutine(ScanForResources());
        AssignResourcesToUnits();
    }

    private IEnumerator ScanForResources()
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

        yield return null;
    }

    private void AssignResourcesToUnits()
    {
        while (_resources.Count > 0)
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
        if(other.TryGetComponent<Resource>(out  Resource resource))
        {
            _gotResource.Invoke();
        }
    }
}
