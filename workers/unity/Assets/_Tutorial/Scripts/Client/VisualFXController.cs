using System;
using System.Collections;
using System.Collections.Generic;
using Improbable.Gdk.Core;
using Improbable.Gdk.Subscriptions;
using Tank;
using UnityEngine;

public class VisualFXController : MonoBehaviour
{
    [SerializeField] private GameObject machineGunFx;
    [SerializeField] private GameObject damageFx;
    [SerializeField] private GameObject healFx;
    [Require] private WeaponsFxReader weaponsFxReader;

    private void OnEnable()
    {
        weaponsFxReader.OnMachineGunEffectEvent += FireMachineGunFX;
        weaponsFxReader.OnDamageEffectEvent += DamageFx;
        weaponsFxReader.OnHealEffectEvent += HealFx;
    }

    private void FireMachineGunFX(Empty empty)
    {
        machineGunFx.GetComponent<ParticleSystem>().Play();
    }

    private void DamageFx(Empty empty)
    {
        damageFx.GetComponent<ParticleSystem>().Play();
    }

    private void HealFx(Empty empty)
    {
        healFx.GetComponent<ParticleSystem>().Play();
    }
}
