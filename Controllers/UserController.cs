﻿using Dapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReadEase_C_.Services;
using RestSharp;
using System.Security.Claims;
using WebApplication1.Models;
using ReadEase_C_.Helpers;
using ReadEase_C_.Models;
using Books.Services;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace userController.Controllers
{
    [ApiController]
    [Route("api")]
    public class UsersController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly UserService _userService;
        private readonly HashingService _hashingService;
        private readonly PhotoService _photoService;
        private readonly Mail _mail;
        private readonly BookService _bookService;
        private readonly ConnectionService _connectionService;

        public UsersController(IConfiguration configuration, UserService service, HashingService hashingService, PhotoService photoService, Mail mail, BookService bookService, ConnectionService CS)
        {
            _configuration = configuration;
            _userService = service;
            _hashingService = hashingService;
            _photoService = photoService;
            _mail = mail;
            _bookService = bookService;
            _connectionService = CS;
        }



        [HttpGet("lookup")]
        [Authorize]
        public IActionResult Lookup()
        {
            var isAuthenticated = User?.Identity?.IsAuthenticated ?? false;
            var role = User?.FindFirstValue(ClaimTypes.Role);

            var username = _userService.getUsername(UserId);

            var response = new
            {
                IsAuthenticated = isAuthenticated,
                Role = role,
                Username = username
            };

            return Ok(response);
        }


        [HttpPost("logout")]
        public IActionResult Logout()
        {
            Response.Cookies.Delete(".AspNetCore.Cookies", new CookieOptions
            {
                Path = "/",
                Secure = true,
                HttpOnly = true,
                SameSite = SameSiteMode.None
            });


            return Ok("Logout successful");
        }


        [HttpPost("register")]
        public async Task<IActionResult> PostUser([FromBody] UserModel model)
        {
            var connection = _connectionService.GetConnection();


            if (model.Password.Length < 3)
            {
                return Conflict("Password is too short!");
            }
            // Retrieve salt asynchronously
            string salt = await _hashingService.GenerateSaltAsync();

            // Hash the password with retrieved salt
            string hashedPassword = await _hashingService.GenerateSaltedHash(model.Password, salt);

            // Check if the user with the same username already exists
            string checkUserQuery = "SELECT COUNT(*) FROM Users WHERE Username = @Username";
            int existingUser = await connection.QueryFirstOrDefaultAsync<int>(checkUserQuery, new { Username = model.Username });

            if (existingUser > 0)
            {
                // User with the same username already exists, handle accordingly
                return Conflict("Username already exists!");
            }

            // Insert the data into the Users table and retrieve the generated Id using Dapper
            string insertQuery = "INSERT INTO Users (Username, Password, Salt, Role) OUTPUT INSERTED.Id VALUES (@Username, @Password, @Salt, 'User')";
            int userId = await connection.QueryFirstOrDefaultAsync<int>(insertQuery, new { Username = model.Username, Password = hashedPassword, Salt = salt });

            // Set the generated Id in the UserModel
            model.Id = userId;
            string role = await _userService.CheckIfUserIsAdminAsync(model.Id);

            // Create claims for the registered user
            List<Claim> claims = [
            new Claim(ClaimTypes.NameIdentifier, model.Id.ToString()),
            new Claim(ClaimTypes.Name, model.Username),
            new Claim(ClaimTypes.Role, role ?? "User")];
            // Add additional claims as needed

            var authProperties = new AuthenticationProperties
            {
                // Persist the cookie even after the browser is closed
                IsPersistent = true
            };
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var principal = new ClaimsPrincipal(identity);

            // Sign in the user after successful registration
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, authProperties);
            model.Role = role ?? "User";

            // Send the registration success email
            _mail.SendEmail(model.Username);
            // Return the user's role after successful registration
            return Ok(model.Role);

        }


        [HttpPost("login")]
        public async Task<ActionResult> LoginUser([FromBody] UserModel model)
        {

            var connection = _connectionService.GetConnection();

            // Retrieve the salt for the user
            string salt = await _hashingService.GetSalt(model.Username, connection);

            if (salt == null)
            {
                // User not found, return 401 Unauthorized
                return Conflict("User does not exist!");
            }

            // Hash the password with retrieved salt
            string hashedPassword = await _hashingService.GenerateSaltedHash(model.Password, salt);

            string selectQuery = "SELECT * FROM Users WHERE Username = @Username AND Password = @Password";
            var user = await connection.QueryFirstOrDefaultAsync<UserModel>(selectQuery, new { Username = model.Username, Password = hashedPassword });




            if (user != null)
            {
                // Retrieve the role from the database
                string role = await _userService.CheckIfUserIsAdminAsync(user.Id);

                // Create claims for the authenticated user
                List<Claim> claims =
                [
                new (ClaimTypes.NameIdentifier, user.Id.ToString()),
                new (ClaimTypes.Name, user.Username),
                new (ClaimTypes.Role, role ?? "User")];

                var authProperties = new AuthenticationProperties
                {
                    // Persist the cookie even after the browser is closed
                    IsPersistent = true
                };
                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var principal = new ClaimsPrincipal(identity);

                // Sign in the user
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, authProperties);

                user.Role = role ?? "User";
                return Ok(user.Role);
            }
            else
            {
                // Return 401 Unauthorized
                return Conflict("Incorrect password!");
            }

        }



        [HttpPost("recover")]
        public async Task<ActionResult> PasswordRecovery([FromBody] RecoveryMail mail)
        {
            var connection = _connectionService.GetConnection();


            string countQuery = "SELECT COUNT(*) FROM USERS WHERE Username=@Username ";
            int user = connection.QueryFirstOrDefault<int>(countQuery, new { Username = mail.email });

            if (user == 0)
            {
                return Conflict("User does not exist!");
            }

            string salt = await _hashingService.GetSalt(mail.email, connection);
            string randowPass = _hashingService.GenerateRandomPassword(6);
            string hashedPass = await _hashingService.GenerateSaltedHash(randowPass, salt);
            string insertQuery = "UPDATE USERS SET Password=@Value1 WHERE Username=@Username ";
            connection.Execute(insertQuery, new { Username = mail.email, Value1 = hashedPass });

            _mail.RecoverEmail(mail.email, randowPass);
            return Ok("Your new password is sent to your e-mail!");
        }




        [HttpPost("photo")]
        [Authorize]
        public IActionResult Photo(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file is uploaded.");

            // Check if the uploaded file is a valid image format
            string[] allowedImageTypes = { "image/jpeg", "image/png" }; // JPEG and PNG only
            if (!allowedImageTypes.Contains(file.ContentType))
                return BadRequest("Invalid file format. Only JPEG and PNG formats are allowed.");

            using (var memoryStream = new MemoryStream())
            {
                file.CopyTo(memoryStream);

                // Resize the image while maintaining aspect ratio
                using var image = SixLabors.ImageSharp.Image.Load(memoryStream.ToArray());
                // Calculate resizing dimensions while maintaining aspect ratio
                int width = image.Width;
                int height = image.Height;
                int maxSize = 460;

                if (width > height)
                {
                    height = (int)Math.Round((double)height / width * maxSize);
                    width = maxSize;
                }
                else
                {
                    width = (int)Math.Round((double)width / height * maxSize);
                    height = maxSize;
                }

                // Resize the image
                image.Mutate(x => x.Resize(width, height));

                // Save the resized image with appropriate compression settings
                using var resizedMemoryStream = new MemoryStream();
                image.SaveAsJpeg(resizedMemoryStream, new SixLabors.ImageSharp.Formats.Jpeg.JpegEncoder
                {
                    Quality = 90 // Adjust quality as needed (0-100)
                });
                _photoService.InsertPhoto(UserId, resizedMemoryStream.ToArray());
            }

            return Ok("Photo uploaded successfully.");
        }

        [HttpGet("getphoto")]
        [Authorize]
        public IActionResult GetPhoto()
        {


            byte[] photoBytes = _photoService.GetPhoto(UserId);

            if (photoBytes == null)
                return NotFound("No photo found for this user.");

            return File(photoBytes, "image/jpeg"); // adjust the content type based on your image format
        }





        public int UserId
        {
            get
            {
                return Int32.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            }
        }

        [HttpGet("role")]
        [Authorize]
        public async Task<ActionResult> GetRole()
        {
            string role = await _userService.CheckIfUserIsAdminAsync(UserId);

            return Ok(role);
        }



        [HttpPut("update")]
        [Authorize]
        public async Task<IActionResult> UpdateUser([FromBody] updateCredentialsModal user)
        {
            var connection = _connectionService.GetConnection();


            string countUsers = "SELECT COUNT(*) FROM USERS WHERE Username=@Username";
            int count = connection.QueryFirst<int>(countUsers, new { Username = user.UpdatedUsername });

            if (count > 0)
            {
                return Conflict("Email alredy in use!");

            }


            string userEmail = _userService.getUsername(UserId);

            string salt = await _hashingService.GetSalt(userEmail, connection);

            if (!string.IsNullOrEmpty(user.UpdatedPassword))
            {
                string hashedPass = await _hashingService.GenerateSaltedHash(user.UpdatedPassword, salt);
                await _userService.UpdateUserAsync(user.UpdatedUsername, hashedPass, UserId);
            }
            else
            {
                await _userService.UpdateUserAsync(user.UpdatedUsername, null, UserId);
            }
            return Ok("Username and/or password updated successfully");
        }





    }
}
