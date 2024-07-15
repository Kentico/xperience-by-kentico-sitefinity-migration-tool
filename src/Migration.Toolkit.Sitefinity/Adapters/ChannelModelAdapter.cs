using CMS.ContentEngine;
using CMS.Helpers;

using Kentico.Xperience.UMT.Model;

using Microsoft.Extensions.Logging;

using Migration.Toolkit.Data.Models;
using Migration.Toolkit.Sitefinity.Abstractions;
using Migration.Toolkit.Sitefinity.Model;

namespace Migration.Toolkit.Sitefinity.Adapters;
internal class ChannelModelAdapter(ILogger<ChannelModelAdapter> logger) : UmtAdapterBaseWithDependencies<Site, ChannelDependencies>(logger)
{
    protected override IEnumerable<IUmtModel>? AdaptInternal(Site source, ChannelDependencies channelDependencies)
    {
        var channel = new ChannelModel
        {
            ChannelDisplayName = source.Name,
            ChannelName = ValidationHelper.GetCodeName(source.Name),
            ChannelGUID = source.Id,
            ChannelType = ChannelType.Website,
        };

        yield return channel;

        var siteDefaultLanguage = source.Cultures?.FirstOrDefault(x => x.IsDefault);

        if (siteDefaultLanguage == null)
        {
            logger.LogWarning($"Default language not found for site {source.Name}");
            yield break;
        }

        var language = channelDependencies.ContentLanguages.FirstOrDefault(x => x.ContentLanguageCultureFormat == siteDefaultLanguage.UICulture);

        if (language == null)
        {
            logger.LogWarning($"Imported language not found for site {source.Name}");
            yield break;
        }

        var websiteChannel = new WebsiteChannelModel
        {
            WebsiteChannelChannelGuid = source.Id,
            WebsiteChannelGUID = source.Id,
            WebsiteChannelDefaultCookieLevel = 1000,
            WebsiteChannelDomain = source.LiveUrl,
            WebsiteChannelHomePage = "home",
            WebsiteChannelPrimaryContentLanguageGuid = language.ContentLanguageGUID,
            WebsiteChannelStoreFormerUrls = false
        };

        yield return websiteChannel;
    }

}
