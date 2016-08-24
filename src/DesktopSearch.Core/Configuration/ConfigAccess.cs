using System.ComponentModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopSearch.Core.Configuration
{
    public class ConfigAccess
    {
        private static JsonSerializerSettings _formatSettings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore, Formatting = Formatting.Indented };

        private IStreamFactory _factory;

        public ConfigAccess(IStreamFactory factory)
        {
            _factory = factory;
        }

        public Settings Get()
        {
            //var builder = new ConfigurationBuilder();
            //builder.SetBasePath(directory);
            //builder.AddJsonFile(settingsFile, optional: false, reloadOnChange: true );
            //var connectionStringConfig = builder.Build();
            string content = null;

            
            using (var rd = new StreamReader(_factory.GetReadableStream()))
            {
                content = rd.ReadToEnd();
            }

            var foldersToIndex = JsonConvert.DeserializeObject<Settings>(content);

            return foldersToIndex;
        }

        public static string GetJSONExample()
        {
            var fs = new FoldersToIndex()
            {
                Folders = new[]
                {
                    new Folder { Path="c:\\temp", IndexingType = "Code" },
                    new Folder { Path="c:\\temp", IndexingType = "Documents" }
                }
            };

            var serialized =
            JsonConvert.SerializeObject(
                fs,
                _formatSettings);

            return serialized;
        }

        public void SaveChanges(Settings settings)
        {
            if (settings == null)
                throw new ArgumentNullException(nameof(settings));

            var serialized =
            JsonConvert.SerializeObject(
                settings,
                _formatSettings);

            using (var sw = new StreamWriter(_factory.GetWritableStream()))
            {
                sw.Write(serialized);
            }
        }
    }

    
}
