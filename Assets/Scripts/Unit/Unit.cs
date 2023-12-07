using UnityEngine;

[RequireComponent(typeof(UnitMover))]
public class Unit : MonoBehaviour
{
    private Resource _currentResource;
    private Transform _baseTransform;
    private UnitMover _mover;
    private bool _isPicked;
    public bool IsBusy { get; private set; }

    public void AssignCurrentResource(Resource resource)
    {
        if(resource != null && !IsBusy) 
        {
            _currentResource = resource;
            MoveToTarget();
            IsBusy = true;
        }
    }

    private void Awake()
    {
        _baseTransform = transform.parent;
        _mover = GetComponent<UnitMover>();
    }

    private void MoveToTarget()
    {
        if (_currentResource == null) return;

        _mover.SetTarget(_currentResource.transform.position);
    }

    private void MoveToBase()
    {
        if(_baseTransform == null) return;

        _mover.SetTarget(_baseTransform.position);
    }

    private void Update()
    {
        if (_currentResource == null) return;

        if (!_isPicked && Vector3.Distance(transform.position, _currentResource.transform.position) < 1f)
        {
            PickUpResource();
        }

        if (_isPicked && Vector3.Distance(transform.position, _baseTransform.position) < 1f)
        {
            DropOffResource();
        }
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
    }
}
