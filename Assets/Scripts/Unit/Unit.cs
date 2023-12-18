using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(UnitResourceCollector))]
[RequireComponent(typeof(UnitMover))]
public class Unit : MonoBehaviour
{
    [SerializeField] private Base _base;

    public UnityEvent IsFree;
    public UnityEvent Dropped;
    private Resource _currentResource;
    private UnitMover _mover;
    private UnitResourceCollector _collector;
    
    public bool IsBusy { get; private set; }

    private void Start()
    {
        IsBusy = false;
        _mover = GetComponent<UnitMover>();
        _collector = GetComponent<UnitResourceCollector>();

        IsFree.Invoke();
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

    private void OnTriggerEnter(Collider other)
    {
        if (!_collector.IsPicked && other.TryGetComponent<Resource>(out Resource resource))
        {
            if (resource == _currentResource)
            {
                _collector.PickUpResource(_currentResource);
                SetTarget(_base.transform.position);
            }
        }
        else if (_collector.IsPicked && other.TryGetComponent<Base>(out Base @base))
        {
            _collector.DropOffResource(_currentResource);
            IsBusy = false;
            Dropped.Invoke();
        }
    }

    private void SetTarget(Vector3 targetPosition)
    {
        _mover.SetTarget(targetPosition);
    }
}
