﻿using eHandbook.Infrastructure.CrossCutting.Utilities.Filters;
using eHandbook.modules.ManualManagement.Application.Contracts;
using eHandbook.modules.ManualManagement.Application.CQRS.Commands.CreateManual;
using eHandbook.modules.ManualManagement.Application.CQRS.Queries.GetManual;
using eHandbook.modules.ManualManagement.Application.CQRS.Queries.GetManuals;
using eHandbook.modules.ManualManagement.CoreDomain.DTOs.Manual;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;

namespace eHandbook.api.EndPoints
{
    /// <summary>
    /// My EndPoints definitions for ManualManagement, using Minimal APIs approach. Used to create lightweight services that expose functionalities to clients.
    /// They'll handle incoming HTTP requests and process them to perform specific tasks, such as retrieving data, manipulating data, or performing some business logic.
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
            // The MapGroup extension method help me to organize groups of similar endpoints with a common prefix. It reduces repetitive code and allows for customizing entire groups of endpoints with a single call to methods like RequireAuthorization and WithMetadata which add endpoint metadata.
            // Bellow Here I add the same EndPointfilter and Filter Factory (MyCustomValidationFilterFactory) to all the endpoints created under rootEndPointsGroup group.

            //var rootEndPointsGroup = app.MapGroup("api/V2/manual/").WithTags("GetManualById");
            //rootEndPointsGroup
            //    .AddEndpointFilter<MyValidationFilters>() //adding customFilter
            //    .AddEndpointFilterFactory(MyValidationGenericFactoryFilter.ValidationFilterFactory); //Adding filter factory.

            //Setting MinimalApis EndPoints with logical handlers in each RuteHandlerBuilder ext. method(e. app.MapGet(), app.MapPut()..). Learn more about Minimal API at https://tinyurl.com/MinimalAPI

            //Get Manual by ID End_Point_V1.
            //Use [Validate] attribute for using FilterFactory.
            app.MapGet("api/V2/manuals/{Id}", async (Guid Id, HttpRequest req, IMediator mediator) =>
            {
                //if (string.IsNullOrEmpty(Id.ToString().ToUpper()))
                //{
                //    throw new BadHttpRequestException("id is required");
                //}
                //var getManual = new GetManualByIdQuery { Id = ManualId };
                //var manual = await mediator.Send(getManual);

                //Now here I am using defined Records Querry
                GetManualByIdQueryRec GetManualRecord = new GetManualByIdQueryRec(Id);
                var manual = await mediator.Send(GetManualRecord);

                return TypedResults.Ok(manual);
            })
                //API Enpoint document. in Swagger.
                .WithName("GetManualById_V2")
                .WithOpenApi(generatedOperation =>
                {
                    var parameter = generatedOperation.Parameters[0];
                    parameter.Description = "The Manual Id bound from request.";
                    parameter.AllowEmptyValue = false;
                    generatedOperation.Summary = "Minimal API Endpoint to get an existing Manual.";
                    generatedOperation.Description = "Retrieve a Manual using using CQRS + Mediator Patterns to encapsulate and segregate http request/response" +
                                                     ",we can achieve this injecting to delegate handler service type IMediator (Resolvable Type), registered with the DI in our Service Container.This is how we use DI here." +
                                                     "This applies for the rest of our Minimal APIs EndPoints.";
                    generatedOperation.Tags = new List<OpenApiTag>() {
                        new() { Name = "GetManualById",Description="Get an existing Manual." }
                      };
                    return generatedOperation;
                });
            //.AddEndpointFilter(async (ctx, next) =>
            //{
            //    var Id = ctx.Arguments[0];

            //    if (Id is null)
            //    {
            //        throw new Exception();
            //    }

            //    return await next(ctx);

            //    });
            //adding IEnpointFilters defined in Shared Infrastructure Filters.

