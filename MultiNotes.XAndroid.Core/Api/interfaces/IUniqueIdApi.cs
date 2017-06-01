using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiNotes.XAndroid.Core.Api
{
    public interface IUniqueIdApi
    {
        Task<string> GetUniqueId();
    }
}