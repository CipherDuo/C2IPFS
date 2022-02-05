namespace CipherDuo.IPFS.Logger
{
    public interface ILog
    {
        public void Log(string msg, params object[] args);
        public void Error(string msg, params object[] args);
        public void Warns(string msg, params object[] args);
    }
}