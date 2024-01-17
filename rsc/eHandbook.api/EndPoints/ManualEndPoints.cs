using eHandbook.modules.ManualManagement.Application.Contracts;
using eHandbook.modules.ManualManagement.Application.CQRS.Queries.GetManual;
using eHandbook.modules.ManualManagement.CoreDomain.DTOs.Manual;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using static eHandbook.modules.ManualManagement.CoreDomain.Validations.FluentValidation.ManualRequestValidatorsContainer;

namespace eHandbook.api.EndPoints
{
    /// <summary>
    /// Manual Minimal APIs endpoints. Used to create lightweight services that expose functionalities to clients.
    /// They handle incoming HTTP requests and process them to perform specific tasks, such as retrieving data, manipulating data, or performing some business logic.
    /// These APIs essentially represent a service layer responsible for handling external requests and providing responses.
    /// </summary>
    public static class ManualEndPoints
    {

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="app"></param>
        public static void MapManualEndPoints(this IEndpointRouteBuilder app)
        {
            //Setting a Minimal Api for Service and Data Access Layer testing purpose. Learn more about Minimal API at https://tinyurl.com/MinimalAPI

            //Get manual by given id using CQRS 
            app.MapGet("api/V2/manual/{ManualId}", async (Guid ManualId, IMediator mediator) =>
            {
                
                var getManual = new GetManualByIdQuery { Id = ManualId };

                //using Records Querry
                GetManualByIdQueryRec GetManualRecord = new GetManualByIdQueryRec(ManualId);
               

                var manual = await mediator.Send(getManual);
                var manualrec = await mediator.Send(GetManualRecord);

                return TypedResults.Ok(manualrec);
            })
                .WithName("GetManualByIdV2")
                .WithOpenApi(generatedOperation =>
                {
                    var parameter = generatedOperation.Parameters[0];
                    parameter.Description = "The id associated with Manual.";
                    generatedOperation.Summary = "MInimal API Endpoint using CQRS  + MediatR Patterns.";
                    return generatedOperation;
                });
            

            //Get manual by given id
            app.MapGet("api/V1/manual/{Id}", async (Guid Id, IManualService manualService) =>
            {
                var result = await manualService.GetManualByIdAsync(Id);

               return  Results.Ok(result);

            })
                .WithName("GetManualById")
                .WithOpenApi( generatedOperation => 
                {
                    var parameter = generatedOperation.Parameters[0];
                    parameter.Description = "The id associated with Manual.";
                    generatedOperation.Summary = "Minimal API Endpoint.";
                    return generatedOperation;
                });

            //Get all manuals End Point.
            app.MapGet("api/V1/manual/get-all", async (IManualService manualService) =>
            {

                var response = await manualService.GetAllManualsAsync();
                //var manual = await dbContext
                //.Set<ManualEntity>()
                //.AsNoTracking()
                //.FirstOrDefaultAsync(m => m.Id == manualId);

                if (response == null)
                {
                    return Results.NotFound($"EndPoint Response: Error fetching all manuals from Db.");
                }

                #region data members Not Used.
                //MinimalApiResponse helper class not used anymore.
                // var helperResponder = new MinimalApiResponse(response.Data!.Id.ToString(), response.Data.Description!, response.Data.Path!);
                #endregion

                return Results.Ok(response);

            });

            //Create a new manual EndPoint.
            app.MapPost("api/V1/manual/create", async ([FromBody] CreateManualDto manualCreateDto, IManualService manualService) =>
            {         

                var result = await manualService.AddNewManualAsync(manualCreateDto);

                return TypedResults.Ok(result);
                
               
                //return result != null ? Results.Ok(result) : Results.Problem(
                // statusCode: StatusCodes.Status400BadRequest,
                // title: "Bad Request",
                // //URI pointing to somewhere where api consumer can get more details about this failure
                // type: "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                // //create an extension object to extend problems details with arbitrary values, here I am creating a Dictionary extension object type.
                // extensions: new Dictionary<string, object?>
                // {
                //       {"errors", new[] { result.Error} }
                // });
            });

            //Update an exisitng manual Endpoint.
            app.MapPut("api/V1/manual/update", async ([FromBody] UpdateManualDto manualUpdateDto, [FromServices] IManualService manualService) =>
            {
                var response = await manualService.UpdateManualAsyn(manualUpdateDto);

                if (response == null)
                {
                    return Results.NotFound($"EndPoint Response:Error updating Muanual resourse to Db.");

                }

                return Results.Ok(response);
            });

            //SoftDelete an existing Manual EndPoint.
            app.MapDelete("api/V1/manual/delete/{Id}", async (Guid id, IManualService manualService) =>
            {


                var response = await manualService.SoftDeleteManualAsync(id);
                //var manual = await dbContext
                //.Set<ManualEntity>()
                //.AsNoTracking()
                //.FirstOrDefaultAsync(m => m.Id == manualId);

                if (response == null)
                {
                    return Results.NotFound($"EndPoint Response:Error Adding new Muanual Record to Db.");


                }

                #region data members Not Used.
                //MinimalApiResponse helper class not used anymore.
                // var helperResponder = new MinimalApiResponse(response.Data!.Id.ToString(), response.Data.Description!, response.Data.Path!);
                #endregion

                return Results.Ok(response);
            });

            //Delete an exisiting Manual EndPoint.
            app.MapDelete("api/V1/manual/delete", async ([FromBody] DeleteManualDto manualDeleteDto, [FromServices] IManualService manualService) =>
            {
                var response = await manualService.DeleteManualAsync(manualDeleteDto);

                if (response == null)
                {
                    return Results.NotFound($"EndPoint Response: Error Adding new Muanual Record to Db.");


                }
                return Results.Ok(response);
            });

        }
    }
}

