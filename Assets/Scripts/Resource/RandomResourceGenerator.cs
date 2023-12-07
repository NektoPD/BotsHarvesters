using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Terrain))]
public class RandomResourseGenerator : MonoBehaviour
{
    [SerializeField] private Resource _template;
    private Terrain _terrain;
    private float _cooldownTime = 1;

    private void Start()
    {
        _terrain = GetComponent<Terrain>();
        StartCoroutine(SpawnObject());
    }

    private IEnumerator SpawnObject()
    {
        bool isWorking = true;

        while (isWorking)
        {
            float minX = _terrain.transform.position.x;
            float maxX = minX + _terrain.terrainData.size.x;
            float minZ = _terrain.transform.position.z;
            float maxZ = minZ + _terrain.terrainData.size.z;
            WaitForSeconds cooldown = new WaitForSeconds(_cooldownTime);

            Instantiate(_template, new Vector3(Random.Range(minX, maxX), 1, Random.Range(minZ, maxZ)), Quaternion.identity);

            yield return cooldown;
        }
    }
}
