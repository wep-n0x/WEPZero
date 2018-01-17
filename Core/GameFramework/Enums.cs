namespace GameFramework
{
    public enum Channels : sbyte
    {
        None = 0,
        CQC = 1,
        Urban_Ops = 2,
        Battle_Group = 3,
        AI_Channel = 4
    }

    public enum Classes : byte
    {
        Engineer = 0,
        Medic,
        Sniper,
        Assault,
        Heavy,
        COUNT
    }

    public enum Mode : byte
    {
        Explosive = 0,
        Free_For_All,
        Team_Death_Match
    }

    public enum Premium : byte
    {
        Free2Play = 0,
        Bronze,
        Silver,
        Gold,
        Platinum
    }

    public enum RoomState : byte
    {
        Waiting = 1,
        Playing
    }

    public enum Team : byte
    {
        Derbaran = 0,
        NIU = 1,
        None = 2
    }
}
