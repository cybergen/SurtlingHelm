using BepInEx;
using BepInEx.Configuration;
using System;
using UnityEngine;
using ValheimLib.ODB;
using SurtlingHelm.Effect;
using SurtlingHelm.Util;
using SurtlingHelm.Item;
using SurtlingHelm.Patch;

namespace SurtlingHelm
{
  [BepInPlugin(ModGuid, ModName, ModVer)]
  [BepInProcess("valheim.exe")]
  [BepInDependency("ValheimModdingTeam.ValheimLib", BepInDependency.DependencyFlags.HardDependency)]
  public class SurtlingHelm : BaseUnityPlugin
  {
    public const string ModGuid = AuthorName + "." + ModName;
    public static ConfigEntry<string> LaserFireKey;
    public static ConfigEntry<float> BaseLaserDamage;
    public static ConfigEntry<float> LaserHitInterval;
    public static ConfigEntry<float> BasePhysicalDamage;
    public static ConfigEntry<float> KnockbackForce;
    public static ConfigEntry<float> ChopDamage;
    public static KeyCode SurtlingFireKey;

    private const string AuthorName = "cybergen";
    private const string ModName = "SurtlingHelm";
    private const string ModVer = "0.0.2";

    internal static SurtlingHelm Instance { get; private set; }

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private async void Awake()
    {
      Instance = this;
      Log.Init(Logger);
      InitConfigData();
      Language.LanguageData.Init();
      ObjectDBHelper.OnBeforeCustomItemsAdded += AssetHelper.Init;
      ObjectDBHelper.OnBeforeCustomItemsAdded += InitStatusEffects;
      ObjectDBHelper.OnBeforeCustomItemsAdded += ItemData.Init;
      ObjectDBHelper.OnAfterInit += PlayerPatch.Init;
    }

    /// <summary>
    /// Destroying the attached Behaviour will result in the game or Scene receiving OnDestroy.
    /// OnDestroy occurs when a Scene or game ends.
    /// It is also called when your mod is unloaded, this is where you do clean up of hooks, harmony patches,
    /// loose GameObjects and loose monobehaviours.
    /// Loose here refers to gameobjects not attached
    /// to the parent BepIn GameObject where your BaseUnityPlugin is attached
    /// </summary>
    private void OnDestroy()
    {
      PlayerPatch.Disable();
    }

    private void InitConfigData()
    {
      LaserFireKey = Config.Bind("General", "LaserFireKey", "G", "Laser Fire Hotkey");
      BaseLaserDamage = Config.Bind("General", "BaseLaserDamage", 20f, "Base Laser Damage");
      LaserHitInterval = Config.Bind("General", "LaserHitInterval", 0.1f, "Frequency to apply laser hits and hit effects");
      BasePhysicalDamage = Config.Bind("General", "BasePhysicalDamage", 10f, "The Physical damage the laser does");
      KnockbackForce = Config.Bind("General", "KnockbackForce", 45f, "The amount of knockback done by the laser");
      ChopDamage = Config.Bind("General", "ChopDamage", 20f, "The amount of chop done by the laser");
      if (!Enum.TryParse<KeyCode>(LaserFireKey.Value, out var laserFireKeyCode))
      {
        Debug.Log("Failed to parse hotkey for laser fire");
        return;
      }
      SurtlingFireKey = laserFireKeyCode;
    }

    private void InitStatusEffects()
    {
      Debug.Log("Initializing status effects");
      var effect = ScriptableObject.CreateInstance<SE_SurtlingEquippedEffect>();
      effect.m_icon = AssetHelper.Icon;
      effect.m_name = Language.LanguageData.EffectValue;
      effect.name = Language.LanguageData.EffectValue;
      effect.m_tooltip = Language.LanguageData.SurtlingTooltipName;
      ObjectDBHelper.Add(new CustomStatusEffect(effect, true));
    }
  }
}