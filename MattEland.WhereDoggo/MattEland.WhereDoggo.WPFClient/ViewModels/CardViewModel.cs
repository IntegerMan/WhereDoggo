using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

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
        public string Role => card.CurrentCard.ToString();
        public Teams Team => card.CurrentCard.Team;
        public Brush TeamForeground => Team == Teams.Villagers ? Brushes.Blue : Brushes.Red;

        public override string ToString() => CardName;
    }
}
