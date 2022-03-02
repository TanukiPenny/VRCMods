using Il2CppSystem.Collections.Generic;
using VRC.Core;
using VRC.DataModel.Core;
using VRC.UI.Elements.Menus;

namespace UserTagDumper
{
    public class Functions
    {
        private static SelectedUserMenuQM _selectedUserMenuQm;

        public static void LogTags()
        {
            var user = GetSelectedAPIUser();
            List<string> userid = user.tags;
            Main.log.Msg("-----------------------------");
            Main.log.Msg($"{user.displayName}'s tags");
            Main.log.Msg("-----------------------------");
            foreach (var i in userid)
            {
                Main.log.Msg(i);
            }
            Main.log.Msg("-----------------------------");
        }
        private static APIUser GetSelectedAPIUser()
        {
            if (_selectedUserMenuQm == null)
                _selectedUserMenuQm = UnityEngine.Object.FindObjectOfType<SelectedUserMenuQM>();
            if (_selectedUserMenuQm != null)
            {
                DataModel<APIUser> user = _selectedUserMenuQm.field_Private_IUser_0.Cast<DataModel<APIUser>>();
                return user.field_Protected_TYPE_0;
            }
            Main.log.Error("Unable to get SelectedUserMenuQM component!");
            return null;
        }
    }
}