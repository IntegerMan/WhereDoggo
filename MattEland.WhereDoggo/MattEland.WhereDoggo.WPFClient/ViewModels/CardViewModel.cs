using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MattEland.WhereDoggo.WPFClient.ViewModels
{
    public class CardViewModel : ViewModelBase
    {
        private readonly IHasCard card;

        public CardViewModel(IHasCard card) 
        {
            this.card = card;
        }

        public CardViewModel() : this(new CenterCardSlot("Placeholder", new VillagerRole()))
        {
        }

        public string CardName => card.Name;

        public override string ToString() => CardName;
    }
}
