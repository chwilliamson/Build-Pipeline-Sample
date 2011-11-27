using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Build.Utilities;
using Microsoft.Build.Framework;

namespace Williamson.MSBuild.Tasks
{
    public abstract class BaseJenkinTask : Task
    {
        /// <summary>
        /// Jenkins Url
        /// </summary>
        [Required]
        public string Url
        {
            get;
            set;
        }

        protected IJenkinsTasks JenkinsTasks
        {
            get;
            set;
        }

        public override bool Execute()
        {
            this.Log.LogMessage("Creating for " + this.Url);
            this.JenkinsTasks = new JenkinsTasks(new Uri(this.Url), this.Log);
            return this.DoExecute();
        }

        protected abstract bool DoExecute();
        
    }
}
