using SurtlingHelm.Util;
using System.Collections.Generic;
using UnityEngine;
using ValheimLib;
using ValheimLib.ODB;
using SurtlingHelm.Language;
using SurtlingHelm.Effect;

namespace SurtlingHelm.Item
{
  public static class ItemData
  {
    public const string CraftingStationPrefabName = "piece_workbench";

    internal static void Init()
    {
      AddCustomItems();
    }

    private static void AddCustomItems()
    {
      Debug.Log("adding custom items to object db");
      var mock = Mock<ItemDrop>.Create("HelmetTrollLeather");
      var cloned = Prefab.GetRealPrefabFromMock<ItemDrop>(mock).gameObject.InstantiateClone($"{LanguageData.TokenValue}", false);
      var helm = new CustomItem(cloned, fixReference: true);
      var item = helm.ItemDrop;
      //item.m_itemData.m_dropPrefab = helm.ItemPrefab;
      item.m_itemData.m_shared.m_name = LanguageData.TokenName;
      item.m_itemData.m_shared.m_description = LanguageData.TokenDescriptionName;
      //item.m_itemData.m_shared.m_icons = new Sprite[]{ AssetHelper.Icon };
      //item.m_itemData.m_shared.m_setName = string.Empty;
      //item.m_itemData.m_shared.m_setSize = 0;
      //item.m_itemData.m_shared.m_setStatusEffect = null;
      //item.m_itemData.m_shared.m_equipStatusEffect = null;// Prefab.Cache.GetPrefab<SE_SurtlingEquippedEffect>(LanguageData.EffectValue);
      //item.m_itemData.m_shared.m_backstabBonus = 1;
      //var meshRenderer = helm.ItemPrefab.transform.GetComponentInChildren<MeshRenderer>();
      //var mat = Object.Instantiate(meshRenderer.materials[0]);
      //mat.name = "surtling_helm_material";
      //mat.color = new Color(255, 0, 194, 255);
      //meshRenderer.materials[0] = mat;
      //var skinnedRenderer = helm.ItemPrefab.transform.Find("attach_skin/hood").GetComponent<SkinnedMeshRenderer>();
      //skinnedRenderer.materials[0] = mat;
      ObjectDBHelper.Add(helm);
      Prefab.NetworkRegister(AssetHelper.EyeGlowPrefab);
      Prefab.NetworkRegister(AssetHelper.EyeBeamPrefab);
      Prefab.NetworkRegister(AssetHelper.EyeBeamPrefab);
      //ObjectDBHelper.Add(new CustomItem(AssetHelper.EyeGlowPrefab, true));
      //ObjectDBHelper.Add(new CustomItem(AssetHelper.EyeBeamPrefab, true));
      //ObjectDBHelper.Add(new CustomItem(AssetHelper.EyeHitPrefab, true));

      Debug.Log("adding custom RECIPE to object db");
      var recipe = ScriptableObject.CreateInstance<Recipe>();
      recipe.name = "Recipe_SurtlingHelm";
      recipe.m_item = helm.ItemDrop;
      recipe.m_enabled = true;
      recipe.m_minStationLevel = 0;
      var neededResources = new List<Piece.Requirement>
      {
        MockRequirement.Create("Wood", 1),
      };
      recipe.m_resources = neededResources.ToArray();
      var helmRecipe = new CustomRecipe(recipe, true, true);
      ObjectDBHelper.Add(helmRecipe);
    }
  }
}