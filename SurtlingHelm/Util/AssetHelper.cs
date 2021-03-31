using System.Linq;
using System.Reflection;
using UnityEngine;
using ValheimLib;
using ValheimLib.ODB;

namespace SurtlingHelm.Util
{
  public static class AssetHelper
  {
    public const string AssetBundleName = "surtlinghelm";
    public static AssetBundle SurtlingAssetBundle;

    public const string HelmPrefabPath = "assets/effects/surtlinghelm.prefab";
    public static GameObject HelmPrefab;

    public const string EyeGlowPrefabPath = "assets/effects/eyeflames/eyefire.prefab";
    public static GameObject EyeGlowPrefab
    {
      get
      {
        if (_eyeGlowGameObject == null)
        {
          _eyeGlowGameObject = SurtlingAssetBundle.LoadAsset<GameObject>(EyeGlowPrefabPath);
          _eyeGlowGameObject.transform.SetParent(null);
        }
        return _eyeGlowGameObject;
      }
    }
    private static GameObject _eyeGlowGameObject;

    public const string EyeBeamPrefabPath = "assets/effects/eyebeam/eyebeam.prefab";
    public static GameObject EyeBeamPrefab
    {
      get
      {
        if (_eyeBeamGameObject == null)
        {
          _eyeBeamGameObject = SurtlingAssetBundle.LoadAsset<GameObject>(EyeBeamPrefabPath);
          _eyeBeamGameObject.transform.SetParent(null);
        }
        return _eyeBeamGameObject;
      }
    }
    private static GameObject _eyeBeamGameObject;

    public const string EyeHitEffectPrefabPath = "assets/effects/eyehit/eyehit.prefab";
    public static GameObject EyeHitPrefab
    {
      get
      {
        if (_eyeHitGameObject == null)
        {
          _eyeHitGameObject = SurtlingAssetBundle.LoadAsset<GameObject>(EyeHitEffectPrefabPath);
          _eyeHitGameObject.transform.SetParent(null);
        }
        return _eyeHitGameObject;
      }
    }
    private static GameObject _eyeHitGameObject;

    public const string IconPath = "assets/icons/icon";
    public static Sprite Icon
    {
      get
      {
        if (_icon == null)
        {
          _icon = SurtlingAssetBundle.LoadAsset<Sprite>(IconPath);
        }
        return _icon;
      }
    }
    private static Sprite _icon;

    public static void Init()
    {
      SurtlingAssetBundle = GetAssetBundleFromResources(AssetBundleName);
      var p = Prefab.Cache.GetPrefab<ItemDrop>("HelmetTrollLeather");
      HelmPrefab = GameObject.Instantiate(p.gameObject);
      HelmPrefab.SetActive(false);
      HelmPrefab.name = "SurtlingHelm";
      var meshRenderer = HelmPrefab.transform.GetComponentInChildren<MeshRenderer>();
      var mat = Object.Instantiate(meshRenderer.materials[0]);
      mat.color = new Color(255, 0, 194, 255);
      meshRenderer.materials[0] = mat;
      var skinnedRenderer = HelmPrefab.transform.GetComponentInChildren<SkinnedMeshRenderer>();
      skinnedRenderer.materials[0] = mat;
    }

    public static AssetBundle GetAssetBundleFromResources(string fileName)
    {
      var execAssembly = Assembly.GetExecutingAssembly();
      var resourceName = execAssembly.GetManifestResourceNames().Single(str => str.EndsWith(fileName));
      using (var stream = execAssembly.GetManifestResourceStream(resourceName))
      {
        return AssetBundle.LoadFromStream(stream);
      }
    }
  }
}