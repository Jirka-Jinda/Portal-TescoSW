using System.Text.Json;

namespace Models
{
    public static class FileIOModel
    {
        public static bool Ready { get; private set; } = false;
        public static string AppLinksFile
        {
            get { return _appLinksFile; }
            set
            {
                if (Path.Exists(value))
                    _appLinksFile = value;
                else
                    throw new Exception("Invalid file path for application links, in root directory './' there should be 'AppLinks.json' present.");
            }
        }
        private static string _appLinksFile;
        public static string SiteLinksFile
        {
            get { return _siteLinksFile; }
            set
            {
                if (Path.Exists(value))
                    _siteLinksFile = value;
                else
                    throw new Exception("Invalid file path for site links, in root directory './' there should be 'SiteLinks.json' present.");
            }
        }
        private static string _siteLinksFile;
        public static string LogFile
        {
            get { return _LogFile; }
            set
            {
                if (Path.Exists(value))
                    _LogFile = value;
                else
                    throw new Exception("Invalid file path for site links, in root directory './' there should be 'SiteLinks.json' present.");
            }
        }
        private static string _LogFile;

        public static void InitFilePaths()
        {
            try
            {
                string path = AppDomain.CurrentDomain.BaseDirectory;
                AppLinksFile = path + "JsonDb/AppLinks.json";
                SiteLinksFile = path + "JsonDb/SiteLinks.json";
                LogFile = path + "JsonDb/Log.json";
            }
            catch (Exception e)
            {
                ErrorViewModel.Log(e.Message);
            }
        }

        // IO App Links
        #region
        public static void WriteAppLinks(List<AppLinksModel> appLinks)
        {
            Ready = false;
            bool done = false;
            
            try
            {
                var OrigContentObjTemp = ReadAppLinks();
                foreach (var item in appLinks.OrderBy(link => link.Name).ToList())
                {
                    if (OrigContentObjTemp is not null && !OrigContentObjTemp.Any(Link => Link.Name == item.Name))
                        OrigContentObjTemp.Add(item);
                }
                    

                string ToWrite = JsonSerializer.Serialize(OrigContentObjTemp);

                done = false;
                using (StreamWriter sw = new(AppLinksFile))
                {
                    sw.Write(ToWrite);
                }
            }
            catch (Exception e) when (done == false)
            {
                ErrorViewModel.Log("Error in file access while writing new application links.\n" + e.Message);
            }
            catch (Exception e)
            {
                ErrorViewModel.Log("Error in json parsing while writing new application links.\n" + e.Message);
            }
            finally
            {
                Ready = true;
            }
        } // TODO async
        public static void WriteAppLinks(AppLinksModel appLink)
        {
            WriteAppLinks(new List<AppLinksModel>() { appLink });
        }

        public static List<AppLinksModel>? ReadAppLinks()
        {
            Ready = false;
            string OriginalContent = "";

            try
            {
                using (StreamReader sr = new(AppLinksFile))
                {
                    OriginalContent = sr.ReadToEnd();
                }
                if (OriginalContent == "")
                    return new List<AppLinksModel>();
                else
                    return JsonSerializer.Deserialize<List<Models.AppLinksModel>>(OriginalContent);
            }
            catch (Exception e)
            {
                ErrorViewModel.Log("Error in file access while reading new application links.\n" + e.Message);
                return null;
            }
            finally { Ready = true; }
        }
        #endregion

        // IO Site Links
        #region
        public static void WriteSiteLinks(List<SiteLinksModel> appLinks)
        {
            Ready = false;
            bool done = false;

            try
            {
                var OrigContentObjTemp = ReadSiteLinks();
                foreach (var item in appLinks)
                    OrigContentObjTemp?.Add(item);

                string ToWrite = JsonSerializer.Serialize(OrigContentObjTemp);
                done = false;
                using (StreamWriter sw = new(SiteLinksFile))
                {
                    sw.Write(ToWrite);
                }
            }
            catch (Exception e) when (done == false)
            {
                ErrorViewModel.Log("Error in file access while writing new site links.\n" + e.Message);
            }
            catch (Exception e)
            {
                ErrorViewModel.Log("Error in json parsing while writing new site links.\n" + e.Message);
            }
            finally
            {
                Ready = true;
            }
        }
        public static void WriteSiteLinks(SiteLinksModel appLink)
        {
            WriteSiteLinks(new List<SiteLinksModel>() { appLink });
        }
        public static List<SiteLinksModel>? ReadSiteLinks()
        {
            Ready = false;
            string OriginalContent = "";
            try
            {
                using (StreamReader sr = new(SiteLinksFile))
                {
                    OriginalContent = sr.ReadToEnd();
                }
                if (OriginalContent == "")
                    return new List<SiteLinksModel>();
                else
                    return JsonSerializer.Deserialize<List<SiteLinksModel>>(OriginalContent);
            }
            catch (Exception e)
            {
                ErrorViewModel.Log("Error in file access while reading new application links.\n" + e.Message);
                return null;
            }
            finally { Ready = true; }
        }
        #endregion

        // IO Database Links
        #region 
        public static Dictionary<string, string>? ReadDatabaseLink(string appName)
        {
            string appSettingsContent = "";
            var result = new Dictionary<string, string>();
            try
            {
                string path = "/maj-web13/wwwroot/" + appName + "/appsettings.json";
                using (StreamReader sr = new(path))
                {
                    appSettingsContent = sr.ReadToEnd();
                }
                if (appSettingsContent == "")
                    return result;
                else
                {
                    var settings = JsonSerializer.Deserialize<AppSettings>(appSettingsContent);
                    Connectionstring connectionString;

                    switch (settings?.ConnectionStrings.Count())
                    {
                        case 0:
                            throw new Exception($"No connection string has been read for {appName}");
                        case 1:
                            connectionString = settings.ConnectionStrings[0];
                            break;
                        default:
                            ErrorViewModel.Log($"Application {appName} has more than one connection strings, resolving ambiguity by taking first as default.");
                            connectionString = settings.ConnectionStrings[0];
                            break;
                    }

                    foreach (string attr in connectionString.ConnectionString.Split(';'))
                    {
                        var values = attr.Split('=');
                        result.Add(values[0], values[1]);
                    }

                    return result;
                }
            }
            catch (Exception e)
            {
                ErrorViewModel.Log("Error while reading appsettings.json or retreiving connection strings.\n" + e.Message);
                return null;
            }
        }

        #endregion

        public static void WriteLog(string message)
        {
            try
            {
                using (StreamWriter sw = new(LogFile))
                {
                    sw.Write(DateTime.Now + "  :  " + message);
                }
            }
            catch(Exception e)
            {
                throw e;
            }
        }

        public static void ClearFiles()
        {
            // Mozna se uz deje ve write App links
            using (StreamWriter sw = new(AppLinksFile))
                sw.Write(string.Empty);
        }
    }
}
