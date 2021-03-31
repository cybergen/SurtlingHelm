using UnityEngine;
using SurtlingHelm.Util;

namespace SurtlingHelm.Effect
{
  public class SE_SurtlingEquippedEffect : StatusEffect
  {
    private GameObject _eyeFireEffect;
    private Transform _transform;

    public void Awake()
    {
      _eyeFireEffect = Instantiate(AssetHelper.EyeGlowPrefab);
      _eyeFireEffect.SetActive(true);
      _transform = _eyeFireEffect.transform;
      foreach (var p in _eyeFireEffect.GetComponentsInChildren<ParticleSystem>()) p.Play();
      Debug.Log("Claling awake on surtling effect");
    }

    public override void UpdateStatusEffect(float dt)
    {
      base.UpdateStatusEffect(dt);

      var eye = m_character.m_head;
      _transform.position = eye.position;
      _transform.forward = eye.right;
      _transform.position -= eye.right * 0.1f;
      _transform.position += eye.up * 0.18f;
    }

    public override void OnDestroy()
    {
      base.OnDestroy();
      Debug.Log("Calling destroy on surtling equipped effect");
    }
  }
}
