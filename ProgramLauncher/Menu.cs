using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UIExpansionKit;
using UIExpansionKit.API;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace ProgramLauncher {
    internal class Menu {
        internal static ICustomShowableLayoutedMenu menu = ExpansionKitApi.CreateCustomQuickMenuPage(LayoutDescription.QuickMenu4Columns),
            RemoveMenu = ExpansionKitApi.CreateCustomQuickMenuPage(LayoutDescription.QuickMenu4Columns);

        private static Dictionary<string, Transform> MainButtons = new Dictionary<string, Transform>(),
            MainRemoveButtons = new Dictionary<string, Transform>(),
            Programs = new Dictionary<string, Transform>(),
            RemovePrograms = new Dictionary<string, Transform>();

        private static Dictionary<string, GameObject> ProgList, RemoveProgList;

        internal static bool runOnce_start;

        public static void Init() {
            ExpansionKitApi.GetExpandedMenu(ExpandedMenu.QuickMenu).AddSimpleButton("Program\nLauncher", () => {
                if (!runOnce_start) {
                    BuildMenu();
                    BuildRemoveMenu();
                    runOnce_start = true;
                    menu.Show();
                } else {
                    menu.Show();
                }
            });
        }

        private static void BuildMenu() {
            MainButtons.Clear();
            menu.AddSimpleButton("<color=red>Back</color>", () => menu.Hide(), g => MainButtons["BackBtn"] = g.transform);

            menu.AddSpacer();

            menu.AddSimpleButton("Add Program", () => {
                BuiltinUiUtils.ShowInputPopup("Add Program", "", InputField.InputType.Standard, false, "Add",
                    (ProgramName, ignore, ignore2) => {

                        BuiltinUiUtils.ShowInputPopup("Program Path", "", InputField.InputType.Standard, false, "Set",
                            (FilePath, ignoree, ignoree2) => {
                                SetPrograms.AddItem(ProgramName, FilePath.Replace("\\", "\\"));

                                menu.AddSimpleButton(ProgramName, () => Process.Start(FilePath), g => {
                                    ProgList[$"Program_{ProgramName}"] = g;
                                    Programs[$"Program_{ProgramName}"] = g.transform;
                                    g.name = $"Program_{ProgramName}";
                                });

                                RemoveMenu.AddSimpleButton($"<color=red>{ProgramName}</color>", () => {
                                    SetPrograms.RemoveItem(ProgramName);

                                    foreach (var g in RemoveProgList) {
                                        if (g.Key == ProgramName) {
                                            //Object.DestroyImmediate();
                                            g.Value.SetActive(false);
                                        }
                                    }

                                    RemovePrograms.Remove($"Program_{ProgramName}");
                                    RemoveProgList.Remove($"Program_{ProgramName}");
                                }, g => {
                                    RemoveProgList[$"Program_{ProgramName}"] = g;
                                    RemovePrograms[$"Program_{ProgramName}"] = g.transform;
                                    g.name = $"Program_{ProgramName}";
                                });

                                #region Knah, This is the complete defination of jank, but it works for what issues we ran into
                                // Yell at Lily for this Jank
                                BuiltinUiUtils.ShowInputPopup("HEY!", "", InputField.InputType.Standard, false, "CUTIE",
                                    (ok, youre, cute) => {
                                        return;
                                    }, null, "You\'re Cute");

                                #endregion

                            }, null, "Enter the path to the program EXE", false);

                    }, null, "Name of Program", false);
            }, g => MainButtons["AddProg"] = g.transform);

            menu.AddSimpleButton("Remove\nProgram(s)", () => {
                menu.Hide();
                RemoveMenu.Show();
            }, g => MainButtons["Remove"] = g.transform);

            Programs.Clear();
            ProgList = new Dictionary<string, GameObject>();
            foreach (var item in SetPrograms._prog.ListOfPrograms) {
                menu.AddSimpleButton(item.Name, () => {
                    Process.Start(item.FilePath);
                }, g => {
                    ProgList[$"Program_{item.Name}"] = g;
                    Programs[$"Program_{item.Name}"] = g.transform;
                    g.name = $"Program_{item.Name}";
                });
            }
            Main.log.Msg("Menu Built!");
        }

        private static void BuildRemoveMenu() {
            RemoveMenu.AddSimpleButton("<color=yellow>Back</color>", () => { RemoveMenu.Hide(); menu.Show(); }, g => MainRemoveButtons["BackBtn"] = g.transform);
            RemoveMenu.AddSpacer();
            RemoveMenu.AddSpacer();
            RemoveMenu.AddSpacer();

            RemoveProgList = new Dictionary<string, GameObject>();

            foreach (var item in SetPrograms._prog.ListOfPrograms) {
                RemoveMenu.AddSimpleButton($"<color=red>{item.Name}</color>", () => {
                    SetPrograms.RemoveItem(item.Name);
                    RemovePrograms.Remove($"Program_{item.Name}");

                    foreach (var g in RemoveProgList) {
                        if (g.Key == item.Name) {
                            //Object.DestroyImmediate();
                            g.Value.SetActive(false);
                        }
                    }

                    RemoveProgList.Remove($"Program_{item.Name}");
                }, g => {
                    RemoveProgList[$"Program_{item.Name}"] = g;
                    RemovePrograms[$"Program_{item.Name}"] = g.transform;
                    g.name = $"Program_{item.Name}";
                });
            }
        }
    }
}
