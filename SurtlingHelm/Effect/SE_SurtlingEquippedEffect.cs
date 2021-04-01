using UnityEngine;
using SurtlingHelm.Util;

namespace SurtlingHelm.Effect
{
  public class SE_SurtlingEquippedEffect : StatusEffect
  {
    private GameObject _eyeFireEffect;
    private Transform _transform;

    public override void Setup(Character character)
    {
      base.Setup(character);
      _eyeFireEffect = Instantiate(AssetHelper.EyeGlowPrefab);
      _eyeFireEffect.SetActive(true);
      _transform = _eyeFireEffect.transform;
      foreach (var p in _eyeFireEffect.GetComponentsInChildren<ParticleSystem>()) p.Play();
    }

    public override void UpdateStatusEffect(float dt)
    {
      base.UpdateStatusEffect(dt);
      var eye = m_character.m_head;
      _transform.position = eye.position;
      _transform.forward = eye.right;
      _transform.position -= eye.right * 0.165f;
      _transform.position += eye.up * 0.18f;
    }

    public override void Stop()
    {
      base.Stop();
      //_eyeFireEffect.GetComponent<ZNetView>().Destroy();
      Destroy(_eyeFireEffect);
    }
  }
}
