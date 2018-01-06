using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Entities;

namespace Core.PlayerData
{
    public class PlayerDataService
    {

        #region Properties

        private Account _account;
        public Account myAccount
        {
            get
            {
                if (_account == null) _account = new Account();
                return _account;
            }
            set
            {
                _account = value;
            }
        }

        private Settings _settings;
        public Settings mySettings
        {
            get
            {
                if (_settings == null) _settings = new Settings();
                return _settings;
            }
            set
            {
                _settings = value;
            }
        }

        #endregion



    }
}
