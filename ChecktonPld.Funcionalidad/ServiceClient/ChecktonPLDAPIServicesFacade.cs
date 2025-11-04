using System.ComponentModel.DataAnnotations;
using ChecktonPld.DOM.Errors;
using ChecktonPld.Funcionalidad.Helper;
using ChecktonPld.Funcionalidad.Remoting.REST.ChecktonPldManagement;

namespace ChecktonPld.Funcionalidad.ServiceClient;

internal static class ChecktonConfigurationSettingsData
{
    internal const string ServiceName = "ChecktonPLDAPIServices"; 
    internal const string Version = "0.1";
    internal const string RemoteServiceNameConfig = "checkton-pld-base-uri";
    internal const string ServiceErrorCode = "EM-INCORRECT-AUTHORIZATION-TYPE";
}

public interface IChecktonPLDAPIServicesFacade
{
    Task<Response3> ValidacionChecktonRenapo(string curp);
   
}

public class ChecktonPLDAPIServicesFacade(
    IServiceProvider serviceProvider,
    UrlBuilder urlBuilder)
    : ServiceFacadeBase(urlBuilder: urlBuilder,
        runningServiceName: ChecktonConfigurationSettingsData.ServiceName,
        runningModuleName: nameof(ChecktonPLDAPIServicesFacade),
        remoteServiceNameConfig: ChecktonConfigurationSettingsData.RemoteServiceNameConfig,
        version: ChecktonConfigurationSettingsData.Version), IChecktonPLDAPIServicesFacade
{
    
    
    private ChecktonPLDAPIServices BuildLocalServiceClientApiKey()
    {
        // Get api key
        var apiKey = Environment.GetEnvironmentVariable("api-key-checkton");
        // Build service client
        return BuildServiceClient(
            authorizationType: AuthorizationType.API_KEY,
            authorization: apiKey,
            serviceErrorCode: ChecktonConfigurationSettingsData.ServiceErrorCode,
            init: (client, baseUrl) => new ChecktonPLDAPIServices(client)
            {
                BaseUrl = baseUrl
            });
    }
    protected override EMGeneralAggregateException? ExtractEMGeneralAggregateException(Exception exception)
    {
  
        // generalmente solo habrá una excepción agregada.
        List<EMGeneralException> exceptions = new();

        // 3. Mapear la ChecktonApiException a la estructura EMGeneralException
        // La propiedad 'Message' de checktonEx contiene el detalle completo del error de la API.
        exceptions.Add(new EMGeneralException(
            // Usamos el mensaje ya formateado que viene del cliente
            message: exception.Message, 
            // Usamos un código de error interno genérico para API PLD
            code: "CHECKTON-PLD-API-SERVICES-ERROR", 
            // Título descriptivo para este tipo de error
            title: "Error en el Servicio Externo Checkton PLD", 
            // Detalle completo
            description: exception.Message, 
            serviceName: "Checkton PLD Service",
            module: this.GetType().Name,
            serviceInstance: "N/A",
            serviceLocation: "N/A"
        ));
    
        // 4. Retornar la excepción agregada
        return new EMGeneralAggregateException(exceptions: exceptions);
    }

    public async Task<Response3> ValidacionChecktonRenapo(string curp)
    {
        try
        {
            var serviceClient = BuildLocalServiceClientApiKey();
            var requestBody = new ValidateCurpRequest()
            {
                Curp = curp
            };
            var response = await serviceClient.ValidateAsync(body: requestBody);
            return response;
        }
        catch (Exception e)
        {
            throw HandelAPIException(e);
        }
    }
}