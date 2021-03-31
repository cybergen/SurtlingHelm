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

    internal static void Init()
    {
      //AddCustomRecipe();
      AddCustomItem();
      Language.AddToken(TokenName, TokenValue);
      Language.AddToken(TokenDescriptionName, TokenDescriptionValue);
      Language.AddToken(EffectName, EffectValue);
      Language.AddToken(SurtlingTooltipName, SurtlingTooltipValue);
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

    private static void AddCustomItem()
    {
      var p = Prefab.Cache.GetPrefab<ItemDrop>("HelmetTrollLeather");
      //var go = GameObject.Find("HelmetTrollLeather");
      Debug.Log($"Got helmet troll prefab w/ parent {p?.gameObject?.transform.parent} and zdo {p?.gameObject?.GetComponent<ZNetView>()?.m_zdo}");
      //var CustomItem = new CustomItem(AssetHelper.HelmPrefab, true);
      //ObjectDBHelper.Add(CustomItem);
    }
  }
}