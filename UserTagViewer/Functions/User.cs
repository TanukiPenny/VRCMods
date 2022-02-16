using UnityEngine;
using VRC.UI.Elements.Menus;
using VRC.Core;
using VRC.DataModel.Core;


namespace UserTagViewer
{
    public class User
    {
        private static SelectedUserMenuQM _selectedUserMenuQM;
        public static APIUser GetSelectedAPIUser()
        {
            if (_selectedUserMenuQM == null)
                _selectedUserMenuQM = Object.FindObjectOfType<SelectedUserMenuQM>();
            if (_selectedUserMenuQM != null)
            {
                DataModel<APIUser> user = _selectedUserMenuQM.field_Private_IUser_0.Cast<DataModel<APIUser>>();
                return user.field_Protected_TYPE_0;
            }
            Main.log.Error("Unable to get SelectedUserMenuQM component!");
            return null;
        }
        public static void GetUserTags()
        {
            var user = GetSelectedAPIUser();
            APIUser _user = user;
            Main.log.Msg(user); 
            /*foreach(var tag in user._tags._items)
            {
                Main.log.Msg(user._tags._items.ToString()
            }*/
        }
    }
}
