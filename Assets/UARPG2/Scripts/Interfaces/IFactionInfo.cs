public interface IFactionInfo
{
    public int faction { get; set; }
    public bool Conflict(ICharacter other);
}