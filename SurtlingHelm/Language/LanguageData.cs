using Jotunn.Entities;
using Jotunn.Managers;
using System.Collections.Generic;

namespace SurtlingHelm.Language
{
    public class LanguageData
    {

        public const string TokenName = "$custom_item_laserhelm";
        public const string TokenValue = "SurtlingHelm";

        public const string TokenDescriptionName = "$custom_item_laserhelm_description";
        public const string TokenDescriptionValue = "A helm that imbues you with an ominous strength";

        public const string EffectName = "$se_surtlingeffect_name";
        public const string EffectValue = "Ooooooh, pewpewpew";

        public const string SurtlingTooltipName = "$se_surtlingeffect_tooltip";
        public const string SurtlingTooltipValue = "Your eyes glow with ominous force";

        public const string NeedResourcesErrorName = "$surtling_helm_error";
        public const string NeedResourcesErrorValue = "Need Surtling Cores to use helm powers";
        
        public static CustomLocalization Localization = new CustomLocalization();

        public static void Init()
        {
            LocalizationManager.Instance.AddLocalization(Localization);
            Localization.AddTranslation("English", new Dictionary<string, string>
            {
                { TokenName, TokenValue },
                { TokenDescriptionName, TokenDescriptionValue },
                { EffectName, EffectValue },
                { NeedResourcesErrorName, NeedResourcesErrorValue },
                { SurtlingTooltipName, SurtlingTooltipValue }
            });
        }
    }
}
