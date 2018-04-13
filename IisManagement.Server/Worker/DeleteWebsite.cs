using System;
using System.IO;
using IisAdmin;
using IisManagement.Shared;
using NLog;

namespace IisManagement.Server.Worker
{
    public class DeleteWebsite : SiteManagement, IWorker<DeleteWebsiteRequest, DefaultResult>
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        public DefaultResult ReceiveAndSendMessage(DeleteWebsiteRequest message)
        {
            try
            {
                Logger.Info("Starting CreateWebsite");
                CurrentSite = message.SiteInformation;
                ManipulateHostsFile();
                ChangeWebsite();
                Logger.Info("Finished CreateWebsite");
                return new DefaultResult { Success = true };
            }
            catch (Exception e)
            {
                Logger.Error(e);
            }

            return new DefaultResult { Success = false };
        }


        private void ChangeWebsite()
        {

            RemoveSiteDirectory();
        }

        private void RemoveSiteDirectory()
        {
            var sitepath = GetSitePath();
            if (!Directory.Exists(sitepath))
                Directory.Delete(sitepath, true);
        }

        private void ManipulateHostsFile()
        {
            ServerHostsFile.RemoveSite(CurrentSite);
            ServerHostsFile.RemoveDuplicateEmptyHostEntries();
        }
    }
}