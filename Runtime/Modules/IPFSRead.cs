using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using CipherDuo.IPFS.Logger;
using Ipfs;

namespace CipherDuo.IPFS.Modules
{
    public static class IPFSRead
    {
        private static ILog logger = LoggerFactory.GetLogger(nameof(IPFS));
        // Base Data Read Layer

        public static async Task<byte[]> ReadBytes(string cid)
        {

            try
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    var fileA = await IPFS.ipfsRelay.FileSystem.ReadFileAsync(cid);
                        
                    fileA.CopyTo(stream);
                    fileA.Dispose();

                    return stream.ToArray();

                }

            }
            catch (Exception error)
            {
                logger.Log(error.Message);
            }
            return null;
        }

        public static async Task<T> ReadDag<T>(string cid)
        {
            try
            {
                var dag = await IPFS.ipfsRelay.Dag.GetAsync<T>(cid);

                return dag;
            }
            catch (Exception error)
            {
                logger.Log(error.Message);
            }
            return default;

        }

    }

}