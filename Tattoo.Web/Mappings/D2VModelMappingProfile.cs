#region Using Directives

using System;
using System.Linq;
using AutoMapper;
using PagedList;
using Tattoo.Common.Enumerations;
using Tattoo.Data.Entities;
using Tattoo.Web.Core.AutoMapper;
using Tattoo.Web.Models;

#endregion

namespace Tattoo.Web.Mappings
{
    public class D2VModelMappingProfile : Profile
    {
        public override string ProfileName
        {
            get { return "DomainToViewModelMappings"; }
        }

        protected override void Configure()
        {
            Mapper.CreateMap<ApplicationUser, UserInfoViewModel>()
                .ForMember(m => m.Name, d => d.MapFrom(s => s.UserName));

            Mapper.CreateMap<ApplicationUser, UserInfoCommentsViewModel>()
                .ForMember(e => e.AvatarUrl, i => i.MapFrom(s=>s.Avatar));

            Mapper.CreateMap<Position, PositionFormModel>();
            Mapper.CreateMap<Position, PositionViewModel>();
            Mapper.CreateMap<IPagedList<Position>, IPagedList<PositionViewModel>>()
                .ConvertUsing<PagedListConverter<Position, PositionViewModel>>();

            Mapper.CreateMap<BodyZone, BodyZoneFormModels>();
            Mapper.CreateMap<BodyZone, BodyZoneViewModels>();
            Mapper.CreateMap<IPagedList<BodyZone>, IPagedList<BodyZoneViewModels>>()
                .ConvertUsing<PagedListConverter<BodyZone, BodyZoneViewModels>>();

            Mapper.CreateMap<Element, ElementFormModel>()
                .ForMember(e => e.Url, i => i.Ignore());
            Mapper.CreateMap<Element, ElementEditFormModel>()
                .ForMember(e => e.Url, i => i.Ignore());
            Mapper.CreateMap<Element, ElementViewModel>()
                .ForMember(e => e.Avatar, p => p.MapFrom(q => q.Author.Avatar));
            Mapper.CreateMap<IPagedList<Element>, IPagedList<ElementViewModel>>()
                .ConvertUsing<PagedListConverter<Element, ElementViewModel>>();

            Mapper.CreateMap<Comment, CommentFormModel>();
            Mapper.CreateMap<Comment, CommentViewModel>();
            Mapper.CreateMap<IPagedList<Comment>, IPagedList<CommentViewModel>>()
                .ConvertUsing<PagedListConverter<Comment, CommentViewModel>>();

            Mapper.CreateMap<ApplicationUser, UpdateProfileFormModel>()
                .ForMember(e => e.Avatar, i => i.Ignore());

            Mapper.CreateMap<Notice, NoticeViewModel>();
            Mapper.CreateMap<Notice, NoticeFormModel>();
            Mapper.CreateMap<IPagedList<Notice>, IPagedList<NoticeViewModel>>()
               .ConvertUsing<PagedListConverter<Notice, NoticeViewModel>>();
        }
    }
}