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

    public const string CraftingStationPrefabName = "piece_workbench";

    internal static void Init()
    {
      AddCustomRecipe();
      AddCustomItem();

      Language.AddToken(TokenName, TokenValue);
      Language.AddToken(TokenDescriptionName, TokenDescriptionValue);
    }

    private static void AddCustomRecipe()
    {
      var recipe = ScriptableObject.CreateInstance<Recipe>();
      recipe.m_item = AssetHelper.HelmPrefab.GetComponent<ItemDrop>();
      var neededResources = new List<Piece.Requirement>
      {
        MockRequirement.Create("SurtlingCore", 5),
        MockRequirement.Create("TrollHide", 2),
      };
      recipe.m_resources = neededResources.ToArray();
      var CustomRecipe = new CustomRecipe(recipe, false, true);
      ObjectDBHelper.Add(CustomRecipe);
    }

    private static void AddCustomItem()
    {
      var CustomItem = new CustomItem(AssetHelper.HelmPrefab, true);
      ObjectDBHelper.Add(CustomItem);
    }
  }
}