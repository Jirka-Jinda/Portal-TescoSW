using System.Collections.Generic;
using System.DirectoryServices;
using Microsoft.Web.Administration;

namespace Models
{
	public class AppLinksModel
	{
		public Guid Id { get; set; }
		public string Name { get; set; }
		public string Url { get; set; }
		public AppState State { get; set; }
		public AppCategory Category { get; set; }
		public DispatcherControlModel Dispatcher { get; set; }
        public Dictionary<string,string> ConnectionString { get; set; }

        public AppLinksModel(string name, string url, AppCategory category)
		{
			Id = Guid.NewGuid();
			Name = name;
			Url = url;
			State = AppState.Unknown;
			Category = category;
			Dispatcher = new DispatcherControlModel(this);
			ConnectionString = FileIOModel.ReadDatabaseLink(name) ?? new();
		}

		public static void InitAppLinks()
		{
			var data = GetAppLinksFromPool();
			FileIOModel.ClearFiles();
			FileIOModel.WriteAppLinks(data);
        }

		private static bool IsPoolValid(string name)
		{
			string[] InvalidNames = {"default",".NET", "rozcestink", "DMS"};

			foreach (string invalidName in InvalidNames)
			{
				if (name.ToLower().Contains(invalidName.ToLower()))
					return false;
			}

			return true;
		}

        #region Updaty
        public static void UpdateAppLinksStatus(List<AppLinksModel> Applications)
		{
			try
			{
				using (ServerManager serverManager = new ServerManager())
				{
					foreach(AppLinksModel Application in Applications)
					{
                        ApplicationPool applicationPool = serverManager.ApplicationPools.FirstOrDefault(pool => pool.Name == Application.Name);

                        if (applicationPool != null)
                        {
                            var state = applicationPool.State;

                            switch (state)
                            {
                                case ObjectState.Starting:
                                    Application.State = AppState.Starting;
                                    break;
                                case ObjectState.Started:
                                    Application.State = AppState.Started;
                                    break;
                                case ObjectState.Stopping:
                                    Application.State = AppState.Stopping;
                                    break;
                                case ObjectState.Stopped:
                                    Application.State = AppState.Stopped;
                                    break;
                                default:
									Application.State = AppState.Unknown;
									break;
                            }
                        }
                        else
                        {
                            ErrorViewModel.Log($"Updating {Application.Name} status failed, server manager couldn't find the app.");
                        }
                    }
					FileIOModel.WriteAppLinks(Applications);
				}
			}
			catch (Exception e)
			{
				ErrorViewModel.Log("Links update failed" + e.Message);
            }
        }

        public static void UpdateAppLinkStatus(AppLinksModel App)
		{
			UpdateAppLinksStatus(new List<AppLinksModel>() { App });
        }

		public static void UpdateAllAppLinksStatus()
		{
			var names = new List<string>();
			var apps = GetAppLinks();
			UpdateAppLinksStatus(apps);
		}
        #endregion

        #region Gettery
        private static List<AppLinksModel> GetAppLinksFromPool()
		{
			var data = new List<AppLinksModel>();

            using (ServerManager serverManager = new())
            {
				try
				{
                    Site site = serverManager.Sites.FirstOrDefault();

                    if (site != null)
                    {
                        foreach (Application app in site.Applications)
                        {
							if (!IsPoolValid(app.ApplicationPoolName)) continue;

							data.Add(new AppLinksModel(
								app.ApplicationPoolName,
								@"https://maj-web13/" + app.ApplicationPoolName,
								AppCategory.Kraje));
                        }
                    }
					UpdateAllAppLinksStatus();
                }
				catch(Exception e)
				{
					ErrorViewModel.Log("Failed to load applications from the pool. Scheduling next update and using archived links." + e.Message);
                    // Selhani nacteni aplikacniho poolu
                    // Preskoceni aktualizace, nacteni stareho a schedule noveho na pozdeji
                }
            }
			return data;
        }

		private static List<AppLinksModel> GetAppLinks()
		{
			return FileIOModel.ReadAppLinks().OrderBy(link => link.Name).ToList();
		}

		public static List<AppLinksModel> GetAllAppLinks()
        {
            var data = GetAppLinks();
            UpdateAppLinksStatus(data);
            return data;
        }

        public static List<AppLinksModel> GetAppLinksByCategory(AppCategory category)
		{
            var data = GetAppLinks();
            var result = new List<AppLinksModel>();
			foreach (var item in data)
			{
				if (item.Category == category)
					result.Add(item);
			}
			UpdateAppLinksStatus(result);
            return result;
        }

        public static List<AppLinksModel> GetAppLinksByText(string text)
		{
			var data = GetAppLinks();
            var result = new List<AppLinksModel>();

			foreach (var item in data) 
			{
				if (item.Name.ToLower().Contains(text.ToLower()) || item.Url.ToLower().Contains(text.ToLower()))
					result.Add(item);
			}
            UpdateAppLinksStatus(result);
            return result;
        }
        #endregion

        #region Ovladani aplikaci
		public static void AppRestart(string poolName)
		{
			try
			{
				using (ServerManager serverManager = new())
				{
					ApplicationPool applicationPool = serverManager.ApplicationPools.FirstOrDefault(ap => ap.Name == poolName);

					if (applicationPool != null)
					{
						if (applicationPool.State == ObjectState.Started)
							applicationPool.Recycle();
					}
				}
			}
			catch (Exception e)
			{
				Models.ErrorViewModel.Log("Error while trying to restart application pool. Most likely pool was not found. " + e.Message);
			}
        }

	    #endregion

    }
}
