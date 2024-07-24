namespace Models
{
    public class SiteLinksModel
    {
        public Guid Id { get; set; }
        public string Url { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }

        public SiteLinksModel(string name, string url, string description = "")
        {
            Id = Guid.NewGuid();
            Name = name;
            Url = url;
            Description = description;
        }

        public static void InitSiteLinks()
        {
            var links = new List<SiteLinksModel>()
            {
                new SiteLinksModel("MPS Wiki", "https://tfs.tescosw.loc/Segmenty/MPS/_wiki/wikis/Wiki%20dokumentace/5/Odd%C4%9Blen%C3%AD-Majetek"),
                new SiteLinksModel("Tesco Wiki", "https://teaf.tescosw.cz/wiki/index.php?title=Hlavn%C3%AD_strana"),
                new SiteLinksModel("Service Desk MW", "https://sd.tescosw.cz/client/main"),
                new SiteLinksModel("TFS", "https://tfs.tescosw.loc/Segmenty/MPS"),
                new SiteLinksModel("Zdrojové kódy", "https://tfs.tescosw.loc/Segmenty/Ypsilon/_versionControl"),
                new SiteLinksModel("SafeQ", "https://safeq.iis.loc/web/Dashboard.jsp"),
                new SiteLinksModel("Docházka", "https://dochazka.tescosw.loc/index.php"),
                new SiteLinksModel("Sharepoint", "https://sharepoint/default.aspx"),
            };
            FileIOModel.WriteSiteLinks(links);
        }

        public static List<SiteLinksModel> GetSiteLinks(bool RewriteInitialConfiguration = false)
        {
            if (RewriteInitialConfiguration)
                InitSiteLinks();

            var links = FileIOModel.ReadSiteLinks();

            if (links == null || links.Count == 0)
                return GetSiteLinks(true);

            return links;
        }
    }
}
