using MessengerApp.Domain.Enumerations;

namespace MessengerApp.Domain.Constants;

public static class Reactions
{
    public static readonly Dictionary<int, string> Dictionary = new()
    {
        {(int)Reaction.ThumbUp, "👍"},
        {(int)Reaction.ThumbDown, "👎"}
    };
}