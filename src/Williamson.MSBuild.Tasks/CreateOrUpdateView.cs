using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Build.Utilities;
using Microsoft.Build.Framework;

namespace Williamson.MSBuild.Tasks
{
    public class CreateOrUpdateView: BaseJenkinTask
    {
        /// <summary>
        /// The Name of the Job
        /// </summary>
        [Required]
        public string JobName
        {
            get;
            set;
        }

        [Required]
        public string InitialJobName
        {
            get;
            set;
        }

        protected override bool DoExecute()
        {
            //Create ListView
            if (this.JenkinsTasks.ViewExists(this.JobName))
            {
                this.Log.LogMessage("{0} Exists", this.JobName);

               
            }
            else
            {
                this.Log.LogMessage("{0} Not Exists", this.JobName);
                this.JenkinsTasks.CreatePipelineView(this.JobName, "", false, false, this.JobName, this.InitialJobName, 5, false);
            }

           
            return true;
        }
    }
}
