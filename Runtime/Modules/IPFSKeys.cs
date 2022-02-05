using Ipfs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CipherDuo.IPFS.Constants;
using CipherDuo.IPFS.Logger;
using static CipherDuo.IPFS.Utility.IPFSUtility;

namespace CipherDuo.IPFS.Modules
{

    public static class IPFSKeys
    {
        private static ILog logger = LoggerFactory.GetLogger(nameof(IPFSKeys));
        public static async Task SetSharedKey()
        {
            var hash = await SetKey(IPFSSharedKey);
            if (hash == null) return;
            IPFSSharedKey.hash = hash.ToString();
        }

        public static async Task SetKey(string name, string keyPassword)
        {
            IpfsKey key = new IpfsKey
            {
                name = name,
                keyPassword = keyPassword.ToCharArray(),
            };

            await SetKey(key);
        }

        public static async Task<Cid> SetKey(IpfsKey key)
        {

            try
            {

                IEnumerable<IKey> engineKeys = await IPFS.ipfsRelay.Key.ListAsync();

                if (engineKeys.Any(k => k.Name == key.name))
                {
                    logger.Log("Key with name: {0} already exist.", key.name);
                    return null;
                }

                IKey engineKey = await IPFS.ipfsRelay.Key.CreateAsync(key.name, "rsa", 1024);
                string pem = await IPFS.ipfsRelay.Key.ExportAsync(key.name, key.keyPassword);
                var file = await IPFS.ipfsRelay.FileSystem.AddTextAsync(pem);

                return file.Id;
            }
            catch (Exception error)
            {
                logger.Log(error.Message);
            }

            return null;
        }

        public static async Task GetSharedKey()
        {
            await GetKey(IPFSSharedKey);
        }

        public static async Task<IKey> GetKey(IpfsKey key)
        {
            try
            {
                IEnumerable<IKey> allKeys = await IPFS.ipfsRelay.Key.ListAsync();

                if (!allKeys.Any(k => k.Name == key.name))
                {
                    string pem = await IPFS.ipfsRelay.FileSystem.ReadAllTextAsync(key.hash);
                    var importedKey = await IPFS.ipfsRelay.Key.ImportAsync(key.name, pem, key.keyPassword);

                    return importedKey;
                }
                else
                {
                    foreach (var engineKey in allKeys)
                    {
                        if (engineKey.Name == key.name)
                        {
                            return engineKey;
                        }
                    }

                    logger.Log("Key doesn't exist");

                    return null;
                }
            }
            catch (Exception error)
            {
                logger.Log(error.Message);
            }

            return null;
        }
    }

}