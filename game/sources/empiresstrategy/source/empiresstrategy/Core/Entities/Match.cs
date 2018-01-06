using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Map.Data;

namespace Core.Entities
{
    public class Match
    {

        #region enums

        public enum MatchType { None, Developement, SinglePlayer, MultiPlayer };
        public enum MatchNetworkType { None, Online, Offline };

        #endregion

        public static MatchType matchType;
        public MatchNetworkType matchNetworkType;

        private MapData _mapData;
        public MapData mapData
        {
            get
            {
                if (_mapData == null) _mapData = new MapData();
                return _mapData;
            }
            set
            {
                _mapData = value;
            }
        }


    }
}
