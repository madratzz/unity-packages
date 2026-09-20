using System;
using System.Collections.Generic;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace CustomEditorUtilities
{
    // AdvancedDropdown — the same searchable tree widget behind Unity's
    // "Add Component" menu — listing every project asset of assetType.
    internal class SearchableAssetDropdown : AdvancedDropdown
    {
        private readonly Type _assetType;
        private readonly Action<Object> _onSelect;
        private readonly Dictionary<int, Object> _itemsById = new Dictionary<int, Object>();

        public SearchableAssetDropdown(AdvancedDropdownState state, Type assetType, Action<Object> onSelect)
            : base(state)
        {
            _assetType = assetType;
            _onSelect = onSelect;
            minimumSize = new Vector2(200, 250);
        }

        protected override AdvancedDropdownItem BuildRoot()
        {
            var root = new AdvancedDropdownItem(_assetType.Name);
            _itemsById.Clear();

            var none = new AdvancedDropdownItem("None") { id = 0 };
            root.AddChild(none);
            _itemsById[0] = null;

            int nextId = 1;
            foreach (Object asset in SearchableAssetFinder.FindAssets(_assetType))
            {
                var item = new AdvancedDropdownItem(asset.name) { id = nextId };
                _itemsById[nextId] = asset;
                root.AddChild(item);
                nextId++;
            }

            return root;
        }

        protected override void ItemSelected(AdvancedDropdownItem item)
        {
            if (_itemsById.TryGetValue(item.id, out Object asset))
                _onSelect(asset);
        }
    }
}
