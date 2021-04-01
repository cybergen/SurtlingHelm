namespace SurtlingHelm.Language
{
  public class LanguageData
  {
    public const string TokenName = "$custom_item_laserhelm";
    public const string TokenValue = "SurtlingHelm";

    public const string TokenDescriptionName = "$custom_item_laserhelm_description";
    public const string TokenDescriptionValue = "A helm that imbues you with an ominous strength";

    public const string EffectName = "$se_surtlingeffect_name";
    public const string EffectValue = "SurtlingHelm";

    public const string SurtlingTooltipName = "$se_surtlingeffect_tooltip";
    public const string SurtlingTooltipValue = "Your eyes glow with ominous force";

    public static void Init()
    {
      ValheimLib.Language.AddToken(TokenName, TokenValue, true);
      ValheimLib.Language.AddToken(TokenDescriptionName, TokenDescriptionValue, true);
      ValheimLib.Language.AddToken(EffectName, EffectValue, true);
      ValheimLib.Language.AddToken(SurtlingTooltipName, SurtlingTooltipValue, true);
    }
  }
}
