using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DriveThroughManager : MonoBehaviour
{
    public static DriveThroughManager instance;
    [SerializeField] List<CarController> _carPrefabs = new List<CarController>();
    [SerializeField] List<CarController> _carQueue = new List<CarController>();
    [SerializeField] List<Transform> _waitPoints = new List<Transform>();
    [SerializeField] Transform _spawnPoint;
    [SerializeField] Transform _despawnPoint;
    float _spawnTime = 1f;

    int _maxCars, _currentQueue = 0;
    float _ctr = 0f;
    int lastCarrIndex = -1;
    bool _canSpawn = true;


    private void Awake()
    {
        instance = this;
        _maxCars = _waitPoints.Count;

    }


    private void Update()
    {
        if (DriveThroughCashier.instance != null && DriveThroughCashier.instance.unlocked) return;
        if (!_canSpawn)
        {
            _ctr = 0;
            return;
        }

        _ctr += Time.deltaTime;
        if (_ctr >= _spawnTime)
        {
            _ctr = 0f;

            SpawnCar();

        }
    }

    // [Button(ButtonSizes.Small)]
    private void SpawnCar()
    {
        if (_carQueue.Count >= _maxCars) return;

        var randIndex = Random.Range(0, _carPrefabs.Count);
        while (randIndex == lastCarrIndex && lastCarrIndex != -1)
        {
            randIndex = Random.Range(0, _carPrefabs.Count);
        }
        lastCarrIndex = randIndex;
        CarController car = Instantiate(_carPrefabs[randIndex], _spawnPoint.position, Quaternion.LookRotation(Vector3.forward));

        car.transform.SetParent(transform);
        _carQueue.Add(car);
        car.SetTarget(_waitPoints[_currentQueue]);
        _currentQueue++;


    }

    public void RemoveCar()
    {
        if (_carQueue.Count > 0)
        {

            _carQueue.RemoveAt(0);
            _currentQueue--;
            for (int i = 0; i < _carQueue.Count; i++)
            {
                _carQueue[i].SetTarget(_waitPoints[i]);
            }
            _canSpawn = true;
        }

    }


    public void GetCone()
    {
        if (_carQueue.Count > 0)
        {
            if (DriveThroughCashier.instance != null)
            {
                CarController car = _carQueue[0];
                car.SetTarget(DriveThroughCashier.instance.GetCashierTrans());
            }
            else
            {
               // Debug.Log("not unlocked");
                return;
            }
        }
    }

    public Transform GetDeSpawnPoint()
    {
        return _despawnPoint;
    }

}
