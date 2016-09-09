using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericCore.Support
{
    public sealed class KeysManager
    {
        public static KeysManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new KeysManager();
                }
                return _instance;
            }
        }

        public static int NextKey
        {
            get
            {
                return Instance.GetKey();
            }
        }

        private int _lastKey;
        private static KeysManager _instance = null;

        private KeysManager()
        {
            _lastKey = 0;
        }

        public int GetKey()
        {
            return ++_lastKey;
        }
    }
}
