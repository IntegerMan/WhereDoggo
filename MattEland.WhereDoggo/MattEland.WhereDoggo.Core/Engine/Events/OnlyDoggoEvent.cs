﻿namespace MattEland.WhereDoggo.Core.Engine.Events;

public class OnlyDoggoEvent : GameEventBase
{
    public OnlyDoggoEvent(GamePlayer player) : base(player)
    {
        
    }

    public override string ToString()
    {
        return $"{Player} saw that they were the only doggo";
    }
}