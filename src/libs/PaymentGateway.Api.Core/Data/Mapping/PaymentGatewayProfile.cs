using AutoMapper;
using Logging.Extensions;
using PaymentGateway.Api.Core.Data.Enums;
using PaymentGateway.Api.Core.Extensions;
using System;

namespace PaymentGateway.Api.Core.Data.Mapping
{
    public class PaymentGatewayProfile : Profile
    {
        public PaymentGatewayProfile()
        {

            CreateMap<Dtos.CardInformation, Model.CardDetails>()
               .ForMember(dest => dest.CardType, m => m.MapFrom(source => source.CardType.HasValue ? Enum.GetName(typeof(CardType), source.CardType.Value) : default))               
               .ForMember(dest => dest.LastFourCard, m => m.MapFrom(source => source.CardNumber.HasValue() ? source.CardNumber.GetLast(4) : default))
               .ForMember(dest => dest.ExpiryMonth, m => m.MapFrom(source => source.ExpiryMonth.HasValue ? source.ExpiryMonth : default))
               .ForMember(dest => dest.ExpiryYear, m => m.MapFrom(source => source.ExpiryYear.HasValue ? source.ExpiryYear : default));

            CreateMap<Dtos.CardInformation, Model.PaymentRecord>()
              .ForMember(dest => dest.Amount, m => m.MapFrom(source => source.Amount.HasValue ? source.Amount.Value : default))
              .ForMember(dest => dest.ClientId, m => m.MapFrom(source => source.ClientId.HasValue() ? source.ClientId : default))
              .ForMember(dest => dest.Currency, m => m.MapFrom(source => source.Currency.HasValue() ? source.Currency : default));


            CreateMap<Model.PaymentRecord, Dtos.Response>()
            .ForMember(dest => dest.PaymentRecordId, m => m.MapFrom(source => source.PaymentRecordId))
            .ForMember(dest => dest.Status, m => m.MapFrom(source => source.Status))
            .ForMember(dest => dest.StatusCode, m => m.MapFrom(source => source.StatusCode));

            CreateMap<Model.PaymentRecord, Dtos.PaymentRecord>().AfterMap((src, dest, context) => dest.Card = context.Mapper.Map<Model.PaymentRecord, Dtos.CardDetails>(src));


            CreateMap<Model.PaymentRecord, Dtos.PaymentRecord>();
            CreateMap<Model.CardDetails, Dtos.CardDetails>();
        }
    }
}
