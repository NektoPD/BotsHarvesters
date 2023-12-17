using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BaseScanner))]
public class Base : MonoBehaviour
{
    [SerializeField] private int _resourceCount = 0;

    private BaseScanner _baseScanner;
    private List<Unit> _units = new List<Unit>();
    private List<Resource> _filteredResources = new List<Resource>();

    private void Start()
    {
        _units.AddRange(FindObjectsOfType<Unit>());
        _baseScanner = GetComponent<BaseScanner>();

        _baseScanner.Detected += AssignResourcesToUnits;
    }

    public void IncreaseResourceCount()
    {
        _resourceCount++;
    }

    private void AssignResourcesToUnits()
    {
        Unit availableUnit = FindAvailableUnit(_units);
        Resource resource = _baseScanner.GetResource();
       
        if (resource != null && !_filteredResources.Contains(resource))
        {
            _filteredResources.Add(resource);
            availableUnit.AssignCurrentResource(resource, resource.transform);
        }
        else
        {
            return;
        }
    }

    private Unit FindAvailableUnit(List<Unit> units)
    {
        foreach (Unit unit in units)
        {
            if (!unit.IsBusy)
            {
                return unit;
            }
        }

        return null;
    }
}
