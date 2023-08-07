using InstrumentService.Data;
using Microsoft.EntityFrameworkCore;

namespace InstrumentService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorPages().AddRazorRuntimeCompilation(); 

            string connection = builder.Configuration.GetConnectionString("DefaultConnection");

            builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(connection));
            //AddHttpContextAccessor - ���������� HttpContextAccessor �������� �����������, ������ ��� 
            //���������� �������� � ������� ���������� HTTP
            //� ��������������� ���� �� ������������ ������ � AddSession ��� ������������ ���������� ������, ��� �����
            //����������� ������� � HttpContext ��� ��������� ��������, ��������� � �������.
            builder.Services.AddHttpContextAccessor();
            //���������� ��������� ������
            builder.Services.AddSession(Options =>
            {
                Options.IdleTimeout = TimeSpan.FromMinutes(10);
                //������������� HttpOnly � �������� true, ��� �� cookie ������������ ������ ��� �������
                //� �� ����� ���� �������� ����� ���������� ������ (��������, JavaScript)
                //��� ������������ �������������� ������ �� ������������ �����������, ����� ��� ����� CSRF (����������� �������� ��������)
                Options.Cookie.HttpOnly = true;
                //IsEssential = true ��� ��������, ��� ���������� cookies ������ ���� ������ ���������� � ���������,
                //���� ���� ������������ �������� ������������� cookies � ����� ��������. ��� �����, ������ ��� ���������� cookies ������������
                //��� ����������� ��� ������ ����������� �������
                Options.Cookie.IsEssential = true;
            });
            builder.Services.AddControllersWithViews();
            

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();
            //���������� middleware ������. ���������� ������, �������� ���������� ������������ �������� ������ ��� �������� ������,
            //��������� � ������������ �������������, �� ���������� ���������� HTTP-��������.
            app.UseSession();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}