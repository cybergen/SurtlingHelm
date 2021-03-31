using SurtlingHelm.Util;
using System.Collections.Generic;
using UnityEngine;
using ValheimLib;
using ValheimLib.ODB;

namespace SurtlingHelm.Item
{
  public static class ItemData
  {
    public const string TokenName = "$custom_item_laserhelm";
    public const string TokenValue = "Surtling Helm";

    public const string TokenDescriptionName = "$custom_item_laserhelm_description";
    public const string TokenDescriptionValue = "A helm that imbues you with an ominous strength";

    public const string EffectName = "$se_surtlingeffect_name";
    public const string EffectValue = "Surtling Helm";

    public const string SurtlingTooltipName = "$se_surtlingeffect_tooltip";
    public const string SurtlingTooltipValue = "Your eyes glow with ominous force";

    public const string CraftingStationPrefabName = "piece_workbench";

    internal static void Init(StatusEffect helmEffect)
    {
      SetupHelm(helmEffect);
      AddCustomItems();
      AddCustomRecipe();
      Language.AddToken(TokenName, TokenValue, false);
      Language.AddToken(TokenDescriptionName, TokenDescriptionValue, false);
      Language.AddToken(EffectName, EffectValue, false);
      Language.AddToken(SurtlingTooltipName, SurtlingTooltipValue, false);
    }

    private static void SetupHelm(StatusEffect helmEffect)
    {
      var helm = AssetHelper.Helm;
      var item = helm.ItemDrop;
      item.m_itemData.m_shared.m_name = Item.ItemData.TokenName;
      item.m_itemData.m_shared.m_description = Item.ItemData.TokenDescriptionName;
      item.m_itemData.m_shared.m_setName = string.Empty;
      item.m_itemData.m_shared.m_setSize = 0;
      item.m_itemData.m_shared.m_setStatusEffect = null;
      item.m_itemData.m_shared.m_equipStatusEffect = helmEffect;
      item.m_itemData.m_shared.m_backstabBonus = 0;
    }

    private static void AddCustomRecipe()
    {
      var recipe = ScriptableObject.CreateInstance<Recipe>();
      recipe.m_item = AssetHelper.Helm.ItemDrop;
      var neededResources = new List<Piece.Requirement>
      {
        MockRequirement.Create("SurtlingCore", 10),
        MockRequirement.Create("TrollHide", 5),
      };
      recipe.m_resources = neededResources.ToArray();
      var CustomRecipe = new CustomRecipe(recipe, true, true);
      ObjectDBHelper.Add(CustomRecipe);
    }

    private static void AddCustomItems()
    {
      ObjectDBHelper.Add(AssetHelper.Helm);
      ObjectDBHelper.Add(new CustomItem(AssetHelper.EyeGlowPrefab, true));
      ObjectDBHelper.Add(new CustomItem(AssetHelper.EyeBeamPrefab, true));
      ObjectDBHelper.Add(new CustomItem(AssetHelper.EyeHitPrefab, true));
    }
  }
}