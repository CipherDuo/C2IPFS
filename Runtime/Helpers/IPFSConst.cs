using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Threading;

namespace CipherDuo.IPFS.Constants
{
    public enum IPFSNetworks
    {
        CipherDuo,
        Infura,
        Node,
        RaspPi,
        CustomIPFS
    };

    public struct IpfsKey
    {
        public string name;
        public char[] keyPassword;
        public string aesPassword;
        public string hash;
    }
}