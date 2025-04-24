using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.StaticFiles;
using XiaoYiSharp_BlazorApp.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddAntDesign();
builder.Services.AddSingleton<XiaoYi_AgentService>();

var app = builder.Build();

//¾²Ì¬ wwwroot
//Ìí¼ÓMIME
FileExtensionContentTypeProvider provider = new FileExtensionContentTypeProvider();
provider.Mappings[".log"] = "application/octet-stream";
provider.Mappings[".vue"] = "application/octet-stream";
provider.Mappings[".mat"] = "application/octet-stream";
provider.Mappings[".pqy"] = "application/octet-stream";
provider.Mappings[".ply"] = "application/octet-stream";
provider.Mappings[".exr"] = "application/octet-stream";
provider.Mappings[".epicraw"] = "application/octet-stream";
provider.Mappings[".moc"] = "application/octet-stream";
provider.Mappings[".mtn"] = "application/octet-stream";
app.UseStaticFiles(new StaticFileOptions
{
    ContentTypeProvider = provider
});

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
