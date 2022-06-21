using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace OldMate
{
    public static class NicknameManager
    {

        public class Nickname
        {
            public string UserId { get; set; }

            public string OriginalName { get; set; }

            public string ModifiedName { get; set; }
        }

        public static List<Nickname> nicknames = new List<Nickname>();

        public static void UpdateNickname(Nickname nickname)
        {
            RemoveNickname(nickname.UserId);
            nicknames.Add(nickname);
            SaveNicknames();
        }

        public static void RemoveNickname(string UserId)
        {
            nicknames.RemoveAll(n => n.UserId == UserId);
            SaveNicknames();
        }

        public static bool Contains(string UserId)
        {
            return nicknames.Where(n => n.UserId == UserId).Count() > 0;
        }

        public static string GetModifiedName(string UserId)
        {
            return nicknames.Where(n => n.UserId == UserId).First().ModifiedName;
        }

        public static string GetOriginalName(string UserId)
        {
            return nicknames.Where(n => n.UserId == UserId).First().OriginalName;
        }

        public static void LoadNicknames()
        {
            if (File.Exists(Path.Combine(Environment.CurrentDirectory, "UserData/OldMate.json")))
            {
                nicknames = JsonConvert.DeserializeObject<List<Nickname>>(File.ReadAllText(Path.Combine(Environment.CurrentDirectory, "UserData/OldMate.json")));
            }
        }

        public static void SaveNicknames()
        {
            File.WriteAllText(Path.Combine(Environment.CurrentDirectory, "UserData/OldMate.json"), JsonConvert.SerializeObject(nicknames));
        }

    }
}