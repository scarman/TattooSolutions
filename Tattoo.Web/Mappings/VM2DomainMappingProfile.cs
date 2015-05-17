#region Using Directives

using AutoMapper;
using PagedList;
using Tattoo.Data.Entities;
using Tattoo.Web.Core.AutoMapper;
using Tattoo.Web.Models;

#endregion

namespace Tattoo.Web.Mappings
{
    public class VM2DomainMappingProfile : Profile
    {
        public override string ProfileName
        {
            get { return "ViewModelToDomainMappings"; }
        }

        protected override void Configure()
        {
            Mapper.CreateMap<PositionFormModel, Position>();
            Mapper.CreateMap<IPagedList<PositionFormModel>, IPagedList<Position>>()
                .ConvertUsing<PagedListConverter<PositionFormModel, Position>>();

            Mapper.CreateMap<BodyZoneFormModels, BodyZone>();
            Mapper.CreateMap<IPagedList<BodyZoneFormModels>, IPagedList<BodyZone>>()
                .ConvertUsing<PagedListConverter<BodyZoneFormModels, BodyZone>>();

            Mapper.CreateMap<ElementFormModel, Element>();
            Mapper.CreateMap<ElementEditFormModel, Element>()
                .ForMember(o => o.Url, p => p.Ignore());
            Mapper.CreateMap<IPagedList<ElementFormModel>, IPagedList<Element>>()
                .ConvertUsing<PagedListConverter<ElementFormModel, Element>>();

            Mapper.CreateMap<CommentFormModel, Comment>();
            Mapper.CreateMap<IPagedList<CommentFormModel>, IPagedList<Comment>>()
                .ConvertUsing<PagedListConverter<CommentFormModel, Comment>>();

            Mapper.CreateMap<UpdateProfileFormModel, ApplicationUser>()
            .ForMember(o => o.Avatar, p => p.Ignore());

            Mapper.CreateMap<NoticeFormModel, Notice>();
        }
    }
}