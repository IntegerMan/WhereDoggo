using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MattEland.WhereDoggo.Core.Events
{
    public class WokeUpEvent : GameEventBase
    {
        public WokeUpEvent(GamePhase phase, GamePlayer player) : base(phase, player)
        {
        }

        /// <inheritdoc />
        public override string ToString() => Phase == GamePhase.Day 
                ? $"{Player} woke up in the morning." 
                : $"{Player} woke up in the {Phase}.";
    }
}
