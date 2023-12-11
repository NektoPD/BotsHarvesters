using System;
using UnityEngine;

[RequireComponent(typeof(UnitMover))]
public class Unit : MonoBehaviour
{
    public Action OnAvailable;
    private Resource _currentResource;
    [SerializeField] private Base _base;
    private UnitMover _mover;
    private bool _isPicked;
    public bool IsBusy { get; private set; }

    public void AssignCurrentResource(Resource resource)
    {
        if (resource == null || IsBusy) return;

        _currentResource = resource;
        MoveToTarget();
        IsBusy = true;
    }

    private void Start()
    {
        _base = GetComponent<Base>();
        _mover = GetComponent<UnitMover>();
        IsBusy = false;

        OnAvailable.Invoke();
    }

    private void MoveToTarget()
    {
        if (_currentResource == null) return;

        _mover.SetTarget(_currentResource.transform.position);
    }

    private void MoveToBase()
    {
        if (_base == null) return;

        _mover.SetTarget(_base.transform.position);
    }

    private void PickUpResource()
    {
        _isPicked = true;
        _currentResource.transform.SetParent(transform);
        MoveToBase();
    }

    private void DropOffResource()
    {
        _isPicked = false;
        _currentResource.transform.SetParent(null);
        _currentResource.Destroy();

        IsBusy = false;

        OnAvailable?.Invoke();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!_isPicked && other.gameObject.TryGetComponent(out Resource resource))
        {
            PickUpResource();
        }
        else if (_isPicked && other.gameObject.TryGetComponent<Base>(out Base @base))
        {
            DropOffResource();
        }
    }
}
