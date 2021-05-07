using ControlzEx.Standard;
using MyRecipes.Core.Recipes;
using MyRecipes.ViewModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRecipes.Core.SeasonCalendar
{
    public class Season : ViewModelBase
    {
        private SeasonMonth mSeasonMonthBegin;
        private SeasonMonth mSeasonMonthEnd;
        private WareOriginType mOriginType;
        private bool mWholeYear;

        private Dictionary<SeasonMonth, WareOriginType> mActiveSeasons = new Dictionary<SeasonMonth, WareOriginType>();

        public SeasonMonth SeasonMonthBegin
        {
            get => mSeasonMonthBegin;
            set
            {
                mSeasonMonthBegin = value;
                InvokePropertyChanged();
                InvokePropertyChanged("ActiveSeasons");
            }
        }

        public SeasonMonth SeasonMonthEnd
        {
            get => mSeasonMonthEnd;
            set
            {
                mSeasonMonthEnd = value;
                InvokePropertyChanged();
                InvokePropertyChanged("ActiveSeasons");
            }
        }

        public WareOriginType OriginType
        {
            get => mOriginType;
            set
            {
                mOriginType = value;
                InvokePropertyChanged();
            }
        }

        public bool WholeYear
        {
            get => mWholeYear;
            set
            {
                mWholeYear = value;
                InvokePropertyChanged();
            }
        }

        [JsonIgnore]
        public Dictionary<SeasonMonth, WareOriginType> ActiveSeasons
        {
            get => mActiveSeasons;
            set
            {
                mActiveSeasons = value;
                InvokePropertyChanged();
            }
        }

        [JsonConstructor]
        public Season(SeasonMonth seasonMonthBegin, SeasonMonth seasonMonthEnd, WareOriginType originType, bool wholeYear)
        {
            SeasonMonthBegin = seasonMonthBegin;
            SeasonMonthEnd = seasonMonthEnd;
            OriginType = originType;
            WholeYear = wholeYear;

            RefreshActiveSeasons();
        }

        public Season() : this(SeasonMonth.January, SeasonMonth.March, WareOriginType.Unset, false)
        {

        }

        public void RefreshActiveSeasons()
        {
            mActiveSeasons.Clear();
            if (WholeYear)
            {
                for (int i = 0; i <= 11; i++)
                {
                    mActiveSeasons.Add((SeasonMonth)i, OriginType);
                }
            }
            else
            {
                if (SeasonMonthBegin == SeasonMonthEnd) // Mark all months as active
                {
                    mActiveSeasons.Add(SeasonMonthBegin, OriginType);
                }
                else if (SeasonMonthBegin < SeasonMonthEnd) // Mark months as active, beginning from start month to end month
                {
                    for (int i = (int)SeasonMonthBegin; i <= (int)SeasonMonthEnd; i++)
                    {
                        mActiveSeasons.Add((SeasonMonth)i, OriginType);
                    }
                }
                else // Mark months as active, beginning from january to end month and start month to december
                {
                    for (int i = 0; i <= (int)SeasonMonthEnd; i++)
                    {
                        mActiveSeasons.Add((SeasonMonth)i, OriginType);
                    }
                    for (int i = (int)SeasonMonthBegin; i <= 11; i++)
                    {
                        mActiveSeasons.Add((SeasonMonth)i, OriginType);
                    }
                }
            }

            InvokePropertyChanged("ActiveSeasons");
        }
    }
}
