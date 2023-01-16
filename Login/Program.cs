using Login.CustomMiddleware; 

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseLoginCustomMiddleware(); 

app.Run(async context =>
{
    await context.Response.WriteAsync("\nEnd");
}); 

app.Run();

