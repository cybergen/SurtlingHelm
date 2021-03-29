using System.Linq;
using System.Reflection;
using UnityEngine;

namespace SurtlingHelm.Util
{
  public static class AssetHelper
  {
    public const string AssetBundleName = "surtlinghelm";
    public static AssetBundle SurtlingAssetBundle;

    public const string HelmPrefabPath = "Assets/CustomItems/SurtlingHelm.prefab";
    public static GameObject HelmPrefab;

    public const string EyeGlowPrefabPath = "Assets/CustomItems/SurtlingHelmEyeCenter.prefab";
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

    public const string EyeBeamPrefabPath = "Assets/CustomItems/EyeBeam.prefab";
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

    public const string EyeHitEffectPrefabPath = "Assets/CustomItems/EyeHit.prefab";
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

    public static void Init()
    {
      SurtlingAssetBundle = GetAssetBundleFromResources(AssetBundleName);
      HelmPrefab = SurtlingAssetBundle.LoadAsset<GameObject>(HelmPrefabPath);
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