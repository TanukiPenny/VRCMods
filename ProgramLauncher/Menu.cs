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
                BuildMenu();
                /*
                if (!runOnce_start) {
                    // Build
                    runOnce_start = true;
                    menu.Show();
                } else {
                    menu.Show();
                    UpdatePrograms(); // ??????
                }
                */
            });

            Main.log.Msg("Menu Built!");
        }

        private static void BuildMenu() {
            MainButtons.Clear();
            menu.AddSimpleButton("<color=red>Back</color>", () => menu.Hide(), g => MainButtons["BackBtn"] = g.transform);
            menu.AddSimpleButton(string.Empty, () => Main.log.Msg("You're Cute~"), g => MainButtons["Cute1"] = g.transform);
            menu.AddSimpleButton(string.Empty, () => Main.log.Msg("You're Cute~"), g => MainButtons["Cute2"] = g.transform);
            menu.AddSimpleButton("Add Program", () => {

                UIExpansionKit.API.BuiltinUiUtils.ShowInputPopup("Add Program", "", UnityEngine.UI.InputField.InputType.Standard, false, "Add", 
                    (ProgramName, __, ___) => {
                    // Action Code
                    UIExpansionKit.API.BuiltinUiUtils.ShowInputPopup("Program Path", "", UnityEngine.UI.InputField.InputType.Standard, false, "Set", 
                        (FilePath, yy, yyy) => {
                            // Action Code
                            SetPrograms.AddItem(ProgramName, FilePath);
                        }, null, "Program Path");
                }, null, "Name of Program");

            }, g => MainButtons["AddProg"] = g.transform);
            
            UpdatePrograms();
        }

        private static void UpdatePrograms() {
            Programs.Clear();
            int number = 0;
            
            foreach (var item in SetPrograms._prog.ListOfPrograms) {
                number++;

                menu.AddSimpleButton(item.Name, () => {
                    Process.Start("cmd", $"/C start \"{item.FilePath}\"");
                }, g => MainButtons[$"Program_{number}"] = g.transform);
            }
        }
    }
}
