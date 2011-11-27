using System;
namespace Williamson.MSBuild.Tasks
{
    /// <summary>
    /// Represents various tasks that can be performed on Jenkins
    /// </summary>
    public interface IJenkinsTasks
    {
        /// <summary>
        /// Reloads a jenkins
        /// </summary>
        void ReloadConfiguration();

        void CreatePipelineView(string name,string description,bool filterBuildQueue, 
            bool filterBuildExecutors, string title,string initialJob,int noOfBuilds,bool restrict);

        void CreateListView(string name,string description);

        bool ViewExists(string p);
    }
}
