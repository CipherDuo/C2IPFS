using System.Text;
using UnityEngine;

namespace CipherDuo.IPFS.Logger
{
    public class LoggerFactory
    {
        public static ILog GetLogger<T>()
        {
            return new UnityLogger(typeof(T).Name);
        }

        public static ILog GetLogger(string className)
        {
            return new UnityLogger(className);
        }
        
        private class UnityLogger : ILog
        {
            private readonly string m_Name;

            public UnityLogger(string name)
            {
                m_Name = name;
            }
    
            public void Log(string msg, params object[] args)
            {
                var formattedMessage = FormatMessage(msg, args);
                Debug.Log(formattedMessage);
            }

            public void Error(string msg, params object[] args)
            {
                var formattedMessage = FormatMessage(msg, args);
                Debug.LogError(formattedMessage);
            }

            public void Warns(string msg, params object[] args)
            {
                var formattedMessage = FormatMessage(msg, args);
                Debug.LogWarning(formattedMessage);
            }
    
            private string FormatMessage(string msg, params object[] args)
            {
                var richName = $"[{m_Name}]: ";
                var builder = new StringBuilder(richName);
                if (args is not null && args.Length > 0)
                {
                    builder.AppendFormat(msg, args);
                }
                else
                {
                    builder.Append(msg);
                }

                return builder.ToString();
            }
        }
    }
}