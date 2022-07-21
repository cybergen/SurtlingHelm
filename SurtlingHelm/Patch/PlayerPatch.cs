using HarmonyLib;
using SurtlingHelm.Util;
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
    internal class PlayerUpdatePatch
    {
        private static bool _wasFiring;
        private static Transform _leftEyeBeam;
        private static Transform _rightEyeBeam;
        private static float _hitEffectCooldown;
        private static Camera _cam;
        private static CamShaker _shaker;
        private static float _laserTimeRemaining;

        private static void Postfix(Player __instance, ref Attack ___m_currentAttack, ref float ___m_lastCombatTimer, Rigidbody ___m_body, ZSyncAnimation ___m_zanim,
          CharacterAnimEvent ___m_animEvent, VisEquipment ___m_visEquipment, Attack ___m_previousAttack, float ___m_timeSinceLastAttack)
        {
            if (!Player.m_localPlayer)
            {
                return;
            }

            _hitEffectCooldown -= Time.deltaTime;

            var helm = __instance.GetInventory().GetAllItems().
              FirstOrDefault(v => v.m_shared.m_name == "$custom_item_laserhelm");

            if (_cam == null) _cam = Camera.main;

            if (helm != null && helm.m_equiped)
            {
                var firing = false;
                var firePressed = ZInput.GetButton(SurtlingHelm.LaserFireButton.Name);
                if (firePressed && (!SurtlingHelm.ConsumeCoresAsFuel.Value || _laserTimeRemaining > 0f))
                {
                    firing = true;
                    _laserTimeRemaining -= Time.deltaTime;
                }
                else if (firePressed)
                {
                    //Look for surtling core in inventory and consume if have, otherwise show error message
                    var cores = __instance.GetInventory().GetAllItems().FirstOrDefault(i => i.m_shared.m_name == "$item_surtlingcore");
                    if (cores == null || cores.m_stack == 0)
                    {
                        MessageHud.instance.ShowMessage(MessageHud.MessageType.Center, Language.LanguageData.NeedResourcesErrorName);
                    }
                    else
                    {
                        __instance.GetInventory().RemoveOneItem(cores);
                        _laserTimeRemaining = SurtlingHelm.SecondsOfUsageGrantedPerCore.Value;
                        firing = true;
                    }
                }

                if (firing)
                {
                    if (!_wasFiring || _leftEyeBeam == null)
                    {
                        _wasFiring = true;
                        var leftGO = Object.Instantiate(AssetHelper.EyeBeamPrefab);
                        _leftEyeBeam = leftGO.transform;

                        var rightGO = Object.Instantiate(AssetHelper.EyeBeamPrefab);
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
                        var distToPlayer = Vector3.Distance(_cam.transform.position, __instance.transform.position);
                        foreach (var hit in Physics.RaycastAll(_cam.transform.position + dir * distToPlayer, dir, 50f))
                        {
                            var newEndpoint = hit.point;
                            var newDir = (newEndpoint - position).normalized;
                            _leftEyeBeam.forward = _rightEyeBeam.forward = newDir;

                            if (!hasDoneFlash)
                            {
                                var goOne = Object.Instantiate(AssetHelper.EyeHitPrefab, hit.point + right * 0.06f - dir * 0.07f, Quaternion.identity);
                                var goTwo = Object.Instantiate(AssetHelper.EyeHitPrefab, hit.point - right * 0.06f - dir * 0.07f, Quaternion.identity);
                                goOne.SetActive(true);
                                goTwo.SetActive(true);
                                hasDoneFlash = true;
                            }

                            if (_hitEffectCooldown <= 0f)
                            {
                                var damageType = new HitData.DamageTypes
                                {
                                    m_damage = SurtlingHelm.BasePhysicalDamage.Value,
                                    m_fire = SurtlingHelm.BaseLaserDamage.Value,
                                    m_chop = SurtlingHelm.ChopDamage.Value,
                                    m_pickaxe = SurtlingHelm.BasePhysicalDamage.Value,
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
                    ZNetScene.instance.m_instances.Remove(_leftEyeBeam.GetComponent<ZNetView>().GetZDO());
                    ZNetScene.instance.m_instances.Remove(_rightEyeBeam.GetComponent<ZNetView>().GetZDO());
                    _leftEyeBeam.GetComponent<ZNetView>().Destroy();
                    _rightEyeBeam.GetComponent<ZNetView>().Destroy();
                    _leftEyeBeam = null;
                    _rightEyeBeam = null;
                    Object.Destroy(_shaker);
                    _shaker = null;
                }
            }
        }
    }
}
