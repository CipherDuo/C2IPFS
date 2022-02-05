using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Runtime.Serialization;
using CipherDuo.IPFS.Serialization;
using Newtonsoft.Json;

namespace Ipfs.Http
{
    /// <summary>
    ///   A published message.
    /// </summary>
    /// <remarks>
    ///   The <see cref="PubSubApi"/> is used to publish and subsribe to a message.
    /// </remarks>
    [DataContract]
    public class PublishedMessage : IPublishedMessage
    {
        /// <summary>
        ///   Creates a new instance of <see cref="PublishedMessage"/> from the
        ///   specified JSON string.
        /// </summary>
        /// <param name="json">
        ///   The JSON representation of a published message.
        /// </param>
        public PublishedMessage(string json)
        {
            
            var o = JObject.Parse(json);
            var sender = (string)o["from"];
            var data = (string) o["data"];
            var sequence = (string)o["seqno"];
            var topics = (JArray) (o["topicIDs"]);
            
            // TODO find better way to encode PubSub Messages
            this.Sender = sender;
            this.SequenceNumber = Encoding.ASCII.GetBytes(sequence);
            this.DataBytes = Encoding.ASCII.GetBytes(data);
            
            this.Topics = topics.Select(t => (string)t);
        }

        /// <inheritdoc />
        [DataMember]
        public Peer Sender { get; private set; }

        /// <inheritdoc />
        [DataMember]
        public IEnumerable<string> Topics { get; private set; }

        /// <inheritdoc />
        [DataMember]
        public byte[] SequenceNumber { get; private set; }

        /// <inheritdoc />
        [DataMember]
        public byte[] DataBytes { get; private set; }

        /// <inheritdoc />
        public Stream DataStream
        {
            get
            {
                return new MemoryStream(DataBytes, false);
            }
        }

        /// <inheritdoc />
        [DataMember]
        public long Size
        {
            get { return DataBytes.Length;  }
        }
        /// <summary>
        ///   Contents as a string.
        /// </summary>
        /// <value>
        ///   The contents interpreted as a UTF-8 string.
        /// </value>
        public string DataString
        {
            get
            {
                return Encoding.UTF8.GetString(DataBytes);
            }
        }

        /// <summary>>
        ///   NOT SUPPORTED.
        /// </summary>
        /// <exception cref="NotSupportedException">
        ///   A published message does not have a content id.
        /// </exception>
        public Cid Id => throw new NotSupportedException();
    }
}
