using Ipfs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using CipherDuo.IPFS.Logger;

namespace CipherDuo.IPFS.Modules
{

    public static class IPFSWrite
    {
        private static ILog logger = LoggerFactory.GetLogger(nameof(IPFS));
        // Base Data Write Layer

        public static async Task<Cid> WriteByte(byte[] data)
        {
            try
            {
                using (Stream stream = new MemoryStream(data))
                {
                    var file = await IPFS.ipfsRelay.FileSystem.AddAsync(stream);
                    return file.Id;
                }
            }
            catch (Exception error)
            {
                logger.Log(error.Message);
            }
            return null;
        }
        public static async Task<Cid> WriteObject(object data)
        {
            try
            {
                var dagHash = await IPFS.ipfsRelay.Dag.PutAsync(data);

                return dagHash;
            }
            catch (Exception error)
            {
                logger.Log(error.Message);
            }
            return null;
        }
    }

}
