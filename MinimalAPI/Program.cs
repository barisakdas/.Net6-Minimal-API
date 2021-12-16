// All namespace taken from global namespace class.

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSwaggerGen(options =>   // options for using token with swagger
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Description = "Bearer authentication with jwt token.",
        Type = SecuritySchemeType.Http
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Id = "Bearer",
                    Type = ReferenceType.SecurityScheme
                }
            },
            new List<string>()
        }
    });
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateActor = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["JWT:Issuer"],
        ValidAudience = builder.Configuration["JWT:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]))
    };
});

builder.Services.AddAuthorization();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSingleton<IMovieService, MovieService>();
builder.Services.AddSingleton<IUserService, UserService>();

var app = builder.Build();

app.UseSwagger();
app.UseAuthorization();
app.UseAuthentication();

app.MapGet("/", () => "Hello World!");

app.MapPost("login", (UserLogin userLogin, IUserService userService) => Login(userLogin, userService));
IResult Login(UserLogin userLogin, IUserService userService)
{
    if (!string.IsNullOrEmpty(userLogin.UserName) && !string.IsNullOrEmpty(userLogin.Password))
    {
        var user = userService.Get(userLogin);

        if (user is null) return Results.NotFound("user not found");

        var claims = new[] {
        new Claim(ClaimTypes.NameIdentifier,user.UserName),
        new Claim(ClaimTypes.Email,user.Email),
        new Claim(ClaimTypes.GivenName,user.FirstName),
        new Claim(ClaimTypes.Surname,user.LastName),
        new Claim(ClaimTypes.Role,user.Role),
        };

        var token = new JwtSecurityToken(
            issuer: builder.Configuration["JWT:Issuer"],
            audience: builder.Configuration["JWT:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(30),
            notBefore: DateTime.UtcNow,
            signingCredentials: new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"])),
                SecurityAlgorithms.HmacSha256)
            );


        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

        return Results.Ok(tokenString);
    }

    return Results.NotFound("user not found");

}

app.MapPost("/create",
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrator")]
(Movie movie, IMovieService movieService) => Create(movie, movieService));
IResult Create(Movie movie, IMovieService movieService) { return Results.Ok(movieService.Create(movie)); }

app.MapGet("/get",
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Standard, Administrator")]
(int id, IMovieService movieService) => GetByID(id, movieService));
IResult GetByID(int id, IMovieService movieService)
{
    var movies = movieService.GetById(id);
    if (movies is null) return Results.NotFound("Data not found!");

    return Results.Ok(movies);
}

app.MapGet("/getall", (IMovieService movieService) => GetAll(movieService));
IResult GetAll(IMovieService movieService) { return Results.Ok(movieService.GetAll()); }

app.MapPost("/update",
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrator")]
(Movie movie, IMovieService movieService) => Update(movie, movieService));
IResult Update(Movie movie, IMovieService movieService)
{
    var updatedMovie = movieService.Update(movie);
    if (updatedMovie is null) return Results.NotFound("Data not found!");
    return Results.Ok(updatedMovie);
}

app.MapGet("/delete",
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrator")]
(int id, IMovieService movieService) => Delete(id, movieService));
IResult Delete(int id, IMovieService movieService)
{
    var result = movieService.Delete(id);
    return result ? Results.Ok() : Results.NotFound();
}

app.UseSwaggerUI();
app.Run();
