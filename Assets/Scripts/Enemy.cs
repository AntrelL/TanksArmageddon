using Assets.Constructors.FuturisticTanks.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private ParticleSystem _hitFX;
    [SerializeField] private Transform _hitPoint;
    [SerializeField] private int _health = 100;
    [SerializeField] private Tank _tank;

    private bool _isAlive = true;

    private void FixedUpdate()
    {
        if (_health <= 0)
        {
            _tank.Destroy();
            _isAlive = false;
        }
    }

    private void OnEnable()
    {
        Bullet.TankHit += PlayHitEffect;
    }

    private void OnDisable()
    {
        Bullet.TankHit -= PlayHitEffect;
    }

    private void PlayHitEffect()
    {
        if (_isAlive == true)
        {
            _health -= 50;
            ParticleSystem flash = Instantiate(_hitFX, _hitPoint.position, _hitPoint.rotation);
            flash.Play();
            Destroy(flash.gameObject, flash.main.duration);
        }
        else
        {
            return;
        }
    }
}
