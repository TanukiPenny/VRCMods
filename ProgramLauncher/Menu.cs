using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;
using MelonLoader;
using UIExpansionKit;
using UIExpansionKit.API;
using UnityEngine;
using UnityEngine.UI;
using VRC.UI.Core;
using Object = UnityEngine.Object;
using ReMod.Core;
using ReMod.Core.UI.QuickMenu;
using ReMod.Core.VRChat;

namespace ProgramLauncher {
    public class Menu
    {
        public static IEnumerator OnQuickMenu()
        {
            while (UIManager.prop_UIManager_0 == null) yield return null;
            while (Object.FindObjectOfType<VRC.UI.Elements.QuickMenu>() == null) yield return null;
            BuildTab();
            BuildLauncher();
            RemoveMenu();
            UpdatePrograms();

        }

        private static ReCategoryPage _plTab;
        private static ReMenuCategory _plMenu, _plLauncher, c;
        private static ReCategoryPage _removePage;
        private static List<ReMenuButton> pButtonsl = new List<ReMenuButton>();
        private static ReMenuButton _pButton;
        private static List<ReMenuButton> pButtonslRemove = new List<ReMenuButton>();
        private static ReMenuButton _pButtonRemove;
        
        private static void BuildTab()
        {
            _plTab = new ReCategoryPage("Program Launcher", true);
            ReTabButton.Create("Program Launcher", "Open Program Launcher", "Program Launcher", BundleManager.Plaunch);
            _plMenu = _plTab.AddCategory("Menu");
            _plMenu.AddButton("Add Program", "Adds program to your program launcher.", () =>
            {
                VRCUiPopupManager.prop_VRCUiPopupManager_0.ShowInputPopupWithCancel("Enter name of program", "",
                    InputField.InputType.Standard, false, "Set Name", (programName, ignore, ignore2) =>
                    {
                        MelonCoroutines.Start(PromptDelayed(programName.Replace("\"","")));
                    }, () => VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.HideCurrentPopup());
            }, BundleManager.Plus);
            _removePage = _plMenu.AddCategoryPage("Remove Program", "Removes program from your program launcher.",
                BundleManager.Minus);
        }
        private static void BuildLauncher()
        {
            _plLauncher = _plTab.AddCategory("Launcher");
        }
        static IEnumerator PromptDelayed(string programName)
        {
            yield return new WaitForSeconds(1f);
            VRCUiPopupManager.prop_VRCUiPopupManager_0.ShowInputPopupWithCancel("Enter program path", "",
                InputField.InputType.Standard, false, "Set Path",
                (filePath, ignoree, ignoree2) =>
                {
                    SetPrograms.AddItem(programName, filePath.Replace("/", "\\"));
                    _pButton = _plLauncher.AddButton(programName, $"Open {programName}", () => Process.Start(filePath),
                        BundleManager.Launch);
                    pButtonsl.Add(_pButton);
                    _pButtonRemove = c.AddButton($"<color=red>{programName}</color>", $"Remove {programName}", () =>
                    {
                        var b = pButtonsl.FirstOrDefault(x => x.Name.Contains(programName));
                        if (b != null && b.Name.Contains(programName))
                        {
                            Object.DestroyImmediate(b.GameObject);
                            SetPrograms.RemoveItem(programName);
                            pButtonsl.Remove(_pButton);
                            var br = pButtonslRemove.FirstOrDefault(x => x.Name.Contains(programName));
                            if (br != null && br.Name.Contains(programName))
                            {
                                Object.DestroyImmediate(br.GameObject);
                            }
                            pButtonslRemove.Remove(b);
                        }
                    }, BundleManager.LaunchRed);
                    pButtonslRemove.Add(_pButtonRemove);
                }, () => VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.HideCurrentPopup());
        }
        private static void UpdatePrograms()
        {
            foreach (var p in SetPrograms.Prog.ListOfPrograms)
            {
                _pButton = _plLauncher.AddButton(p.Name, $"Open {p.Name}", () => Process.Start(p.FilePath),
                    BundleManager.Launch);
                pButtonsl.Add(_pButton);
            }
        }
        private static void RemoveMenu()
        {
            c = _removePage.AddCategory("Remove Programs", false);
            foreach (var p in SetPrograms.Prog.ListOfPrograms)
            {
                _pButtonRemove = c.AddButton($"<color=red>{p.Name}</color>", $"Remove {p.Name}", () =>
                {
                    var b = pButtonsl.FirstOrDefault(x => x.Name.Contains(p.Name));
                    if (b != null && b.Name.Contains(p.Name))
                    {
                        Object.DestroyImmediate(b.GameObject);
                        SetPrograms.RemoveItem(p.Name);
                        pButtonslRemove.Remove(b);
                        var br = pButtonslRemove.FirstOrDefault(x => x.Name.Contains(p.Name));
                        if (br != null && br.Name.Contains(p.Name))
                        {
                            Object.DestroyImmediate(br.GameObject);
                        }
                    }
                }, BundleManager.LaunchRed);
                pButtonslRemove.Add(_pButtonRemove);
            }
        }
    }
}
