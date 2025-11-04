using System;
using System.Threading.Tasks;
using ChecktonPld.RestAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace ChecktonPld.RestAPI.Controllers.Implementation;

/// <inheritdoc />
public class ChecktonPldApi: ChecktonPldApiControllerBase
{
    /// <inheritdoc />
    public override async Task<IActionResult> PostValidaConRenapo(string version, ValidacionCurpRequest body)
    {
        return null;
    }
}