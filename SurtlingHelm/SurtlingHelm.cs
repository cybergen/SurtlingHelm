using BepInEx;
using BepInEx.Configuration;
using Jotunn.Configs;
using Jotunn.Entities;
using Jotunn.Managers;
using Jotunn.Utils;
using SurtlingHelm.Effect;
using SurtlingHelm.Item;
using SurtlingHelm.Language;
using SurtlingHelm.Patch;
using SurtlingHelm.Util;
using UnityEngine;

namespace SurtlingHelm
{
    [BepInPlugin(ModGuid, ModName, ModVer)]
    [BepInDependency(Jotunn.Main.ModGuid)]
    [NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.Minor)]
    public class SurtlingHelm : BaseUnityPlugin
    {
        public const string ModGuid = ModName;
        public static ConfigEntry<KeyboardShortcut> LaserFireKey;
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

        public static ConfigEntry<bool> UseEyeTrailEffects;
        public static ConfigEntry<bool> UseLaserSound;

        public static ButtonConfig LaserFireButton;

        private const string AuthorName = "cybergen";
        private const string ModName = "SurtlingHelm";
        private const string ModVer = "0.1.2";

        internal static SurtlingHelm Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
            InitConfigData();
            LanguageData.Init();
            PlayerPatch.Init();

            PrefabManager.OnVanillaPrefabsAvailable += AssetHelper.Init;
            PrefabManager.OnVanillaPrefabsAvailable += InitStatusEffects;
            PrefabManager.OnVanillaPrefabsAvailable += ItemData.Init;
        }

        private void OnDestroy()
        {
            PlayerPatch.Disable();
        }

        private void InitConfigData()
        {
            Config.SaveOnConfigSet = true;

            LaserFireKey = Config.Bind("General", "LaserFireKey", new KeyboardShortcut(KeyCode.G), "Laser Fire Hotkey");
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

            UseEyeTrailEffects = Config.Bind("General", "UseEyeTrailEffects", true, "Whether to generate eye trail effects while helm is equipped");
            UseLaserSound = Config.Bind("General", "UseLaserSound", true, "Whether to use the lazer sound");

            LaserFireButton = new ButtonConfig
            {
                Name = "LaserFireKey",
                ShortcutConfig = LaserFireKey
            };
            InputManager.Instance.AddButton(ModGuid, LaserFireButton);
        }

        private void InitStatusEffects()
        {
            var effect = ScriptableObject.CreateInstance<SE_SurtlingEquippedEffect>();
            effect.m_icon = AssetHelper.Icon;
            effect.m_name = LanguageData.EffectValue;
            effect.name = LanguageData.EffectValue;
            effect.m_tooltip = LanguageData.SurtlingTooltipName;
            ItemManager.Instance.AddStatusEffect(new CustomStatusEffect(effect, true));

            PrefabManager.OnVanillaPrefabsAvailable -= InitStatusEffects;
        }
    }
}