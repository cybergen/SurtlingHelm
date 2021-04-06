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
    public const string ModGuid = ModName;
    public static ConfigEntry<string> LaserFireKey;
    public static ConfigEntry<float> BaseLaserDamage;
    public static ConfigEntry<float> LaserHitInterval;
    public static ConfigEntry<float> BasePhysicalDamage;
    public static ConfigEntry<float> KnockbackForce;
    public static ConfigEntry<float> ChopDamage;

    public static ConfigEntry<int> SurtlingRequired;
    public static ConfigEntry<int> TrollHideRequired;
    public static ConfigEntry<int> LinenThreadRequired;
    public static ConfigEntry<int> SurtlingTrophyRequired;
    public static ConfigEntry<int> WorkbenchLevelRequired;

    public static ConfigEntry<bool> ConsumeCoresAsFuel;
    public static ConfigEntry<float> SecondsOfUsageGrantedPerCore;

    public static KeyCode SurtlingFireKey;

    private const string AuthorName = "cybergen";
    private const string ModName = "SurtlingHelm";
    private const string ModVer = "0.1.1";

    internal static SurtlingHelm Instance { get; private set; }

    private void Awake()
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

      SurtlingRequired = Config.Bind("General", "SurtlingRequired", 15, "The amount of Surtling Cores required to craft");
      TrollHideRequired = Config.Bind("General", "TrollHideRequired", 4, "The amount of Troll Hide required to craft");
      LinenThreadRequired = Config.Bind("General", "LinenThreadRequired", 10, "The amount of Linen Thread required to craft");
      SurtlingTrophyRequired = Config.Bind("General", "SurtlingTrophyRequired", 3, "The amount of Surtling Trophies required to craft");
      WorkbenchLevelRequired = Config.Bind("General", "WorkbenchLevelRequired", 5, "The level of workbench required to craft");

      ConsumeCoresAsFuel = Config.Bind("General", "ConsumeSurtlingCoresAsFuel", true, "Whether using the laser should consume Surtling Cores");
      SecondsOfUsageGrantedPerCore = Config.Bind("General", "SecondsOfUsagePerCore", 5f, "How many seconds of laser usage to grant per consumed core");

      if (!Enum.TryParse<KeyCode>(LaserFireKey.Value, out var laserFireKeyCode))
      {
        Debug.Log("Failed to parse hotkey for laser fire");
        return;
      }
      SurtlingFireKey = laserFireKeyCode;
    }

    private void InitStatusEffects()
    {
      var effect = ScriptableObject.CreateInstance<SE_SurtlingEquippedEffect>();
      effect.m_icon = AssetHelper.Icon;
      effect.m_name = Language.LanguageData.EffectValue;
      effect.name = Language.LanguageData.EffectValue;
      effect.m_tooltip = Language.LanguageData.SurtlingTooltipName;
      ObjectDBHelper.Add(new CustomStatusEffect(effect, true));
    }
  }
}