#region Using Directives

using AutoMapper;

#endregion



namespace Tattoo.Web.Mappings
{
    public class AutoMapperSetup
    {
        public static void Configure()
        {
            Mapper.Initialize(conf =>
            {
                conf.AddProfile<D2VModelMappingProfile>();
                conf.AddProfile<VM2DomainMappingProfile>();
            });
        }
    }
}