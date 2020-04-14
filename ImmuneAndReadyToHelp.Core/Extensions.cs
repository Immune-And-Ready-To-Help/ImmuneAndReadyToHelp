using System;
using System.Linq;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;
using System.Text.RegularExpressions;

namespace ImmuneAndReadyToHelp.Core
{
    public static class Extensions
    {
        public static string SanitizeInput(this string input)
        {
            if (String.IsNullOrEmpty(input)) return input;

            //remove tags to avoid cross site scripting
            //TODO: make more robust and maybe allow for formatting tags.
            return Regex.Replace(input, "[<>]", String.Empty, RegexOptions.Singleline | RegexOptions.IgnoreCase);
        }

        public static string GetExtension(this Image img)
            => ImageCodecInfo.GetImageEncoders()
            .FirstOrDefault(x => x.FormatID == img.RawFormat.Guid)
            .FilenameExtension
            .Split(";", StringSplitOptions.RemoveEmptyEntries)
            .FirstOrDefault()
            .Trim('*')
            .ToLower();
    }
}
