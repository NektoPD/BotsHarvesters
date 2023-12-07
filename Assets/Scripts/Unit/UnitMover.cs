using UnityEngine;

public class UnitMover : MonoBehaviour
{
    [SerializeField] private float _speed;
    private Vector3 _targetPosition;
    private bool _haveTarget = false;

    private void Update()
    {
        if (_haveTarget)
        {
            transform.position = Vector3.MoveTowards(transform.position, _targetPosition, _speed * Time.deltaTime);
        }
       
    }

    public void SetTarget(Vector3 targetPosition)
    {
        if (targetPosition != null)
        {
            _targetPosition = targetPosition;
            _haveTarget = true;
        }
    }
}
