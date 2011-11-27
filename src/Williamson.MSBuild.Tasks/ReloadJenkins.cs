using System;
using Microsoft.Build.Utilities;
using Microsoft.Build.Framework;
using System.Net;

namespace Williamson.MSBuild.Tasks
{
    /// <summary>
    /// Causes Jenkins to reload configuration
    /// </summary>
    public class ReloadJenkins : BaseJenkinTask
    {        
        /// <summary>
        /// Do something
        /// </summary>
        /// <returns></returns>
        protected override bool DoExecute()
        {            
            Log.LogMessage("Reloading Jenkins: {0}", this.Url);

            //visit the jenkins url
            try
            {
                this.JenkinsTasks.ReloadConfiguration();
                return true;
            }
            catch (Exception ex)
            {
                Log.LogErrorFromException(ex);
                return false;
            }
        }
    }
}
