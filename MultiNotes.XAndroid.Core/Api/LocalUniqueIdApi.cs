using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MultiNotes.XAndroid.Core.Api;

namespace MultiNotes.XAndroid.Core
{
    internal class LocalUniqueIdApi : IUniqueIdApi
    {
        public static readonly string LocalModifier = "::Local::";

        public string GetUniqueId()
        {
            return GetUniqueIdImpl();
        }

        private string GetUniqueIdImpl()
        {
            int currentId;
            if (!System.IO.File.Exists(Constants.LocalIdFile))
            {
                currentId = 0;
            }
            else
            {
                try
                {
                    currentId = int.Parse(System.IO.File.ReadAllText(Constants.LocalIdFile));
                }
                catch
                {
                    currentId = 0;
                }
            }
            System.IO.File.WriteAllText(Constants.LocalIdFile, (++currentId).ToString());
            return LocalModifier + currentId.ToString();
        }
    }
}
