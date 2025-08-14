using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class CubesSpawner : MonoBehaviour
{
    [SerializeField] private Cube _cubePrefab;
    [SerializeField] private Plane _spawnPlane;
    [SerializeField] private int _poolCapacity;
    [SerializeField] private int _poolMaxSize;
    [SerializeField] private float _repeatRate;

    private ObjectPool<Cube> _pool;

    private void Awake()
    {
        _pool = new ObjectPool<Cube>(
            createFunc: () => Instantiate(_cubePrefab),
            actionOnGet: (cube) => ActionOnGet(cube),
            actionOnRelease: (cube) => ActionOnRelease(cube),
            actionOnDestroy: (cube) => Destroy(cube),
            collectionCheck: true,
            defaultCapacity: _poolCapacity,
            maxSize: _poolMaxSize);
    }

    private void Start()
    {
        StartCoroutine(GetCubeCountdown(_repeatRate));
    }

    private void ActionOnGet(Cube cube)
    {
        cube.LifeTimeOver += _pool.Release;

        cube.transform.position = GetSpawnCoordinate();
        cube.gameObject.SetActive(true);
    }

    private void ActionOnRelease(Cube cube)
    {
        cube.LifeTimeOver -= _pool.Release;

        cube.gameObject.SetActive(false);
    }

    private IEnumerator GetCubeCountdown(float repeatRate)
    {
        var wait = new WaitForSeconds(repeatRate);

        while (enabled)
        {
            _pool.Get();
            yield return wait;
        }
    }

    private Vector3 GetSpawnCoordinate()
    {
        float heightY = 8f;
        float minRandomX = -4f;
        float maxRandomX = 4f;
        float minRandomZ = -4f;
        float maxRandomZ = 4f;

        float pointX = _spawnPlane.transform.position.x + Random.Range(minRandomX, maxRandomX);
        float pointZ = _spawnPlane.transform.position.z + Random.Range(minRandomZ, maxRandomZ);
        float pointY = _spawnPlane.transform.position.y + heightY;

        return new Vector3(pointX, pointY, pointZ);
    }
}
