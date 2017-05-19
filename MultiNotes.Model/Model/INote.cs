using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiNotes.Core
{ 
    public interface INote
    {
        string Id { get; }
        string OwnerId { get; }
        string Content { get; }
        DateTime CreateTimestamp { get; }
        DateTime LastChangeTimestamp { get; }
    }
}
