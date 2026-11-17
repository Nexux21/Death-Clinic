using System;

public enum TargetType
{
     Troops,
        
     Building,

        All
    
    

    }

public enum SpeedType
{
    Slow,

    Normal,

    Fast
}

    [Flags]
    public enum SpecialMofierType
    {
        none = 0,

        InovokeEveryTick = 1 << 0,

        InvokeOnDeath = 1 << 2,

        ExplotionOnDeath = 1 << 3,


    }


