using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UIExpansionKit;
using UIExpansionKit.API;
using UnityEngine;

namespace ProgramLauncher
{
    internal class Menu
    {
        internal static ICustomShowableLayoutedMenu menu = ExpansionKitApi.CreateCustomQuickMenuPage(LayoutDescription.QuickMenu4Columns);

        static Dictionary<string, Transform> MainButtons = new Dictionary<string, Transform>(), Programs = new Dictionary<string, Transform>();
        internal static bool runOnce_start;

        public static void Init() {
            ExpansionKitApi.GetExpandedMenu(ExpandedMenu.QuickMenu).AddSimpleButton("Program\nLauncher", () => {
                if (!runOnce_start) {
                    BuildMenu();
                    runOnce_start = true;
                    menu.Show();
                } else {
                    menu.Show();
                    try { UpdatePrograms(); } catch (Exception e) { Main.log.Error(e); }
                }
            });

        }

        private static void BuildMenu() {
            MainButtons.Clear();
            menu.AddSimpleButton("<color=red>Back</color>", () => menu.Hide(), g => MainButtons["BackBtn"] = g.transform);
            //menu.AddSimpleButton(string.Empty, () => Main.log.Msg("You're Cute~"), g => MainButtons["Cute1"] = g.transform);
            menu.AddSpacer();
            menu.AddSimpleButton("Add Program", () => {
                BuiltinUiUtils.ShowInputPopup("Add Program", "", UnityEngine.UI.InputField.InputType.Standard, false, "Add",
                    (ProgramName, ignore, ignore2) => {
                        BuiltinUiUtils.ShowInputPopup("Program Path", "", UnityEngine.UI.InputField.InputType.Standard, false, "Set",
                            (FilePath, ignoree, ignoree2) => {
                                SetPrograms.AddItem(ProgramName, FilePath.Replace("\\", "\\\\"));
                            }, null, "Program Path");
                    }, null, "Name of Program");
            }, g => MainButtons["AddProg"] = g.transform);
            menu.AddSimpleButton("Remove\nProgram(s)", () => Main.log.Msg("Temp Action"), g => MainButtons["Remove"] = g.transform);
            Main.log.Msg("Menu Built!");
        }

        private static void UpdatePrograms() {
            Programs.Clear();
            //foreach (var i in Programs) {
            //    UnityEngine.Object.Destroy(i.Value);
            //}
            int number = 0;
            
            foreach (var item in SetPrograms._prog.ListOfPrograms) {
                number++;

                menu.AddSimpleButton(item.Name, () => {
                    //Process.Start("cmd", $"/C START \"{item.FilePath}\"");
                    Process.Start(item.FilePath);
                }, g => Programs[$"Program_{number}"] = g.transform);
            }
        }
    }
}
