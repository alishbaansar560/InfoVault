using AutoMapper;
using INFOVUALT.DTOs;
using INFOVUALT.Models;

namespace INFOVUALT.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CreateFolderDto, Folder>();

            CreateMap<CreateNoteDto, Note>();

            CreateMap<RegisterDto, User>();
        }
    }
}