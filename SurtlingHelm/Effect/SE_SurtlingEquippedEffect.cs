using System;
using System.Collections.Generic;
using UnityEngine;
using SurtlingHelm.Util;

namespace SurtlingHelm.Effect
{
  public class SE_SurtlingEquippedEffect : StatusEffect
  {
    private static GameObject _eyeFireEffect;
    private static Transform _transform;

    public override void Setup(Character character)
    {
      base.Setup(character);
      SpawnFlames();
    }

    public override void UpdateStatusEffect(float dt)
    {
      base.UpdateStatusEffect(dt);

      if (!SurtlingHelm.UseEyeTrailEffects.Value) return;

      if (!m_character.IsTeleporting() && _eyeFireEffect == null)
      {
        SpawnFlames();
      }
      else if (!m_character.IsTeleporting() && _transform != null)
      {
        var target = GetTargetPosition(m_character);
        _transform.position = target.Item1;
        _transform.forward = target.Item2;
      }
    }

    public override void Stop()
    {
      base.Stop();
      ZNetScene.instance.m_instances.Remove(_eyeFireEffect.GetComponent<ZNetView>().GetZDO());
      _eyeFireEffect.GetComponent<ZNetView>().Destroy();
      _eyeFireEffect = null;
      _transform = null;
    }

    private void SpawnFlames()
    {
      if (!SurtlingHelm.UseEyeTrailEffects.Value) return;

      var target = GetTargetPosition(m_character);
      _eyeFireEffect = Instantiate(AssetHelper.EyeGlowPrefab, target.Item1, Quaternion.identity);
      _transform = _eyeFireEffect.transform;
      _transform.forward = target.Item2;
      foreach (var p in _eyeFireEffect.GetComponentsInChildren<ParticleSystem>()) p.Play();
    }

    private Tuple<Vector3, Vector3> GetTargetPosition(Character c)
    {
      var head = c.m_head;
      var position = head.position;
      var forward = head.right;
      position -= head.right * 0.165f;
      position += head.up * 0.19f;
      return new Tuple<Vector3, Vector3>(position, forward);
    }
  }
}
