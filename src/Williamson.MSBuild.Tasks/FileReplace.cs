using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Build.Utilities;
using Microsoft.Build.Framework;
using System.IO;

namespace Williamson.MSBuild.Tasks
{
    /// <summary>
    /// Change to use XmlPoke
    /// </summary>
    public class FileReplace : Task
    {
        [Required]
        public string SrcFile
        {
            get;
            set;
        }

        [Required]
        public string OldValue
        {
            get;
            set;
        }

        [Required]
        public string NewValue
        {
            get;
            set;
        }


        public override bool Execute()
        {
            Log.LogMessage("Replacing: {0} with {1}", this.OldValue, this.NewValue);
            string contents= File.ReadAllText(this.SrcFile,Encoding.UTF8).Replace(this.OldValue, this.NewValue);
            File.WriteAllText(this.SrcFile, contents,Encoding.UTF8);
            return true;
        }
    }
}
