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
    [Require] private WeaponsFxReader weaponsFxReader;

    private void OnEnable()
    {
        weaponsFxReader.OnMachineGunEffectEvent += FireMachineGunFX;
        weaponsFxReader.OnDamageEffectEvent += DamageFx;
    }

    private void FireMachineGunFX(Empty empty)
    {
        machineGunFx.GetComponent<ParticleSystem>().Play();
    }

    private void DamageFx(Empty empty)
    {
        damageFx.GetComponent<ParticleSystem>().Play();
    }
}
