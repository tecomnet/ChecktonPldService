using System.Net;
using System.Text;
using Newtonsoft.Json;
using ChecktonPld.RestAPI.Models;
using ChecktonPld.UnitTest.FixtureBase;
using InlineResponse400 = ChecktonPld.RestAPI.Models.InlineResponse400;

namespace ChecktonPld.UnitTest.IntegrationTest;

public class UserApiTest : DatabaseTestFixture
{
    // Api URI
    private const string API_URI = "users";

    // Api version
    private const string API_VERSION = "0.1";


    public UserApiTest()
    {
        SetupDataAsync(async context =>
        {
            // Create data
            var commonSettings = new CommonSettings();
            // Add data
            //await context.AddRangeAsync(commonSettings.Users);
            // Save changes
            await context.SaveChangesAsync();
        }).GetAwaiter().GetResult();
    }

    #region Private methods
    /*
    /// <summary>
    /// Validate the properties  
    /// </summary>
    /// <param name="result">The result</param>
    private void ValidateProperties(UserResult result)
    {
        // Validate type of properties 
        Assert.IsType<string>(result.User);
        // Verify the type of common properties
        Assert.IsType<Guid>(result.Guid);
        Assert.IsType<DateTime>(result.ModificationTimestamp);
        Assert.IsType<Guid>(result.ModificationUser);
        Assert.IsType<DateTime>(result.CreationTimestamp);
        Assert.IsType<Guid>(result.CreationUser);
    }

    /// <summary>
    /// Create string content
    /// </summary>
    /// <param name="body">any object</param>
    /// <returns>String content</returns>
    private StringContent CreateContent(object body)
    {
        // Serialize the object request
        var json = JsonConvert.SerializeObject(body);
        // Create content
        var content = new StringContent(
            content: json,
            encoding: Encoding.UTF8,
            mediaType: "application/json");
        // Return content
        return content;
    }


    /// <summary>
    /// Get the config result from database
    /// </summary>
    /// <returns>TransaccionesConfigResult</returns>
    private async Task<UserResult?> GetTransactionConfigFromDataBase()
    {

        // Create client
        var client = Factory.CreateAuthenticatedClient();
        // Call api method
        var response = await client.GetAsync(
            requestUri: $"{API_VERSION}/{API_URI}");
        // Read the content
        var responseContentString = await response.Content.ReadAsStringAsync();
        // Is ok status
        if (response.StatusCode != HttpStatusCode.OK)
            return null;
        else
        {
            // Deserialize the object
            var result =
                JsonConvert.DeserializeObject<UserResult>(responseContentString);
            // Return the result
            return result;
        }
    }

    /// <summary>
    /// Clear all the ApiKeyChannel
    /// </summary>
    private async Task ClearTransactionConfig()
    {
        // Context
        var context = CreateContext();
        // Remueve la config de transaction
        //await context.KeyValueConfig.OfType<TransactionsKeyValueConfig>().ExecuteDeleteAsync();
        // Save changes
        await context.SaveChangesAsync();
    }

    /// <summary>
    /// Assert the properties sent
    /// </summary>
    /// <param name="body">body request</param>
    /// <param name="result">ApiKeyChannel result</param>
    private void AssertProperties(UserRequest body, UserResult result)
    {
        // Assert key value config properties 
        Assert.Equal(expected: body.User, actual: result.User);
    }

    /// <summary>
    /// Assert the properties sent
    /// </summary>
    /// <param name="body">body request</param>
    /// <param name="result">api key channel result</param>
    private void AssertProperties(UserUpdateRequest body, UserResult result)
    {
        // Assert key value config properties 
        Assert.Equal(expected: body.User, actual: result.User);
    }
    */
    #endregion
}