using Ipfs;
using System;
using System.Threading;
using CipherDuo.IPFS.Logger;

namespace CipherDuo.IPFS.Modules
{

    public static class IPFSPubSub
    {
        private static ILog logger = LoggerFactory.GetLogger(nameof(IPFSPubSub));
        
        public static void SubribeToTopic(string topic, Action<IPublishedMessage> response)
        {
            try
            {
                IPFS.ipfsRelay.PubSub.SubscribeAsync(topic, response, new CancellationTokenSource(TimeSpan.FromMinutes(300)).Token);
            }
            catch (Exception error)
            {
                logger.Log(error.Message);
            }
        }
        
        public static void SubribeToTopic(string topic)
        {
            try
            {
                IPFS.ipfsRelay.PubSub.SubscribeAsync(topic, RelayTopicMessageReceived,new CancellationTokenSource(TimeSpan.FromSeconds(360)).Token);
            }
            catch (Exception error)
            {
                logger.Log(error.Message);
            }
        }

        public static void WriteToTopic(string topic, byte[] msg)
        {
            try
            {
                IPFS.ipfsRelay.PubSub.PublishAsync(topic, msg);
            }
            catch (Exception e)
            {
                logger.Log(e.Message);
            }
        }


        private static void TopicMessageReceived(IPublishedMessage obj)
        {
            var bytes = obj.DataBytes;
            logger.Log($"engine received :{bytes.Length}");
        }

        private static void RelayTopicMessageReceived(IPublishedMessage obj)
        {
            var bytes = obj.DataBytes;
            logger.Log($"relay received :{bytes.Length}");
        }
    }

}