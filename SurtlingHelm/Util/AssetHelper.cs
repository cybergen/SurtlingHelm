using System.Linq;
using System.Reflection;
using UnityEngine;

namespace SurtlingHelm.Util
{
  public static class AssetHelper
  {
    public const string AssetBundleName = "surtlinghelm";
    public static AssetBundle SurtlingAssetBundle;

    public const string EyeGlowPrefabPath = "assets/effects/eyeflames/eyefire.prefab";
    public static GameObject EyeGlowPrefab
    {
      get
      {
        if (_eyeGlowGameObject == null)
        {
          _eyeGlowGameObject = SurtlingAssetBundle.LoadAsset<GameObject>(EyeGlowPrefabPath);
        }
        return _eyeGlowGameObject;
      }
    }
    private static GameObject _eyeGlowGameObject;

    public const string EyeBeamPrefabPath = "assets/effects/eyebeam/eyelaser.prefab";
    public static GameObject EyeBeamPrefab
    {
      get
      {
        if (_eyeBeamGameObject == null)
        {
          _eyeBeamGameObject = SurtlingAssetBundle.LoadAsset<GameObject>(EyeBeamPrefabPath);
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
        }
        return _eyeHitGameObject;
      }
    }
    private static GameObject _eyeHitGameObject;

    public const string IconPath = "assets/icons/icon.png";
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