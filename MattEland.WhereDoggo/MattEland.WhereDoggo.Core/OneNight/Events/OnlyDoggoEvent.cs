﻿namespace MattEland.WhereDoggo.Core.OneNight.Events;

public class OnlyDoggoEvent : GameEventBase
{
    public OnlyDoggoEvent(GamePlayer player) 
        : base(GamePhase.Night, player)
    {
        
    }

    public override string ToString()
    {
        return $"{Player} saw that they were the only doggo";
    }
}