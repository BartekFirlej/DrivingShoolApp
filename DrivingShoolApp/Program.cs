using DrivingSchoolApp;
using DrivingSchoolApp.Repositories;
using DrivingSchoolApp.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IAddressRepository, AddressRepository>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<ICustomerLectureRepository, CustomerLectureRepository>();
builder.Services.AddScoped<ILicenceCategoryRepository, LicenceCategoryRepository>();
builder.Services.AddScoped<IClassroomRepository, ClassroomRepository>();
builder.Services.AddScoped<ICourseTypeRepository, CourseTypeRepository>();
builder.Services.AddScoped<ICourseRepository, CourseRepository>();
builder.Services.AddScoped<ICourseSubjectRepository, CourseSubjectRepository>();
builder.Services.AddScoped<ILecturerRepository, LecturerRepository>();
builder.Services.AddScoped<ILectureRepository, LectureRepository>();
builder.Services.AddScoped<IDrivingLicenceRepository, DrivingLicenceRepository>();
builder.Services.AddScoped<IDrivingLessonRepository, DrivingLessonRepository>();
builder.Services.AddScoped<IRegistrationRepository, RegistrationRepository>();
builder.Services.AddScoped<IRequiredLicenceCategoryRepository, RequiredLicenceCategoryRepository>();
builder.Services.AddScoped<ISubjectRepository, SubjectRepository>();
builder.Services.AddScoped<IAddressService, AddressService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<ICustomerLectureService, CustomerLectureService>();
builder.Services.AddScoped<ILicenceCategoryService, LicenceCategoryService>();
builder.Services.AddScoped<IClassroomService, ClassroomService>();
builder.Services.AddScoped<ICourseTypeService, CourseTypeService>();
builder.Services.AddScoped<ICourseService, CourseService>();
builder.Services.AddScoped<ICourseSubjectService, CourseSubjectService>();
builder.Services.AddScoped<ILecturerService, LecturerService>();
builder.Services.AddScoped<ILectureService, LectureService>();
builder.Services.AddScoped<IDrivingLicenceService, DrivingLicenceService>();
builder.Services.AddScoped<IDrivingLessonService, DrivingLessonService>();
builder.Services.AddScoped<IRegistrationService, RegistrationService>();
builder.Services.AddScoped<IRequiredLicenceCategoryService, RequiredLicenceCategoryService>();
builder.Services.AddScoped<ISubjectService, SubjectService>();  
builder.Services.AddScoped<IDateTimeHelper, DateTimeHelper>();

builder.Services.AddDbContext<DrivingSchoolDbContext>();

builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(typeof(DrivingSchoolMapper));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html"); ;

app.Run();
