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
        CreateMap<ChatMessage, ChatMessageDto>();
        CreateMap<QuickMessage, QuickMessageDto>();
    }
}