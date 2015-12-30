namespace Suteki.TardisBank.Web.Mvc
{
    using System;
    using Domain;
    using Models;

    public class ModelMappingProfile: AutoMapper.Profile
    {
        protected override void Configure()
        {
            CreateMap<Announcement, AnnouncementSummary>();

            CreateMap<Announcement, AnnouncementModel>()
                .ReverseMap()
                .ForMember(d => d.Id, opt => opt.Ignore())
                .ForMember(d => d.LastModifiedUtc, opt => opt.ResolveUsing((AnnouncementModel a) => DateTime.UtcNow));
        }
    }
}