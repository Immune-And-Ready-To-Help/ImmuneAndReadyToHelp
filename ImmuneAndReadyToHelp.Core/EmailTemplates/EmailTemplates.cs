using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace ImmuneAndReadyToHelp.Core.EmailTemplates
{
    public static class EmailTemplates
    {
        public static string GetTemplate(string templateName, IEnumerable<KeyValuePair<string, string>> valuesToInsert = null)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = $"ImmuneAndReadyToHelp.Core.EmailTemplates.{templateName}";
            string htmlTemplate;

            using (var stream = assembly.GetManifestResourceStream(resourceName))
            using (var reader = new StreamReader(stream))
                htmlTemplate = reader.ReadToEnd();

            //by convention, parameters in email body take the form "{key}" which will be replaced by the value in the parameter.
            foreach (var parameter in valuesToInsert)
                htmlTemplate = htmlTemplate.Replace($"{{{parameter.Key}}}", parameter.Value);

            return htmlTemplate;
        }
    }
}
