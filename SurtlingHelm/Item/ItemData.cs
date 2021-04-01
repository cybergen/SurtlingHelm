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
      //Create a clone of existing game asset helmet troll leather for our Surtling Helm
      var mock = Mock<ItemDrop>.Create("HelmetTrollLeather");
      var cloned = Prefab.GetRealPrefabFromMock<ItemDrop>(mock).gameObject.InstantiateClone($"{LanguageData.TokenValue}", true);
      cloned.name = LanguageData.TokenValue;

      //Set the new data on the ItemDrop component of our new helm
      var newItemPrefab = cloned;
      var helm = new CustomItem(cloned, fixReference: true);
      var item = helm.ItemDrop;
      item.m_itemData.m_dropPrefab = newItemPrefab;
      item.m_itemData.m_shared.m_name = LanguageData.TokenName;
      item.m_itemData.m_shared.m_description = LanguageData.TokenDescriptionName;
      item.m_itemData.m_shared.m_icons = new Sprite[] { AssetHelper.Icon };
      item.m_itemData.m_shared.m_setName = string.Empty;
      item.m_itemData.m_shared.m_setSize = 0;
      item.m_itemData.m_shared.m_setStatusEffect = null;
      item.m_itemData.m_shared.m_equipStatusEffect = Prefab.Cache.GetPrefab<SE_SurtlingEquippedEffect>(LanguageData.EffectValue);
      item.m_itemData.m_shared.m_backstabBonus = 1;

      //Tweak the material to make the helmet purple
      var meshRenderer = newItemPrefab.transform.GetComponentInChildren<MeshRenderer>();
      var colorTarget = new Color(255f / 255f, 0f, 194f / 255f, 255f / 255f);
      meshRenderer.material.color = colorTarget;
      var skinnedRenderer = newItemPrefab.transform.Find("attach_skin/hood").GetComponent<SkinnedMeshRenderer>();
      skinnedRenderer.material.color = colorTarget;

      //Add to the object db, along with our new network-synced prefabs
      ObjectDBHelper.Add(helm);
      Prefab.NetworkRegister(AssetHelper.EyeGlowPrefab);
      Prefab.NetworkRegister(AssetHelper.EyeBeamPrefab);
      Prefab.NetworkRegister(AssetHelper.EyeHitPrefab);

      //Add our recipe to the object db so our item can be crafted
      var recipe = ScriptableObject.CreateInstance<Recipe>();
      recipe.name = "Recipe_SurtlingHelm";
      recipe.m_item = helm.ItemDrop;
      recipe.m_enabled = true;
      recipe.m_minStationLevel = 0;
      var neededResources = new List<Piece.Requirement>
      {
        MockRequirement.Create("SurtlingCore", 10),
        MockRequirement.Create("TrollHide", 5)
      };
      recipe.m_resources = neededResources.ToArray();
      var helmRecipe = new CustomRecipe(recipe, true, true);
      ObjectDBHelper.Add(helmRecipe);
    }
  }
}