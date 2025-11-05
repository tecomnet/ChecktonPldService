using AutoMapper;
using ChecktonPld.DOM.Modelos;
using ChecktonPld.RestAPI.Models;

namespace ChecktonPld.RestAPI.Mappers;

/// <summary>
/// 
/// </summary>
public class AutoMapperProfile : Profile
{
    /// <inheritdoc />
    public AutoMapperProfile()
    {
        // Mapeo de validacion result
        CreateMap<ValidacionCurp, ValidacionCurpResult>();

    }
}