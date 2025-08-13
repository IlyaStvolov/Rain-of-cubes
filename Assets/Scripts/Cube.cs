using System;
using System.Collections;
using UnityEngine;

public class Cube : MonoBehaviour
{
    [SerializeField] private float _minLifeTime;
    [SerializeField] private float _maxLifeTime;

    private bool _isColorChanged = false;

    public event Action<Cube> CubeLifeTimeOver;

    private void OnDisable()
    {
        gameObject.GetComponent<Renderer>().material.color = Color.white;
        _isColorChanged = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(_isColorChanged == false && collision.gameObject.TryGetComponent<Plane>(out Plane component))
        {
            gameObject.GetComponent<Renderer>().material.color = UnityEngine.Random.ColorHSV();
            _isColorChanged = true;
            StartCoroutine(CubeLifeTimeCountdown());
        }
    }

    private IEnumerator CubeLifeTimeCountdown()
    {
        var wait = new WaitForSeconds(UnityEngine.Random.Range(_minLifeTime, _maxLifeTime));

        yield return wait;
        CubeLifeTimeOver?.Invoke(this);
    }
}
