using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Cube : MonoBehaviour
{
    [SerializeField] private float _minLifeTime;
    [SerializeField] private float _maxLifeTime;

    private Renderer _renderer;
    private Rigidbody _rigidbody;
    private Color _defaultColor;
    private bool _isColorChanged = false;

    public event Action<Cube> LifeTimeOver;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
        _rigidbody = GetComponent<Rigidbody>();
        _defaultColor = _renderer.material.color;
    }

    private void OnDisable()
    {
        ResetToDefault();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_isColorChanged == false && collision.gameObject.TryGetComponent<Plane>(out Plane component))
        {
            _renderer.material.color = UnityEngine.Random.ColorHSV();
            _isColorChanged = true;
            StartCoroutine(CountDownLifeTime());
        }
    }

    private IEnumerator CountDownLifeTime()
    {
        var wait = new WaitForSeconds(UnityEngine.Random.Range(_minLifeTime, _maxLifeTime));

        yield return wait;
        LifeTimeOver?.Invoke(this);
    }

    private void ResetToDefault()
    {
        transform.rotation = Quaternion.identity;
        _rigidbody.velocity = Vector3.zero;
        _renderer.material.color = _defaultColor;
        _isColorChanged = false;
    }
}
