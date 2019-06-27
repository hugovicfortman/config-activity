using Newtonsoft.Json.Linq;
using System.Activities;
using System.ComponentModel;
using System.IO;

namespace ConfigActivity
{
    public class ConfigActivity : CodeActivity
    {
        [Category("Input")]
        [RequiredArgument]
        public InArgument<string> ConfigurationPath { get; set; }

        [Category("Output")]
        public OutArgument<JObject> Configuration { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            string configPath = ConfigurationPath.Get(context);
            if(File.Exists(configPath))
            {
                string config_text = File.ReadAllText(configPath);
                JObject config_jobject = JObject.Parse(config_text);
                Configuration.Set(context, config_jobject);
                // We purposefully didn't put this in a try-catch 
                // block so the user can handle issues in their process
            }
            throw new FileNotFoundException(string.Format("Configuration file \"{0}\" was not found", configPath));
        }
    }
}
