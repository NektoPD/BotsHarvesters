using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BaseScanner))]
public class Base : MonoBehaviour
{
    [SerializeField] private int _resourceCount = 0;

    private BaseScanner _baseScanner;
    private List<Unit> _units = new List<Unit>();
    private List<Resource> _filteredResources = new List<Resource>();
    private Unit _availableUnit;

    private void Start()
    {
        _units.AddRange(FindObjectsOfType<Unit>());
        _baseScanner = GetComponent<BaseScanner>();

        _baseScanner.Detected += AssignResourcesToUnits;

        StartCoroutine(FindAvailableUnit(_units));
    }

    public void IncreaseResourceCount()
    {
        _resourceCount++;
    }

    private void AssignResourcesToUnits()
    {
        Resource resource = _baseScanner.GetResource();
       
        if (resource != null && !_filteredResources.Contains(resource))
        {
            _filteredResources.Add(resource);
            _availableUnit.AssignCurrentResource(resource, resource.transform);
        }
        else
        {
            return;
        }
    }

    private IEnumerator FindAvailableUnit(List<Unit> units)
    {
        while (true)
        {
            foreach (Unit unit in units)
            {
                if (!unit.IsBusy)
                {
                    _availableUnit = unit;
                    _baseScanner.ScanForResources();
                }
            }

            yield return null;
        }
    }
}
