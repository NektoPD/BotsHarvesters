using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(UnitMover))]
public class Unit : MonoBehaviour
{
    [SerializeField] private Base _base;

    public UnityEvent Dropeed;
    private Resource _currentResource;
    private UnitMover _mover;
    private bool _isPicked;
    
    public bool IsBusy { get; private set; }

    private void Start()
    {
        _mover = GetComponent<UnitMover>();
        IsBusy = false;
    }

    public void AssignCurrentResource(Resource resource, Transform position)
    {
        if (resource != null || IsBusy == false)
        {
            _currentResource = resource;
            SetTarget(position.position);
            IsBusy = true;
        }
    }

    private void PickUpResource(Resource resource)
    {
        _isPicked = true;
        _currentResource.transform.SetParent(transform);

        SetTarget(_base.transform.position);
    }

    private void DropOffResource()
    {
        _isPicked = false;
        _currentResource.transform.SetParent(null);
        _currentResource.Destroy();

        IsBusy = false;
        Dropeed.Invoke();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!_isPicked && other.TryGetComponent<Resource>(out Resource resource))
        {
            if (resource == _currentResource)
            {
                PickUpResource(_currentResource);
            }
        }
        else if (_isPicked && other.TryGetComponent<Base>(out Base @base))
        {
            DropOffResource();
        }
    }

    private void SetTarget(Vector3 targetPosition)
    {
        _mover.SetTarget(targetPosition);
    }
}
