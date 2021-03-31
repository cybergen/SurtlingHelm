﻿using SurtlingHelm.Util;
using HarmonyLib;
using System.Linq;
using UnityEngine;

namespace SurtlingHelm.Patch
{
  internal static class PlayerPatch
  {
    internal static Harmony HarmonyInstance;

    internal static void Init()
    {
      EnableHarmonyPatches();
    }

    internal static void Disable()
    {
      DisableHarmonyPatches();
    }

    private static void EnableHarmonyPatches()
    {
      HarmonyInstance = new Harmony(SurtlingHelm.ModGuid);
      HarmonyInstance.PatchAll();
    }

    private static void DisableHarmonyPatches()
    {
      HarmonyInstance.UnpatchSelf();
    }
  }

  [HarmonyPatch(typeof(Player), "Update")]
  internal class ShowcaseHarmonyPatch
  {
    private static bool _wasFiring;
    private static Transform _leftEyeBeam;
    private static Transform _rightEyeBeam;
    private static float _hitEffectCooldown;
    private static Camera _cam;
    private static CamShaker _shaker;

    private static void Postfix(Player __instance, ref Attack ___m_currentAttack, ref float ___m_lastCombatTimer, Rigidbody ___m_body, ZSyncAnimation ___m_zanim,
      CharacterAnimEvent ___m_animEvent, VisEquipment ___m_visEquipment, Attack ___m_previousAttack, float ___m_timeSinceLastAttack)
    {
      _hitEffectCooldown -= Time.deltaTime;

      var helm = __instance.GetInventory().GetAllItems().
        FirstOrDefault(v => v.m_shared.m_name == "$custom_item_laserhelm");

      if (_cam == null) _cam = Camera.main;

      if (helm != null && helm.m_equiped)
      {
        if (Input.GetKey(SurtlingHelm.SurtlingFireKey))
        {
          if (!_wasFiring || _leftEyeBeam == null)
          {
            _wasFiring = true;
            var leftGO = GameObject.Instantiate(AssetHelper.EyeBeamPrefab);
            leftGO.SetActive(true);
            _leftEyeBeam = leftGO.transform;

            var rightGO = GameObject.Instantiate(AssetHelper.EyeBeamPrefab);
            rightGO.SetActive(true);
            _rightEyeBeam = rightGO.transform;

            _shaker = _cam.gameObject.AddComponent<CamShaker>();
            _shaker.m_continous = true;
            _shaker.m_continousDuration = 0.75f;
            _shaker.m_strength = 1f;
          }

          if (__instance.IsPlayer() && helm != null && helm.m_equiped)
          {
            var head = __instance.m_head;
            var position = head.position + head.up * 0.18f;
            var forward = head.right;
            var right = head.forward;

            var dir = _cam.transform.forward;
            var startPoint = position + forward * 0.4f;
            var endPoint = dir * 60 + _cam.transform.position;

            _leftEyeBeam.position = position + right * 0.06f;
            _rightEyeBeam.position = position - right * 0.06f;
            _leftEyeBeam.forward = _rightEyeBeam.forward = dir;

            bool hasDoneFlash = false;
            bool didDamage = false;
            foreach (var hit in Physics.RaycastAll(_cam.transform.position, dir, 50f))
            {
              var newEndpoint = hit.point;
              var newDir = (newEndpoint - position).normalized;
              _leftEyeBeam.forward = _rightEyeBeam.forward = newDir;

              if (!hasDoneFlash)
              {
                var goOne = GameObject.Instantiate(AssetHelper.EyeHitPrefab, hit.point + right * 0.06f - dir * 0.07f, Quaternion.identity);
                var goTwo = GameObject.Instantiate(AssetHelper.EyeHitPrefab, hit.point - right * 0.06f - dir * 0.07f, Quaternion.identity);
                goOne.SetActive(true);
                goTwo.SetActive(true);
                CoroutineExtensions.DelayedMethod(0.3f, () => { Object.Destroy(goOne); GameObject.Destroy(goTwo); });
                hasDoneFlash = true;
              }

              if (_hitEffectCooldown <= 0f)
              {
                var damageType = new HitData.DamageTypes
                {
                  m_damage = SurtlingHelm.BasePhysicalDamage.Value,
                  m_fire = SurtlingHelm.BaseLaserDamage.Value,
                  m_chop = SurtlingHelm.ChopDamage.Value,
                };
                var hitData = new HitData
                {
                  m_hitCollider = hit.collider,
                  m_attacker = __instance.GetZDOID(),
                  m_dir = dir,
                  m_point = hit.point,
                  m_skill = Skills.SkillType.FireMagic,
                  m_pushForce = SurtlingHelm.KnockbackForce.Value,
                  m_blockable = true,
                  m_dodgeable = false,
                  m_damage = damageType
                };

                var damagable = hit.collider.gameObject.GetComponent<IDestructible>();
                if (damagable == null) damagable = hit.collider.GetComponentInParent<IDestructible>();
                if (damagable == null) damagable = hit.collider.gameObject.GetComponentInChildren<IDestructible>();
                if (damagable != null)
                {
                  damagable.Damage(hitData);
                  didDamage = true;
                }
              }
            }
            if (didDamage) _hitEffectCooldown = SurtlingHelm.LaserHitInterval.Value;
          }
        }
        else if (_wasFiring || _leftEyeBeam != null)
        {
          _wasFiring = false;
          GameObject.Destroy(_leftEyeBeam?.gameObject);
          GameObject.Destroy(_rightEyeBeam?.gameObject);

          Object.Destroy(_shaker);
          _shaker = null;
        }
      }
    }
  }
}