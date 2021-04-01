using System;
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
      var target = GetTargetPosition(character);
      _eyeFireEffect = Instantiate(AssetHelper.EyeGlowPrefab, target.Item1, Quaternion.identity);
      _transform = _eyeFireEffect.transform;
      _transform.forward = target.Item2;
      foreach (var p in _eyeFireEffect.GetComponentsInChildren<ParticleSystem>()) p.Play();
    }

    public override void UpdateStatusEffect(float dt)
    {
      base.UpdateStatusEffect(dt);
      var target = GetTargetPosition(m_character);
      if (_transform != null) _transform.position = target.Item1;
      if (_transform != null) _transform.forward = target.Item2;
    }

    public override void Stop()
    {
      base.Stop();
      Destroy(_eyeFireEffect);
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
