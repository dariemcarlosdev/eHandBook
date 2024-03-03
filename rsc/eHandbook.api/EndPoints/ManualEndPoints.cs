using eHandbook.Infrastructure.CrossCutting.Utilities.Filters;
using eHandbook.modules.ManualManagement.Application.Contracts;
using eHandbook.modules.ManualManagement.Application.CQRS.Commands.CreateManual;
using eHandbook.modules.ManualManagement.Application.CQRS.Commands.DeleteManual;
using eHandbook.modules.ManualManagement.Application.CQRS.Commands.DeleteManualById;
using eHandbook.modules.ManualManagement.Application.CQRS.Commands.SoftDeleteManualById;
using eHandbook.modules.ManualManagement.Application.CQRS.Commands.UpdateManual;
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


            /* The MapGroup extension method help me to organize groups of similar endpoints with a common prefix.
            It reduces repetitive code and allows for customizing entire groups of endpoints with a single call to methods like RequireAuthorization
            and WithMetadata which add endpoint metadata. */


            //-------Bellow Here I add the same EndPointfilter and Filter Factory (MyCustomValidationFilterFactory) to all the endpoints created under rootEndPointsGroup group.------

            //var rootEndPointsGroup = app.MapGroup("api/V2/manual/").WithTags("GetManualById");
            //rootEndPointsGroup
            //    .AddEndpointFilter<MyValidationFilters>() //adding customFilter
            //    .AddEndpointFilterFactory(MyValidationGenericFactoryFilter.ValidationFilterFactory); //Adding filter factory.



            //Setting MinimalApis EndPoints with logical handlers in each RuteHandlerBuilder ext. method(e. app.MapGet(), app.MapPut()..). Learn more about Minimal API at https://tinyurl.com/MinimalAPI

            //-------------------------------------------Get Manual by ID End_Points.----------------------------------------------------------------------------------------

            //Use [Validate] attribute for using FilterFactory.
            //Model data binding FromRoute: Parameter of this action are bound from Route(URL) data.
            //HTTP request GET method https://developer.mozilla.org/en-US/docs/Web/HTTP/Methods/GET
            app.MapGet("api/V2/manuals/{Id:Guid}", async ([FromRoute(Name = "Id")] Guid Id, HttpRequest req, IMediator mediator) =>
            {
                //if (string.IsNullOrEmpty(Id.ToString().ToUpper()))
                //{
                //    throw new BadHttpRequestException("id is required");
                //}
                //var getManual = new GetManualByIdQuery { Id = ManualId };
                //var manual = await mediator.Send(getManual);

                //Now here I am using defined Records Querry
                GetManualByIdQueryRec GetManualRecord = new GetManualByIdQueryRec(Id);

                var response = await mediator.Send(GetManualRecord);

                if (response == null)
                {
                    return Results.Problem(detail: "The request was successfully processes, there's no content to return though", statusCode: 204);

                }
                else
                {
                    if (response.Data == null)
                    {
                        //StatusCode#202:The request has been accepted for processing, but the processing has not been completed.
                        //return Results.Accepted("", response);
                        //StatusCode#204:The server has successfully fulfilled the request and that there is no additional content to send in the response payload body.
                        //we’re still getting the whole response back, but the status code is 404 Not Found.
                        return Results.NotFound(response);
                    }
                }
                //Status response code ref: https://developer.mozilla.org/en-US/docs/Web/HTTP/Status/200#status
                return Results.Ok(response);
            })
                .WithTags("Manual")
                .WithName("GetManualById")
                .WithOpenApi(generatedOperation =>
                {

                    var parameter = generatedOperation.Parameters[0];
                    parameter.Description = "The Manual Id bound from request.";
                    parameter.AllowEmptyValue = false;
                    generatedOperation.Summary = "Minimal API Endpoint to get an existing Manual resource.";
                    generatedOperation.Description = "Retrieve a Manual using using CQRS + Mediator Patterns to encapsulate and segregate http request/response" +
                                                     ",we can achieve this injecting to delegate handler service type IMediator (Resolvable Type), registered with the DI in our Service Container.This is how we use DI here." +
                                                     "This applies for the rest of our Minimal APIs EndPoints.";
                    generatedOperation.Tags = new List<OpenApiTag>() {
                        //new() { Name = "GetManualById",Description="Get an existing Manual." }
                        new() { Name = "API V2.0"}
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


            //Some parameters binding options: [parameter, BindingSource] = id:route value,page:query string,customHeader:header,service:Provided by dependency injection
            //Model data binding FromRoute: Parameter of this action are bound from Route(URL) data.
            app.MapGet("api/V1/manuals/{Id}", async (Guid Id, [FromServices] IManualService manualService, [FromQuery(Name = "p")] int p, [FromHeader(Name = "X-CUSTOM-HEADER")] string customHeade, CancellationToken cancellationToken) =>
            {
                var result = await manualService.GetManualByIdAsync(Id, cancellationToken);

                return Results.Ok(result);

            })
                .WithName("GetManualById_V1")
                .WithOpenApi(generatedOperation =>
                {
                    var parameter = generatedOperation.Parameters[0];
                    parameter.Description = "The Manual Id bound from request.";
                    parameter.AllowEmptyValue = false;
                    generatedOperation.Summary = "Minimal API Endpoint to get an existing Manual Resource.";
                    generatedOperation.Description = "Retrieve a Manual from db injecting Business Service Layer in Delegate handler.";
                    generatedOperation.Tags = new List<OpenApiTag>()
                    {
                        //new() { Name = "GetManualById",Description="Get an existing Resource type Manual." },
                        new() { Name = "API V1.0"}
                    };
                    return generatedOperation;
                });

            //--------------------------------------------Get all manuals End_Points.---------------------------------------------------------------------------------------

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
                    generatedOperation.Summary = "Minimal API Endpoint to fetch all Manuals resources.";
                    generatedOperation.Description = "Retrieve all Manuals from db using Aplication Business Service Layer.";
                    generatedOperation.Tags = new List<OpenApiTag>()
                    {
                        //new() { Name = "GetAllManuals",Description="Get all Manuals Resources." },
                        new() { Name = "API V1.0"
                        }
                    };
                    return generatedOperation;
                });

            //HTTP GET request method  ref:https://developer.mozilla.org/en-US/docs/Web/HTTP/Methods/GET
            app.MapGet("api/V2/manuals/", async (IMediator mediator) =>
            {
                var getManuals = new GetManualsQuery();
                var response = await mediator.Send(getManuals);

                if (response == null)
                {
                    return Results.Problem(detail: "The request was successfully processes, with empty response though", statusCode: 204);
                }
                //ref: https://developer.mozilla.org/en-US/docs/Web/HTTP/Status/200#status
                return Results.Ok(response);

            })
                .WithTags("Manuals")
                .WithName("GetAllManuals")
                .WithOpenApi(generatedOperation =>
                {
                    //var parameter = generatedOperation.Parameters[0];
                    //parameter.Description = "No parameters";
                    generatedOperation.Summary = "Minimal API Endpoint to fetch all Manuals resources.";
                    generatedOperation.Description = "Retrieve All Manuals using using CQRS + Mediator Patterns to encapsulate and segregate http request/response" +
                                                     ",we can achieve this injecting to delegate handler service type IMediator (Resolvable Type), registered with the DI in our Service Container.This is how we use DI here." +
                                                     "This applies for the rest of our Minimal APIs EndPoints.";
                    generatedOperation.Tags = new List<OpenApiTag>()
                    {
                        //new() { Name = "GetAllManuals",Description="Get all Manuals Resources." },
                        new() { Name = "API V2.0"}
                     };
                    return generatedOperation;
                });

            //--------------------------------------------Create a new Manual EndPoints.---------------------------------------------------------------------------------------

            //Model data binding FromBody: Parameter of this action are bound from request body.
            app.MapPost("api/V1/manuals/create", async ([FromBody] ManualToCreateDto manualCreateDto, [FromServices] IManualService manualService, CancellationToken cancellation) =>
             {

                 var result = await manualService.AddNewManualAsync(manualCreateDto, cancellation);

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
                 .WithName("CreateManual_v1")
                 .WithOpenApi(generatedOperation =>
                 {
                     //var parameter1 = generatedOperation.Parameters[0];
                     //var parameter2 = generatedOperation.Parameters[1];
                     //parameter1.Description = "A description for Manual.";
                     //parameter2.Description = "The path where Manual will be Storage.";
                     generatedOperation.Summary = "Minimal API Endpoint to add a new Manual resource.";
                     generatedOperation.Description = "Create a new Manual injecting Business Layer.";
                     generatedOperation.Tags = new List<OpenApiTag>()
                     {
                        //new() { Name = "CreateNewManual",Description="Adding new Resource type Manual." },
                        new() { Name = "API V1.0"}
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

            //Model data binding FromBody: Parameter of this action are bound from request body.
            //The HTTP POST method ref:https://developer.mozilla.org/en-US/docs/Web/HTTP/Methods/post
            app.MapPost("api/V2/manuals/create", async ([FromBody] ManualToCreateDto manualCreateDto, IMediator mediator) =>
            {
                var newManual = new CreateManualCommand(manualCreateDto);
                var response = await mediator.Send(newManual);
                if (response == null)
                {
                    return Results.Problem(detail: "The request was successfully processes, there's no Contect to return though", statusCode: 204);
                }
                else
                {
                    if (response.Data == null)
                    {
                        //StatusCode#202:The request has been accepted for processing, but the processing has not been completed.
                        //return Results.Accepted("", response);
                        //StatusCode#204:The server has successfully fulfilled the request and that there is no additional content to send in the response payload body.
                        //we’re still getting the whole response back, but the status code is 404 Not Found.
                        return Results.NotFound(response);
                    }
                }
                //ref: https://developer.mozilla.org/en-US/docs/Web/HTTP/Status/201
                return Results.Created(" ", response);


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
                .WithTags("Manuals")
                .WithName("CreateManual")
                .WithOpenApi(generatedOperation =>
                {
                    //var parameter1 = generatedOperation.Parameters[0];
                    //var parameter2 = generatedOperation.Parameters[1];
                    //parameter1.Description = "A description for Manual.";
                    //parameter2.Description = "The path where Manual will be Storage.";
                    generatedOperation.Summary = "Minimal API Endpoint to add a new Manual resource.";
                    generatedOperation.Description = "Create a new manual using with CQRS + Mediator Patterns" +
                                                     "by injecting to the RouteMethod's delegate handler service type IMediator (Resolvable Type), registered with the DI in our Service Container" +
                                                     ".This is how we use DI here." + "This applies for the rest of our Minimal APIs EndPoints.";
                    generatedOperation.Tags = new List<OpenApiTag>()
                    {
                        //new() { Name = "CreateNewManual",Description="Adding new Resource type Manual" },
                        new() { Name = "API V2.0"}
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

            //----------------------------------------------Update an exisitng Manual Endpoints-------------------------------------------------------------------------------------


            app.MapPut("api/V1/manuals/update", async ([FromBody] ManualToUpdateDto manualUpdateDto, [FromServices] IManualService manualService, CancellationToken cancellation) =>
            {
                var response = await manualService.UpdateManualAsyn(manualUpdateDto, cancellation);

                if (response == null)
                {
                    return Results.Problem(detail: "The request was successfully processes, with empty response though", statusCode: 204);

                }
                //Return a 200 OK if the update request was successful and returns the updated resource.
                return Results.Ok(response);
            })
                .WithName("UpdateManual_v1")
                .WithOpenApi(generatedOperation =>
                {
                    //var parameter1 = generatedOperation.Parameters[0];
                    //var parameter2 = generatedOperation.Parameters[1];
                    //parameter1.Description = "A new description for Manual.";
                    //parameter2.Description = "The new path where Manual will be Storage.";
                    generatedOperation.Summary = "Minimal API Endpoint to update an exisiting Manual resource.";
                    generatedOperation.Description = "Update an existing Manual uInjecting Business Service Layer from ManualManagement Module.";
                    generatedOperation.Tags = new List<OpenApiTag>()
                    {
                        //new() { Name = "UpdateManual",Description = "Upadate existing Resource type Manual, entirely." },
                        new() { Name = "API V1.0"}
                    };
                    return generatedOperation;
                })

                //This factory pattern is useful to register a filter that depends on the signature of the target endpoint handler.I wanted to verify that the handler an endpoint filter
                //is attached to, has a first parameter that evaluates to a UpdateManualDto type.

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


            //Model data binding FromBody: Parameter of this action are bound from request body.
            //HTTP PUT Request method ref:https://developer.mozilla.org/en-US/docs/Web/HTTP/Methods/put
            app.MapPut("api/V2/manuals/update", async ([FromBody] ManualToUpdateDto manualToUpdateDto, IMediator mediator) =>
            {
                var manualToUpdate = new UpdateManualCommand(manualToUpdateDto);
                var response = await mediator.Send(manualToUpdate);
                if (response == null)
                {
                    return Results.Problem(detail: "The request was successfully processes, with empty response though", statusCode: 204);
                }

                else
                {
                    if (response.Data == null)
                    {
                        //StatusCode#202:The request has been accepted for processing, but the processing has not been completed.
                        //return Results.Accepted("", response);
                        //StatusCode#204:The server has successfully fulfilled the request and that there is no additional content to send in the response payload body.
                        //we’re still getting the whole response back, but the status code is 404 Not Found.
                        return Results.NotFound(response);
                    }
                }

                //Return a 200 OK if the update request was successful and returns the updated resource.the target resource does have a current representation and that representationis successfully modified
                //ref: https://developer.mozilla.org/en-US/docs/Web/HTTP/Status/200#status, https://git.fitko.de/fit-connect/submission-api/-/issues/98
                return Results.Ok(response);

            })
                .WithTags("Manuals")
                .WithName("UpdateManual")
                .WithOpenApi(generatedOperation =>
                {

                    generatedOperation.Summary = "Minimal API Endpoint to update an existing Manual resource.";
                    generatedOperation.Description = "Update a manual using  CQRS + Mediator Patterns" +
                                                     "by injecting to the RouteMethod's delegate handler service type IMediator (Resolvable Type), registered with the DI in our Service Container" +
                                                     ".This is how we use DI here." + "This applies for the rest of our Minimal APIs EndPoints.";
                    generatedOperation.Tags = new List<OpenApiTag>() {
                        //new() { Name = "UpdateManual",Description="Updating an existing Resource type Manual, entirely." },
                        new() { Name = "API V2.0"}
                    };
                    return generatedOperation;
                });

            //.AddEndpointFilter(async (context, next) =>
            // {

            //     var description = (string?)context.Arguments[0];
            //     if (string.IsNullOrWhiteSpace(description))
            //     {
            //         return Results.Problem("Empty TODO description not allowed!");
            //     }
            //     return await next(context);
            // });


            //----------------------------------------------SoftDelete an existing Manual Endpoints-------------------------------------------------------------------------------------


            app.MapPut("api/V1/manuals/delete/{Id}", async ([FromRoute(Name = "Id")] Guid id, [FromServices] IManualService manualService, CancellationToken cancellation) =>
            {
                var response = await manualService.SoftDeleteManualByIdAsync(id, cancellation);
                if (response == null)
                {
                    return Results.Problem(detail: "The request was successfully processes, with empty response though", statusCode: 204);
                }

                return Results.Ok(response);
            })
                .WithName("SoftDeleteManual_v1")
                .WithOpenApi(generatedOperation =>
                {
                    var parameter1 = generatedOperation.Parameters[0];
                    parameter1.Description = "The Manual Id.";
                    generatedOperation.Summary = "Minimal API Endpoint to Logical Delete a Manual resource, updating prop. isDeleted to True. No hard delete a Manual from db.";
                    generatedOperation.Description = "Soft Delete Manual by Id from Route using Aplication Business Service Layer from ManualManagement Module.";
                    generatedOperation.Tags = new List<OpenApiTag>()
                    {
                        //new() { Name = "SoftDeleteManual",Description = "Marks a Manual resource as no longer active or valid without actually deleting it from the database" },
                        new() { Name = "API V1.0"}
                    };
                    return generatedOperation;
                });

            //Model data binding FromRoute: Parameter of this action are bound from Route(URL) data.
            app.MapPut("api/V2/manuals/delete/{Id:Guid}", async ([FromRoute(Name = "Id")] Guid id, IMediator mediator) =>
            {
                var manualToDelete = new SoftDeleteManualByIdCommand(id);

                var response = await mediator.Send(manualToDelete);
                if (response == null)
                {
                    return Results.Problem(detail: "The request was successfully processes, with empty response though", statusCode: 204);
                }
                else
                {
                    if (response.Data == null)
                    {
                        //StatusCode#202:The request has been accepted for processing, but the processing has not been completed.
                        //return Results.Accepted("", response);
                        //StatusCode#204:The server has successfully fulfilled the request and that there is no additional content to send in the response payload body.
                        //we’re still getting the whole response back, but the status code is 404 Not Found.
                        return Results.NotFound(response);
                    }
                }
                //Return a 200 OK if the update request was successful and returns the updated resource.the target resource does have a current representation and that representationis successfully modified
                //ref: https://developer.mozilla.org/en-US/docs/Web/HTTP/Status/200#status, https://git.fitko.de/fit-connect/submission-api/-/issues/98
                return Results.Ok(response);
            })
                .WithTags("Manuals")
                .WithName("SoftDeleteManual")
                .WithOpenApi(generatedOperation =>
                {
                    var parameter1 = generatedOperation.Parameters[0];
                    parameter1.Description = "The Manual Id.";
                    generatedOperation.Summary = "Minimal API Endpoint to delete(Logical) an existing Manual resource. updating prop. isDeleted to True. No hard delete a Manual from db.";
                    generatedOperation.Description = "Mark a manual as no active (IsDeleted) using CQRS + Mediator Patterns" +
                                                     "by injecting to the RouteMethod's delegate handler service type IMediator (Resolvable Type), registered with the DI in our Service Container" +
                                                     ".This is how we use DI here." + "This applies for the rest of our Minimal APIs EndPoints.";
                    generatedOperation.Tags = new List<OpenApiTag>()
                    {
                        //new() { Name = "SoftDeleteManual", Description = "marks a manual resource as no longer active or valid without actually deleting it from the database." },
                        new() { Name = "API V2.0"}
                    };
                    return generatedOperation;
                });



            //----------------------------------------------Delete an exisiting Manual EndPoints-------------------------------------------------------------------------------------


            app.MapDelete("api/V1/manuals/delete", async ([FromBody] ManualToDeleteDto manualDeleteDto, [FromServices] IManualService manualService, CancellationToken cancellation) =>
            {
                var response = await manualService.DeleteManualAsync(manualDeleteDto, cancellation);
                if (response == null)
                {
                    return Results.Problem(detail: "The request was successfully processes, with empty response though", statusCode: 204);


                }
                return Results.NoContent();
            })
                .WithName("HardDeleteManual_v1")
                .WithOpenApi(generatedOperation =>
                {
                    generatedOperation.Summary = "Minimal API Endpoint to permanently delete an existing Manual resource from db injecting ManualService Layer.";
                    generatedOperation.Description = "Delete Manual using Aplication Business Service Layer from ManualManagement Module.";
                    generatedOperation.Tags = new List<OpenApiTag>()
                    { 
                        //new() { Name = "HardDeleteManual", Description = "Delete a Manual resource from data base permanently" },
                        new() { Name = "API V1.0"}
                    };
                    return generatedOperation;
                });



            //HTTP Delete Request Method Ref: https://developer.mozilla.org/en-US/docs/Web/HTTP/Methods/DELETE
            //Model data binding FromBody: Parameter of this action are bound from request body.
            app.MapDelete("api/V2/manuals/delete", async ([FromBody] ManualToDeleteDto manualDeleteDto, IMediator mediator) =>
            {
                var manualToDelete = new DeleteManualCommand(manualDeleteDto);

                var response = await mediator.Send(manualToDelete);

                if (response == null)
                {
                    return Results.Problem(detail: "The request was successfully processes, with empty response though", statusCode: 204);

                }
                else
                {
                    if (response.Data == null)
                    {
                        //StatusCode#202:The request has been accepted for processing, but the processing has not been completed.
                        //return Results.Accepted("", response);
                        //StatusCode#204:The server has successfully fulfilled the request and that there is no additional content to send in the response payload body.
                        //we’re still getting the whole response back, but the status code is 404 Not Found.
                        return Results.NotFound(response);
                    }
                }

                //Ref: https://developer.mozilla.org/en-US/docs/Web/HTTP/Status/204
                //return Result.NoContent();
                return Results.Ok(response);

                //return Results.NoContent() ;
            }).WithTags("Manuals")
                .WithName("HardDeleteManual")
                .WithOpenApi(generatedOperation =>
                {
                    generatedOperation.Summary = "Minimal API Endpoint to hard delete(Physical) an existing Manual resource from data base.";
                    generatedOperation.Description = "Delete a Manual Permanently using CQRS + Mediator Patterns" +
                                                     "by injecting to the RouteMethod's delegate handler service type IMediator (Resolvable Type), registered with the DI in our Service Container" +
                                                     ".This is how we use DI here." + "This applies for the rest of our Minimal APIs EndPoints.";

                    generatedOperation.Tags = new List<OpenApiTag>()
                    {
                        //new() { Name = "HardDeleteManual", Description = "Delete a Manual resource from data base permanently." },
                        new() { Name = "API V2.0"}
                    };
                    return generatedOperation;
                });

            //----------------------------------------------Delete an exisiting Manual by Guid EndPoint_V2-------------------------------------------------------------------------------------

            //HTTP Delete Request Method Ref: https://developer.mozilla.org/en-US/docs/Web/HTTP/Methods/DELETE
            //Model data binding FromRoute: Parameter of this action are bound from Route(URL) data.
            app.MapDelete("api/V2/manuals/deleteBy/{Id:Guid}", async ([FromRoute(Name = "Id")] Guid id, IMediator mediator) =>
            {
                var manualToDelete = new DeleteManualByIdCommand(id);

                var response = await mediator.Send(manualToDelete);

                if (response == null)
                {
                    return Results.Problem(detail: "The request was successfully processes, there's no content to return though", statusCode: 204);

                }

                else
                {
                    if (response.Data == null)
                    {
                        //StatusCode#202:The request has been accepted for processing, but the processing has not been completed.
                        //return Results.Accepted("", response);
                        //StatusCode#204:The server has successfully fulfilled the request and that there is no additional content to send in the response payload body.
                        //we’re still getting the whole response back, but the status code is 404 Not Found.
                        return Results.NotFound(response);
                    }
                }
                //Ref: https://developer.mozilla.org/en-US/docs/Web/HTTP/Status/204
                //return Result.NoContent();
                return Results.Ok(response);
            })
                .WithTags("Manuals")
                .WithName("DeleteManualById")
                .WithOpenApi(generatedOperation =>
                {
                    generatedOperation.Summary = "Minimal API Endpoint to hard delete(Physical) an existing Manual by Id from database.";
                    generatedOperation.Description = "Delete a Manual given Id Permanently using CQRS + Mediator Patterns" +
                                                     "by injecting to the RouteMethod's delegate handler service type IMediator (Resolvable Type), registered with the DI in our Service Container" +
                                                     ".This is how we use DI here." + "This applies for the rest of our Minimal APIs EndPoints.";

                    generatedOperation.Tags = new List<OpenApiTag>()
                    {
                        new() { Name = "API V2.0"}
                    };
                    return generatedOperation;
                });

        }
    }
}

