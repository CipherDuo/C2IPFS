using CipherDuo.IPFS.Constants;
using Ipfs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CipherDuo.IPFS.Logger;

namespace CipherDuo.IPFS.Utility
{
    public static class IPFSUtility
    {
        public static Dictionary<IPFSNetworks, string> IPFSNetworkProvider = new Dictionary<IPFSNetworks, string>()
        {
                {IPFSNetworks.CipherDuo, "http://ipfs.cipherduo.org:5001"},
                {IPFSNetworks.Infura, "https://ipfs.infura.io:5001"},
                {IPFSNetworks.Node, "http://93.44.160.142:45005"},
                {IPFSNetworks.RaspPi, "http://192.168.1.163:5001"},
                {IPFSNetworks.CustomIPFS, "http://127.0.0.1:5001"}
        };

        public const string IPFSSharedKeyName = "CipherDuo_SharedKey";

        public static IpfsKey IPFSSharedKey = new IpfsKey
        {
            name = "CipherDuo_SharedKey",
            keyPassword = "jeF4fDx7zGycTUJPK7XXuMXNYVDCA2e4".ToCharArray(),
            aesPassword = "tQ2ejftWBgD5Q9LQFPEK2BtPFx3Czqav"
        };

        public static string IPFSRelayTCP = "/ip4/157.90.158.135/tcp/4001/p2p/12D3KooWQGfKt3o2tBA2TzpCi4kVd2oq12etXv7NdZLQgRZjoej1";
        public static string IPFSRaspPiTCP = "/ip4/93.44.160.142/tcp/4001/p2p/12D3KooWFi7B47iJQHdKpQrSQZ1ekXmFAdes1EroJrxcpHUeYHs3";
        
        public static string IPFSRelayUDP = "/ip4/157.90.158.135/udp/51602/quic/p2p/12D3KooWCDGVaMNcrSFgq4LRwuJCknisZvpQGpFuec153Cg1nzC1";

        private static ILog logger = LoggerFactory.GetLogger(nameof(IPFSUtility));
        public static Cid CalculateHash(byte[] digest)
        {
            MultiHash hash = new MultiHash("sha2-256", digest);
            Cid cid = new Cid()
            {
                Encoding = "base32",
                ContentType = "dag-cbor",
                Version = 1,
                Hash = hash
            };

            return cid;
        }
        public static async Task<bool> PinAsync(string cid)
        {
            try
            {

                await IPFS.ipfsRelay.Pin.AddAsync(cid);
                await IPFS.ipfsRelay.Dht.ProvideAsync(cid);
                return true;

            }
            catch (Exception error)
            {
                logger.Log(error.Message);
            }

            return false;
        }

    }
}