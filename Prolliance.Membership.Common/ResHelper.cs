using System.IO;
using System.Reflection;

namespace Prolliance.Membership.Common
{
    public class ResHelper
    {
        public static string ReadTextFromRes(Assembly assembly, string fullName)
        {
            Stream stream = assembly.GetManifestResourceStream(fullName);
            StreamReader reader = new StreamReader(stream);
            string text = reader.ReadToEnd();
            return text;
        }
    }
}
