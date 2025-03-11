public interface IResistanceManager
{
    public float Resist(int type, float baseValue);
    public float ResistAllAndSum(Damage dmg);   
}

