﻿using Demo.OrganizationalStructure.Client.WPF.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace Demo.OrganizationalStructure.Client.WPF.AddonFeatures.SimpleHierarchy
{
    public class SimpleHierarchyVM
    {
        private readonly OrganizationalStructureVM _organizationalStructureVM;
        private List<CompositeJobRoleVM> _composites = new List<CompositeJobRoleVM>();
        private List<CompositeLeafEmployeeVM> _compositeLeafs = new List<CompositeLeafEmployeeVM>();

        internal event Action<ICompositeItem> SelectedCompositeItemChanged;

        internal SimpleHierarchyVM(OrganizationalStructureVM organizationalStructureVM)
        {
            _organizationalStructureVM = organizationalStructureVM;

            _organizationalStructureVM.JobRoles.CollectionChanged += (s, e) => RecreateHirarchy();
            _organizationalStructureVM.Employees.CollectionChanged += (s, e) => RecreateHirarchy();

            _organizationalStructureVM.PropertyChanged += SyncWithSelectedItem;
        }

        internal bool ShouldDoAutoRefresh { get => !_organizationalStructureVM.IsLoadingNewOrganisation; }
        public ObservableCollection<ICompositeItem> HirarchyItems { get; } = new ObservableCollection<ICompositeItem>();

        internal void SelectItem(ICompositeItem compositeItem)
        {
            EditableItemBaseVM realSelectedItem = null;

            if (compositeItem is CompositeJobRoleVM compositeJobRole)
            {
                realSelectedItem = compositeJobRole.JobRoleVM;
            }
            else if (compositeItem is CompositeLeafEmployeeVM compositeLeafEmployee)
            {
                realSelectedItem = compositeLeafEmployee.EmployeeVM;
            }

            _organizationalStructureVM.SelectedItem = realSelectedItem;
        }

        internal void RecreateHirarchy()
        {
            if (ShouldDoAutoRefresh == false) { return; }

            HirarchyItems.Clear();

            foreach (var item in _composites) { item.HierarchicalChange -= OnHierarchicalChangeRecreateHirarchy; }
            foreach (var item in _compositeLeafs) { item.HierarchicalChange -= OnHierarchicalChangeRecreateHirarchy; }

            _composites = _organizationalStructureVM.JobRoles.Select(
                x =>
                {
                    var item = new CompositeJobRoleVM(x);
                    item.HierarchicalChange += OnHierarchicalChangeRecreateHirarchy;
                    return item;
                }).ToList();

            _compositeLeafs = _organizationalStructureVM.Employees.Select(
                x =>
                {
                    var item = new CompositeLeafEmployeeVM(x);
                    item.HierarchicalChange += OnHierarchicalChangeRecreateHirarchy;
                    return item;
                }).ToList();

            foreach (var compositeItem in _compositeLeafs) { AddItemToHirarchy(compositeItem, _composites); }
            foreach (var compositeItem in _composites) { AddItemToHirarchy(compositeItem, _composites); }
        }

        private void OnHierarchicalChangeRecreateHirarchy()
        {
            RecreateHirarchy();
        }

        private void AddItemToHirarchy(
            ICompositeItem compositeItem,
            IEnumerable<IComposite> composites)
        {
            var parentComposite = composites.FirstOrDefault(x => x.Key == compositeItem.ParentKey);
            if (parentComposite != null)
            {
                parentComposite.CompositeItems.Add(compositeItem);
            }
            else
            {//Note: no parent has been found - meaning it belongs to root level
                HirarchyItems.Add(compositeItem);
            }
        }

        private void SyncWithSelectedItem(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(OrganizationalStructureVM.SelectedItem))
            {
                var selectedItem = _organizationalStructureVM.SelectedItem;
                var appropriateCompositeItem = FindCompositeItem(selectedItem);
                SelectedCompositeItemChanged?.Invoke(appropriateCompositeItem);
            }
        }

        private ICompositeItem FindCompositeItem(EditableItemBaseVM selectedItem)
        {
            return FindCompositeItem(selectedItem, HirarchyItems);
        }

        private ICompositeItem FindCompositeItem(
            EditableItemBaseVM searchEditableItem,
            IEnumerable<ICompositeItem> compositeItems)
        {
            foreach (var item in compositeItems)
            {
                if (item is CompositeLeafEmployeeVM compositeLeafEmployee &&
                    compositeLeafEmployee.EmployeeVM == searchEditableItem)
                {
                    return compositeLeafEmployee;
                }
                else if (item is CompositeJobRoleVM compositeJobRole)
                {
                    if (compositeJobRole.JobRoleVM == searchEditableItem)
                    {
                        return compositeJobRole;
                    }
                    else if (FindCompositeItem(searchEditableItem, compositeJobRole.CompositeItems) is ICompositeItem positiveResult)
                    {
                        return positiveResult;
                    }
                }
            }
            return null;
        }
    }
}
