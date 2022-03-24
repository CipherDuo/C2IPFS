using Ipfs;
using Ipfs.CoreApi;
using Ipfs.Http;
using System;
using System.IO;
using System.Linq;
using System.Security;
using System.Threading.Tasks;
using CipherDuo.IPFS.Constants;
using CipherDuo.IPFS.Logger;
using CipherDuo.IPFS.Utility;

using static CipherDuo.IPFS.Utility.IPFSUtility;

namespace CipherDuo.IPFS
{

    public static class IPFS
    {
        public static IpfsClient ipfsRelay { get; private set; }
        private static ILog logger = LoggerFactory.GetLogger(nameof(IPFS));
        public static void Start(string dirName, IPFSNetworks network)
        {
            Start(dirName, IPFSNetworkProvider[network]);
        }

        public static void Start(string dirName, string nodeUrl = null)
        {
            StartIPFS(nodeUrl);

        }

        static void StartIPFS(string nodeUrl = null)
        {
            try
            {
                ipfsRelay = new IpfsClient(nodeUrl);
                
                //TODO distinguish based on nodeURL which should connect
                ipfsRelay.Bootstrap.AddAsync(IPFSUtility.IPFSRelayTCP);
                ipfsRelay.Bootstrap.AddAsync(IPFSUtility.IPFSRaspPiTCP);
                logger.Log("Starting IPFS . . .");
            }
            catch (Exception err)
            {
                logger.Log(err.Message);
            }
        }

        // public async void ConnectToLocal()
        // {
        //     var peer = await GetRelayPeer();
        //     await ipfsRelay.Swarm.ConnectAsync(peer.ConnectedAddress);
        // }

        // void SetConfiguration(string dirName, string nodeUrl)
        // {   
        //
        //     string secret = "THIS PASSWORD SHOULD BE SEEDED WITH CSPRNG & SAVED LOCALLY IN AES";
        //     SecureString passphrase = new SecureString();
        //     foreach (var character in secret) passphrase.AppendChar(character);
        //
        //     // creates a folder for the engine
        //     string basePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        //     string path = Path.Combine(basePath, dirName);
        //
        //     // sets various static environment variables
        //     Environment.SetEnvironmentVariable("IPFS_PASS", passphrase.ToString());
        //     Environment.SetEnvironmentVariable("IPFS_PATH", path);
        //
        // }

        public static void Stop()
        {
            ipfsRelay = null;
        }
        

        public static async Task<Peer> GetRelayPeer()
        {
            return await Task.Run(async () => { var peerID = await ipfsRelay.IdAsync(); return peerID; });
        }

        public static Task<int> GetRelayPeerCount()
        {
            return Task.Run(() => { return ipfsRelay.Swarm.PeersAsync().Result.Count(); });
        }

    }
}