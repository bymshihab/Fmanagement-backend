using FarmProject.ConnectionString;
using FarmProject.Data_Process_Logic;
using FarmProject.Manager.Menu;
using FarmProject.Manager.Module;
using FarmProject.Manager.ShedType;
using FarmProject.Manager.Stock;
using FarmProject.Manager.Supplier;
using FarmProject.Manager.Uom;
using FarmProject.Manager.User;
using FarmProject.Manager.UserParmissions;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Hosting.Internal;

var builder = WebApplication.CreateBuilder(args);

//CORS
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
        policy =>
        {
            policy.WithOrigins("*")
            .AllowAnyHeader()
            .AllowAnyMethod();
        });
});
// Add services to the container.

// Configuration
var configuration = builder.Configuration;
builder.Services.AddSingleton(configuration);

// Register business logic, data process, and other services here
builder.Services.AddSingleton<InterfaceUser, UserBusinessLogic>();
builder.Services.AddSingleton<UserDataProcess>();

///==================================================================
builder.Services.AddSingleton<InterfaceUom, UomBusinessLogic>();
builder.Services.AddSingleton<UomDataProcess>();

///==================================================================
builder.Services.AddSingleton<InterfaceSupplier, SupplierBusinessLogic>();
builder.Services.AddSingleton<SuppliersDataProcess>();

///==================================================================
builder.Services.AddSingleton<InterfaceModule, ModuleBusinessLogic>();
builder.Services.AddSingleton<ModuleDataProcess>();

///==================================================================
builder.Services.AddSingleton<InterfaceMenu, MenuBusinessLogic>();
builder.Services.AddSingleton<MenuDataProcess>();

///==================================================================
builder.Services.AddSingleton<InterfaceUserPermission, UserPermissionBusinessLogic>();
builder.Services.AddSingleton<UserPermissionDataProcess>();
///==================================================================
builder.Services.AddSingleton<InterfaceStock, StockBusinessLogic>();
builder.Services.AddSingleton<StockDataProcess>();
///==================================================================
builder.Services.AddSingleton<InterfaceShedType, ShedTypeBusinessLogic>();
builder.Services.AddSingleton<ShedTypeDataProcess>();
//===================================================================
builder.Services.AddSingleton<Connection>();
builder.Services.AddControllers();

//===================================================================


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new Microsoft.Extensions.FileProviders.PhysicalFileProvider(
        Path.Combine(app.Environment.ContentRootPath, "Images")),
    RequestPath = "/Images"
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

//cors
app.UseCors(MyAllowSpecificOrigins);

app.Run();
