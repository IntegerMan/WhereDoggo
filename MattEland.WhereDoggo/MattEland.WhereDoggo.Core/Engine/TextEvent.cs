using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MattEland.WhereDoggo.Core.Engine
{
    /// <summary>
    /// This is a simple event just used for debugging purposes. It should have no impact on AI decisions.
    /// </summary>
    public class TextEvent : GameEventBase
    {
        public string Message { get; }

        public TextEvent(string message) : base()
        {
            Message = message;
        }

        public override string ToString() => Message;
    }
}
