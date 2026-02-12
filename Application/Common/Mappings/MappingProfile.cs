using Application.Dtos.Chat;
using Application.Dtos.ChatMessage;
using Application.Dtos.QuickMessage;
using Application.Dtos.User;
using AutoMapper;
using Domain.Models;

namespace Application.Common.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Chat, ChatDto>()
            .ForMember(dest => dest.Users, opt => opt.MapFrom(src => 
                src.Participants.Select(p => p.User)));
        CreateMap<User, UserDto>();
        CreateMap<ChatMessage, ChatMessageDto>();
        CreateMap<QuickMessage, QuickMessageDto>();
    }
}