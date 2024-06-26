using AutoMapper;
using eHandbook.modules.ManualManagement.CoreDomain.DTOs.Manual;
using eHandbook.modules.ManualManagement.CoreDomain.Entities;

namespace eHandbook.modules.ManualManagement.Application.Mappings
{
    /// <summary>
    /// AutoMapper is a simple library that helps us to transform one object type into another. 
    /// It is a convention-based object-to-object mapper that requires very little configuration. 
    /// </summary>
    public class ManualProfileMap : Profile
    {
        /// <summary>
        /// A good way to organize our mapping configurations is with Profiles. 
        /// We need to create classes that inherit from Profile class and put the configuration in the constructor:
        /// </summary>
        public ManualProfileMap()
        {
            //In this case both POCO objects have same same, in case of diferents props. name, the props. need to be mapped as well.(http://tinyurl.com/yremtj9b)
            CreateMap<ManualEntity, ManualDto>()
                // you should use the .ForPath method instead of .ForMember. The .ForPath method is designed for scenarios where you need to map to a nested property.
                .ForPath(dest => dest.AuditableDetails.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy))
                .ForPath(dest => dest.AuditableDetails.CreatedOn, opt => opt.MapFrom(src => src.CreatedOn))
                .ForPath(dest => dest.AuditableDetails.UpdatedBy, opt => opt.MapFrom(src => src.UpdatedBy))
                .ForPath(dest => dest.AuditableDetails.UpdatedOn, opt => opt.MapFrom(src => src.UpdatedOn))
                .ForPath(dest => dest.AuditableDetails.IsUpdated, opt => opt.MapFrom(src => src.IsUpdated))
                .ForPath(dest => dest.AuditableDetails.DeletedOn, opt => opt.MapFrom(src => src.DeletedOn))
                .ForPath(dest => dest.AuditableDetails.DeletedBy, opt => opt.MapFrom(src => src.DeletedBy))

                //configuring AutoMapper to initialize the nested EntityDetailsDto object if it's null. 
                .AfterMap((src, dest) => {
                    if (dest.AuditableDetails == null)
                    {
                        dest.AuditableDetails = new AuditableDetailsDto();
                    }
                }).ReverseMap();
        }
    }
}
