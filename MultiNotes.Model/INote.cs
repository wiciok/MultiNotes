using System;

namespace MultiNotes.Model
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
