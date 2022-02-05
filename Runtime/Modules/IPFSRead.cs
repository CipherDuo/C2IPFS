using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using CipherDuo.IPFS.Logger;
using CipherDuo.IPFS.Serialization;
using UnityEngine;
using Ipfs;
using UnityEngine.SceneManagement;

namespace CipherDuo.IPFS.Modules
{
    public static class IPFSRead
    {
        private static ILog logger = LoggerFactory.GetLogger(nameof(IPFSRead));
        // TODO pass a better container than transform for scene
        public static async Task<Transform> ReadScene(string cid)
        {
            ScenePacket packet = await ReadDag<ScenePacket>(cid);

            Transform sceneRoot = new GameObject("Scene").transform;
            
            for (int i = 0; i < packet.gameObjects.Length; i++)
            {
                var go = await ReadGameObject(packet.gameObjects[i]);
                
                var position = await ReadDag<Vector3Packet>(packet.positions[i]);
                var positionX = BitConverter.ToSingle(position.x);
                var positionY = BitConverter.ToSingle(position.y);
                var positionZ = BitConverter.ToSingle(position.z);
                
                go.transform.position = new Vector3(positionX, positionY, positionZ);
                
                var scale = await ReadDag<Vector3Packet>(packet.scales[i]);
                var scaleX = BitConverter.ToSingle(scale.x);
                var scaleY = BitConverter.ToSingle(scale.y);
                var scaleZ = BitConverter.ToSingle(scale.z);
                
                go.transform.localScale = new Vector3(scaleX, scaleY, scaleZ);

                go.transform.parent = sceneRoot;
            }
            
            return sceneRoot;
        }
        
        public static async Task<GameObject> ReadGameObject(string cid)
        {
            GameObjectPacket packet = await ReadDag<GameObjectPacket>(cid);

            GameObject go = new GameObject("GameObject");
            var filter = go.AddComponent<MeshFilter>();
            var renderer = go.AddComponent<MeshRenderer>();

            filter.mesh = await ReadMesh(packet.mesh);
            renderer.material = await ReadMaterial(packet.material);
            
            var rotation = await ReadDag<Vector4Packet>(packet.rotation);
            var x = BitConverter.ToSingle(rotation.x);
            var y = BitConverter.ToSingle(rotation.y);
            var z = BitConverter.ToSingle(rotation.z);
            var w = BitConverter.ToSingle(rotation.w);
            
            go.transform.rotation = new Quaternion(x, y, z, w);
            
            return go;
        }
        
        public static async Task<Material> ReadMaterial(string cid)
        {
            MaterialMetadata serMaterial = await ReadDag<MaterialMetadata>(cid);

            var content = await ReadBytes(serMaterial.hash);

            return Serializator.Deserialize(content, serMaterial);
        }
        
        
        // TODO Uniform ANY to this standard if possible
        public static async Task<Mesh> ReadMesh(string cid)
        {
            Mesh result = MeshSerializer.DeserializeMesh(await ReadBytes(cid));            
            return result;
        }

        public static async Task<Texture2D> ReadTexture(string cid)
        {
            TextureMetadata serTexture = await ReadDag<TextureMetadata>(cid);

            var content = await ReadBytes(serTexture.hash);

            return Serializator.Deserialize(content, serTexture);
        }

        public static async Task<string> ReadString(string cid)
        {
            StringMetadata serString = await ReadDag<StringMetadata>(cid);

            var content = await ReadBytes(serString.hash);

            return Serializator.Deserialize(content);
        }

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