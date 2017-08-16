using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using pharosArt.Controllers;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Core.Services;
using Umbraco.Web;
using Umbraco.Web.PublishedContentModels;

namespace pharosArt
{
    public class RegisterEvents : ApplicationEventHandler
    {
        protected override void ApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            MediaService.Trashing += MediaService_Trashed;
            MemberService.Deleting += MemberService_Deleting;
        }

        void MemberService_Deleting(IMemberService sender, Umbraco.Core.Events.DeleteEventArgs<IMember> e)
        {
            var membersMediaRoot = AppHelper.GetHomeNode().MembersMediaRootFolder;
            foreach (var member in e.DeletedEntities.Where(x => x.ContentType.Alias.InvariantEquals("Member")))
            {
                var memberMedia = membersMediaRoot.Descendants<ParentFolder>().Where(x => x.Member != null && x.Member.Id == member.Id).ToList();
                if (memberMedia.Any())
                {
                    foreach (var folder in memberMedia)
                    {
                        ApplicationContext.Current.Services.MediaService.Delete(ApplicationContext.Current.Services.MediaService.GetById(folder.Id));
                    }
                }
            }
        }

        void MediaService_Trashed(IMediaService sender, Umbraco.Core.Events.MoveEventArgs<IMedia> e)
        {
            foreach (var media in e.MoveInfoCollection.Where(x => x.Entity.ContentType.Alias.InvariantEquals(ParentFolder.ModelTypeAlias)))
            {
                if (AppHelper.UmbHelper().TypedMedia(media.Entity.Id).HasValue("member"))
                {
                    var member =
                        ApplicationContext.Current.Services.MemberService.GetById(
                            AppHelper.UmbHelper().TypedMedia(media.Entity.Id).GetPropertyValue<int>("member"));
                    if (member != null)
                        ApplicationContext.Current.Services.MemberService.Delete(member);
                }
            }
        }
    }
}