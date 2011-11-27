using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using Microsoft.Build.Utilities;
using Microsoft.Build.Framework;
using System.Web;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace Williamson.MSBuild.Tasks
{
    /// <summary>
    /// Perform various jenkins tasks
    /// </summary>
    public class JenkinsTasks : IJenkinsTasks
    {


        private Uri JenkinsUri
        {
            get;
            set;
        }

        private TaskLoggingHelper Log
        {
            get;
            set;
        }
        public JenkinsTasks(Uri jenkinsUri, TaskLoggingHelper logging)
        {
            this.JenkinsUri = jenkinsUri;
            this.Log = logging;
        }

        public void ReloadConfiguration()
        {
            string url = this.JenkinsUri.OriginalString + "/reload";
            this.Log.LogMessage("Reloading: " + url);
            //visit the jenkins url
            try
            {
                var wr = HttpWebRequest.Create(url);
                wr.Timeout = 10000;
                wr.GetResponse();
            }
            catch
            {
                //do nothing for now
            }
        }


        public void CreatePipelineView(string name, string description, bool filterBuildQueue, bool filterBuildExecutors, string title, string initialJob, int noOfBuilds, bool restrict)
        {
            Log.LogMessage("CreatePipelineView");

            dynamic o = new JObject();
            o.name = name;
            o.mode = "au.com.centrumsystems.hudson.plugin.buildpipeline.BuildPipelineView";
            o.Submit = "OK";

            this.Post("createView", o);

            o = new JObject();
            o.description = description;
            o.name = name;
            o.Submit = "OK";
            o.buildViewTitle = title;           
            o.selectedJob = initialJob;
            o.noOfDisplayedBuilds = "5";
            o.filterExecutors = "false";
            o.triggerOnlyLatestJob = "false";
            o.filterQueue = "false";
            o.triggerOnlyLatestJob = "false";
            
            this.Post(string.Format("view/{0}/configSubmit", name),o);
            var r = Create(this.JenkinsUri, string.Format("view/{0}/configSubmit",name));
        }

        private static void Post(WebRequest r, string body)
        {
            r.Method = "POST";
            r.ContentType = "application/x-www-form-urlencoded";

            using (var sr = new StreamWriter(r.GetRequestStream()))
            {
                sr.Write(body);
            }

            r.GetResponse();
        }

        static object GetValue(JToken t)
        {
            if (t is JValue)
            {
                return ((JValue)t).Value;
            }
            return "";
        }

        private void Post(string action, JObject o)
        {
            var r = Create(this.JenkinsUri, action);
            
            JsonSerializer serializer = new JsonSerializer();
            var body = o.Select<JToken, string>(t => {
                if (t is JProperty)
                {
                    var p = (JProperty)t;
                    return p.Name + "=" + GetValue(p.Value).ToString();
                }
                return "";
            });
            var bodyAsString = string.Join("&",body);
            //ass json
            bodyAsString += "&json=" + HttpUtility.UrlEncode(o.ToString(Formatting.None));
            try
            {
                Post(r, bodyAsString);
            }
            catch
            {
                Log.LogError(bodyAsString);
                throw;
            }
        }


        public void CreateListView(string name, string description)
        {
            Log.LogMessage("CreateListView");
        }


        public bool ViewExists(string name)
        {
            var request = Create(this.JenkinsUri, "");
            var response = request.GetResponse();
            string v = ">" + name + "</a>";
            bool exists = DoesValueExistInResponse(v, response);
            response.Close();

            return exists;
        }

        private bool DoesValueExistInResponse(string value, WebResponse response)
        {
            Log.LogMessage("DoesValueExistInResponse");

            using (var sr = new StreamReader(response.GetResponseStream()))
            {
                string s = sr.ReadToEnd();
                return s.Contains(value);
            }
        }

        private static WebRequest Create(Uri uri, string action)
        {
            var r = WebRequest.Create(uri + action);
            return r;

        }
    }
}