            //.AddEndpointFilter<MyFilters>();
            //.AddEndpointFilter<MyValidationGenericFilter<Guid>>(); //does not working
            //.AddEndpointFilterFactory(MyValidationGenericFactoryFilter.ValidationFilterFactory); //does not working


            //Get manual by ID End_Point_V1.
            //Some parameters binding options: [parameter, BindingSource] = id:route value,page:query string,customHeader:header,service:Provided by dependency injection
            app.MapGet("api/V1/manuals/{Id}", async ([FromRoute] Guid Id, [FromServices] IManualService manualService, [FromQuery(Name = "p")] int p, [FromHeader(Name = "X-CUSTOM-HEADER")] string customHeade) =>
            {
                var result = await manualService.GetManualByIdAsync(Id);

                return Results.Ok(result);

            })
                .WithName("GetManualById_V1")
                .WithOpenApi(generatedOperation =>
                {
                    var parameter = generatedOperation.Parameters[0];
                    parameter.Description = "The Manual Id bound from request.";
                    parameter.AllowEmptyValue = false;
                    generatedOperation.Summary = "Minimal API Endpoint to get an existing Manual.";
                    generatedOperation.Description = "Retrieve a Manual from db injecting Business Service Layer in Delegate handler.";
                    generatedOperation.Tags = new List<OpenApiTag>() {
                        new() { Name = "GetManualById",Description="Get an existing Manual." }
                    };
                    return generatedOperation;
                });

