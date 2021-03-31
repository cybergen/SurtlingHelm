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
      AddCustomRecipe();
      AddCustomItems();
      Language.AddToken(TokenName, TokenValue);
      Language.AddToken(TokenDescriptionName, TokenDescriptionValue);
      Language.AddToken(EffectName, EffectValue);
      Language.AddToken(SurtlingTooltipName, SurtlingTooltipValue);
    }

    private static void SetupHelm(StatusEffect helmEffect)
    {
      var go = AssetHelper.HelmPrefab;
      var item = go.GetComponent<ItemDrop>();
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
      recipe.m_item = AssetHelper.HelmPrefab.GetComponent<ItemDrop>();
      var neededResources = new List<Piece.Requirement>
      {
        MockRequirement.Create("SurtlingCore", 10),
        MockRequirement.Create("TrollHide", 5),
      };
      recipe.m_resources = neededResources.ToArray();
      var CustomRecipe = new CustomRecipe(recipe, false, true);
      ObjectDBHelper.Add(CustomRecipe);
    }

    private static void AddCustomItems()
    {
      var CustomItem = new CustomItem(AssetHelper.HelmPrefab, true);
      ObjectDBHelper.Add(CustomItem);
      ObjectDBHelper.Add(new CustomItem(AssetHelper.EyeGlowPrefab, false));
      ObjectDBHelper.Add(new CustomItem(AssetHelper.EyeBeamPrefab, false));
      ObjectDBHelper.Add(new CustomItem(AssetHelper.EyeHitPrefab, false));
    }
  }
}