using System;
using System.Threading.Tasks;
using AutoMapper;
using ChecktonPld.DOM.Enums;
using ChecktonPld.Funcionalidad.Functionality.CurpFacade;
using ChecktonPld.RestAPI.Helpers;
using ChecktonPld.RestAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace ChecktonPld.RestAPI.Controllers.Implementation;

/// <inheritdoc />
public class ChecktonPldApiController(IValidacionCurpFacade facade, IMapper mapper): ChecktonPldApiControllerBase
{
    /// <inheritdoc />
    public override async Task<IActionResult> PostValidaConRenapo(string version, ValidacionCurpRequest body)
    {
        // Obtienes el valor como entero de forma segura.
        int genero = (int)body.Genero;
        // Intentar convertir el entero al tipo Enum
        if (!Enum.IsDefined(typeof(DOM.Enums.Genero), genero))
        {
            //Si es un valor inválido, lanza una excepción de validación o un BadRequest.
            throw new ArgumentException($"El valor {genero} no es un Genero válido.");
        }
        var validacion = await facade.ValidarCurpAsync(
            nombre: body.Nombre,
            primerApellido: body.PrimerApellido,
            segundoApellido: body.SegundoApellido,
            fechaNacimiento: DateOnly.FromDateTime(body.FechaNacimiento.Value),
            genero: (Genero)body.Genero,
            estado: body.Estado,
            nombreServicioCliente: body.NombreServicioCliente,
            creationUser: this.GetAuthenticatedUserGuid()
        );
        // Mapeo a result
        var result = mapper.Map<ValidacionCurpResult>(validacion);
        // Retorna ok
        return Created(uri: $"/{version}/checktonpld", result);
    }
}