            //Get all manuals End_Point_V1.
            app.MapGet("api/V1/manuals/", async ([FromServices] IManualService manualService) =>
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

            })
                .WithName("GetAllManuals_V1")
                .WithOpenApi(generatedOperation =>
                {
                    //var parameter = generatedOperation.Parameters[0];
                    //parameter.Description = "No parameters";
                    generatedOperation.Summary = "Minimal API Endpoint to fetch all Manuals.";
                    generatedOperation.Description = "Retrieve all Manuals from db using Aplication Business Service Layer.";
                    generatedOperation.Tags = new List<OpenApiTag>() {
                        new() { Name = "GetAllManuals",Description="Get Manuals." }
                    };
                    return generatedOperation;
                });

            //Get all manuals End Point_V2.
            app.MapGet("api/V2/manuals/", async (IMediator mediator) =>
            {
                var getManuals = new GetManualsQuery();
                var response = await mediator.Send(getManuals);

                if (response == null)
                {
                    return Results.NotFound($"EndPoint Response: Error fetching all manuals from Db.");
                }

                #region data members Not Used.
                #endregion

                return Results.Ok(response);

            })
                .WithName("GetAllManuals_V2")
                .WithOpenApi(generatedOperation =>
                {
                    //var parameter = generatedOperation.Parameters[0];
                    //parameter.Description = "No parameters";
                    generatedOperation.Summary = "Minimal API Endpoint to fetch all Manuals.";
                    generatedOperation.Description = "Retrieve All Manuals using using CQRS + Mediator Patterns to encapsulate and segregate http request/response" +
                                                     ",we can achieve this injecting to delegate handler service type IMediator (Resolvable Type), registered with the DI in our Service Container.This is how we use DI here." +
                                                     "This applies for the rest of our Minimal APIs EndPoints.";
                    generatedOperation.Tags = new List<OpenApiTag>() {
                        new() { Name = "GetAllManuals",Description="Get Manuals." }
                    };
                    return generatedOperation;
                });

            //Create a new manual EndPoint_V1.
            app.MapPost("api/V1/manuals/create", async ([FromBody] ManualToCreateDto manualCreateDto, [FromServices] IManualService manualService) =>
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
            })
                .WithName("CreateManual")
                .WithOpenApi(generatedOperation =>
                {
                    //var parameter1 = generatedOperation.Parameters[0];
                    //var parameter2 = generatedOperation.Parameters[1];
                    //parameter1.Description = "A description for Manual.";
                    //parameter2.Description = "The path where Manual will be Storage.";
                    generatedOperation.Summary = "Minimal API Endpoint to add a new Manual.";
                    generatedOperation.Description = "Create a new Manual injecting Business.";
                    generatedOperation.Tags = new List<OpenApiTag>() {
                        new() { Name = "CreateNewManual",Description="Adding new Manual" }
                    };
                    return generatedOperation;
                })
                //Here I am only going to construct and add a filter to the endpoint if the signature of the endpoint delegate contains the required argument type
                //This approach still suffers from the fact that we need to call AddEndpointFilterFactory on each endpoint. to use our defined generic Filter.
                .AddEndpointFilterFactory((filterFactoryContext, next) =>
                {
                    //cheking the MethodInfo of the endpoint and then attach new instance of generic MyCustomValidationFilter.
                    var isTypeOf = filterFactoryContext.MethodInfo.GetParameters().Any(p => p.ParameterType == typeof(ManualToCreateDto));
                    if (isTypeOf)
                    {
                        var myCustomValidationfilter = new MyValidationGenericFilter<ManualToCreateDto>();
                        return invocationContext => myCustomValidationfilter.InvokeAsync(invocationContext, next);
                    }

                    //if the signature of the endpoint delegate does not contain the required argument type(CreateManualDto), pass - thru filter
                    return invocationContext => next(invocationContext);
                });

            //Create a new manual EndPoint_V2.
            app.MapPost("api/V2/manuals/create", async ([FromBody] ManualToCreateDto manualCreateDto, IMediator mediator) =>
            {
                var newManual = new CreateManualCommand(manualCreateDto);
                var response = await mediator.Send(newManual);
                if (response == null)
                {
                    return Results.NotFound($"EndPoint Respose: Error adding new manual {manualCreateDto}");
                }
                return TypedResults.Ok(response);


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
            })
                .WithName("CreateManual_v2")
                .WithOpenApi(generatedOperation =>
                {
                    //var parameter1 = generatedOperation.Parameters[0];
                    //var parameter2 = generatedOperation.Parameters[1];
                    //parameter1.Description = "A description for Manual.";
                    //parameter2.Description = "The path where Manual will be Storage.";
                    generatedOperation.Summary = "Minimal API Endpoint to add a new Manual.";
                    generatedOperation.Description = "Create a new using with CQRS + Mediator Patterns" +
                                                     "by injecting to the RouteMethod's delegate handler service type IMediator (Resolvable Type), registered with the DI in our Service Container" +
                                                     ".This is how we use DI here." + "This applies for the rest of our Minimal APIs EndPoints.";
                    generatedOperation.Tags = new List<OpenApiTag>() {
                        new() { Name = "CreateNewManual",Description="Adding new Manual" }
                    };
                    return generatedOperation;
                })
                //Here I am only going to construct and add a filter to the endpoint if the signature of the endpoint delegate contains the required argument type
                //This approach still suffers from the fact that we need to call AddEndpointFilterFactory on each endpoint. to use our defined generic Filter.
                .AddEndpointFilterFactory((filterFactoryContext, next) =>
                {
                    //cheking the MethodInfo of the endpoint and then attach new instance of generic MyCustomValidationFilter.
                    var isTypeOf = filterFactoryContext.MethodInfo.GetParameters().Any(p => p.ParameterType == typeof(ManualToCreateDto));
                    if (isTypeOf)
                    {
                        var myCustomValidationfilter = new MyValidationGenericFilter<ManualToCreateDto>();
                        return invocationContext => myCustomValidationfilter.InvokeAsync(invocationContext, next);
                    }

                    //if the signature of the endpoint delegate does not contain the required argument type(CreateManualDto), pass - thru filter
                    return invocationContext => next(invocationContext);
                });


            //Update an exisitng manual Endpoint.
            app.MapPut("api/V1/manuals/update", async ([FromBody] ManualToUpdateDto manualUpdateDto, [FromServices] IManualService manualService) =>
            {
                var response = await manualService.UpdateManualAsyn(manualUpdateDto);

                if (response == null)
                {
                    return Results.NotFound($"EndPoint Response:Error updating Muanual resourse to Db.");

                }

                return Results.Ok(response);
            })
                .WithName("UpdateManual")
                .WithOpenApi(generatedOperation =>
                {
                    //var parameter1 = generatedOperation.Parameters[0];
                    //var parameter2 = generatedOperation.Parameters[1];
                    //parameter1.Description = "A new description for Manual.";
                    //parameter2.Description = "The new path where Manual will be Storage.";
                    generatedOperation.Summary = "Minimal API Endpoint to update an exisiting Manual.";
                    generatedOperation.Description = "Update an existing Manual using Aplication Business Service Layer from ManualManagement Module.";
                    generatedOperation.Tags = new List<OpenApiTag>() { new() { Name = "UpdateManual" } };
                    return generatedOperation;
                })
                //This factory pattern is useful to register a filter that depends on the signature of the target endpoint handler.I wanted to verify that the handler an endpoint filter is attached to, has a first parameter that evaluates to a UpdateManualDto type.
                .AddEndpointFilterFactory((filterFactoryContext, next) =>
                {
                    //EndpointFilterFactoryContext object provides access to the MethodInfo associated with the endpoint's handler and caching some of the information provided in the MethodInfo in a filter.
                    var param = filterFactoryContext.MethodInfo.GetParameters();
                    //The signature of the handler is examined by inspecting MethodInfo for the expected type signature.
                    if (param.Length > 0 && param[0].ParameterType == typeof(ManualToUpdateDto))
                    {
                        //If the expected signature is found(if evaluation is True), the validation filter is registered onto the endpoint. 
                        return async invocationContext =>
                        {
                            var updateManualParam = invocationContext.GetArgument<ManualToUpdateDto>(0);

                            if (updateManualParam == null)
                            {
                                return Results.Problem("The Request argument is not valid.");
                            }
                            return await next(invocationContext);
                        };
                    }
                    //If a matching signature isn't found, then a pass-through filter is registered.
                    return invocationContext => next(invocationContext);
                });

            //SoftDelete an existing Manual EndPoint.
            app.MapDelete("api/V1/manuals/delete/{Id}", async ([FromRoute] Guid id, [FromServices] IManualService manualService) =>
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
            })
                .WithName("SoftDeleteManual")
                .WithOpenApi(generatedOperation =>
                {
                    var parameter1 = generatedOperation.Parameters[0];
                    parameter1.Description = "The Manual Manual Id.";
                    generatedOperation.Summary = "Minimal API Endpoint to update the prop. isDeleted to True.No hard delete a Manual from db.";
                    generatedOperation.Description = "Soft Delete Manual by Id from Route using Aplication Business Service Layer from ManualManagement Module.";
                    generatedOperation.Tags = new List<OpenApiTag>() { new() { Name = "SoftDeleteManual" } };
                    return generatedOperation;
                });

            //Delete an exisiting Manual EndPoint.
            app.MapDelete("api/V1/manuals/delete", async ([FromBody] ManualToDeleteDto manualDeleteDto, [FromServices] IManualService manualService) =>
            {
                var response = await manualService.DeleteManualAsync(manualDeleteDto);
                if (response == null)
                {
                    return Results.NotFound($"EndPoint Response: Error Adding new Muanual Record to Db.");


                }
                return Results.Ok(response);
            })
                .WithName("HardDeleteManual")
                .WithOpenApi(generatedOperation =>
                {
                    generatedOperation.Summary = "Minimal API Endpoint to hard delete an existing Manual from db using ManualService Layer.";
                    generatedOperation.Description = "Delete Manual using Aplication Business Service Layer from ManualManagement Module.";
                    generatedOperation.Tags = new List<OpenApiTag>() { new() { Name = "HardDeleteManual" } };
                    return generatedOperation;
                });

        }
    }
}

