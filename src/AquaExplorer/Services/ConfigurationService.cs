using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using AquaExplorer.BusinessObjects;

namespace AquaExplorer.Services
{
    class ConfigurationService : IConfigurationService
    {
        private readonly IFileSystem _fileSystem;

        public ConfigurationService(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
        }

        public Configuration ReadConfiguration()
        {
            return ReadConfigurationImpl() ?? 
                new Configuration { Accounts = new List<ProtectedCredentials>() };
        }

        private Configuration ReadConfigurationImpl()
        {
            var configFilePath = GetConfigFilePath();
            if (!_fileSystem.FileExists(configFilePath)) return null;

            try
            {
                using (var stream = _fileSystem.OpenRead(configFilePath))
                {
                    var serializer = new XmlSerializer(typeof (Configuration));
                    return (Configuration) serializer.Deserialize(stream);
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public void WriteConfiguration(Configuration config)
        {
            if (config == null) throw new ArgumentNullException("config");

            var configFilePath = GetConfigFilePath();
            var configFileDir = Path.GetDirectoryName(configFilePath);
            _fileSystem.CreateDirectory(configFileDir);

            var settings = new XmlWriterSettings
            {
                OmitXmlDeclaration = true,
                Indent = true,
                IndentChars = "    "
            };

            var ns = new XmlSerializerNamespaces();
            ns.Add("", "");

            using (var stream = _fileSystem.OpenWrite(configFilePath))
            using (var writer = XmlWriter.Create(stream, settings))
            {
                var serializer = new XmlSerializer(typeof (Configuration));
                serializer.Serialize(writer, config, ns);
            }
        }


        private string GetConfigFilePath()
        {
            return Path.Combine(_fileSystem.GetProgramDataFolder(), "AquaExplorer", "config.xml");
        }
    }
}
