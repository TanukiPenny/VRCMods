using System.Diagnostics;
using UnityEngine;
using VRC.UI.Elements.Menus;
using VRC.Core;
using VRC.DataModel.Core;

namespace URLTools;

public class UserFunc
{
    private static SelectedUserMenuQM _selectedUserMenuQM;
    public static void CopyUserURL()
    {
        var user = GetSelectedAPIUser();
        if (user != null)
        {
            string userid = user.id;
            GUIUtility.systemCopyBuffer = $"https://vrchat.com/home/user/{userid}";
            Main.log.Msg($"User URL copied");
        }
                
    }
    public static void OpenUserPage()
    {
        var user = GetSelectedAPIUser();
        if (user != null)
        {
            string userid = user.id;
            Process.Start($"https://vrchat.com/home/user/{userid}");
            Main.log.Msg($"User page opened");
        }
    }
    public static APIUser GetSelectedAPIUser()
    {
        if (_selectedUserMenuQM == null)
            _selectedUserMenuQM = UnityEngine.Object.FindObjectOfType<SelectedUserMenuQM>();
        if (_selectedUserMenuQM != null)
        {
            DataModel<APIUser> user = _selectedUserMenuQM.field_Private_IUser_0.Cast<DataModel<APIUser>>();
            return user.field_Protected_TYPE_0;
        }
        Main.log.Error("Unable to get SelectedUserMenuQM component!");
        return null;
    }
}