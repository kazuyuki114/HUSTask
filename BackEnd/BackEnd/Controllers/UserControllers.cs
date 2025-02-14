using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BackEnd.Models.Entities; 
using BackEnd.Data;
using BackEnd.Models;

namespace BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly WebDbContext _dbContext;
        private readonly ILogger<UsersController> _logger;

        public UsersController(WebDbContext  dbContext, ILogger<UsersController> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }
        
        // Add a new account
        [HttpPost("signup")]
        public async Task<IActionResult> SignUp([FromBody] SignUpModel signUpModel)
        {
            // Check if the email already exists
            var existingUser = await _dbContext.Users
                .FirstOrDefaultAsync(u => u.Email == signUpModel.Email);

            if (existingUser != null)
            {
                return BadRequest("Email already in use.");
            }
            
            // Create new user
            var currentTime = DateTime.UtcNow.AddHours(7);
            var newUser = new User
            {
                Name = signUpModel.Name,
                Email = signUpModel.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(signUpModel.Password), // Hash password before storing
                CreatedAt = currentTime,
                UpdatedAt = currentTime    
            };

            _dbContext.Users.Add(newUser);
            await _dbContext.SaveChangesAsync(); 

            // Create exactly one contact associated with the new user
            var newContact = new Contact
            {
                Name = signUpModel.Name,
                Email = signUpModel.Email,
                Gender = signUpModel.Gender,
                Occupation = signUpModel.Occupation,
                Phone = signUpModel.Phone,
                Notes = signUpModel.Notes,
                Dob = signUpModel.DoB,
                UserId = newUser.Id 
            };

            _dbContext.Contacts.Add(newContact); 
            await _dbContext.SaveChangesAsync(); 

            return Ok(new { UserId = newUser.Id, ContactId = newContact.Id });
        }
        
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserById(int userId)
        {
            try
            {
                // Fetch user information along with their associated contact based on UserId as a foreign key
                var user = await _dbContext.Users
                    .FirstOrDefaultAsync(u => u.Id == userId);

                if (user == null)
                {
                    return NotFound(new { Message = "User not found." });
                }
                // Fetch the contact associated with the user
                var contact = await _dbContext.Contacts
                    .FirstOrDefaultAsync(c => c.UserId == userId);

                // Return user details along with contact information
                return Ok(new
                {
                    UserId = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    CreatedAt = user.CreatedAt,
                    UpdatedAt = user.UpdatedAt,
                    Contact = contact != null
                        ? new
                        {
                            ContactId = contact.Id,
                            Name = contact.Name,
                            Email = contact.Email,
                            Gender = contact.Gender,
                            Occupation = contact.Occupation,
                            Phone = contact.Phone,
                            Notes = contact.Notes,
                            Dob = contact.Dob
                        }
                        : null // Handle case where no contact exists
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving user information.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            try
            {
                // Fetch all users from the database but exclude the password field
                var users = await _dbContext.Users
                    .Select(user => new 
                    {
                        user.Id,
                        user.Name,
                        user.Email,
                        // Add any other fields you want to include, excluding the password
                    })
                    .ToListAsync();
                
                // If no users found, return a 404 Not Found response
                if (users == null || users.Count == 0)
                {
                    return NotFound("No users found.");
                }

                return Ok(users);
            }
            catch (Exception ex)
            {
                // Handle any errors and return a 500 Internal Server Error response
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpPut("{userId}")]
        public async Task<IActionResult> UpdateUserInfo(int userId, [FromBody] UpdateUserModel updateUserModel)
        {
            try
            {
                // Find the user by ID
                var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
                if (user == null)
                {
                    return NotFound(new { Message = "User not found." });
                }

                // Update basic user information
                user.Name = updateUserModel.Name ?? user.Name;
                user.Email = updateUserModel.Email ?? user.Email;
                user.UpdatedAt = DateTime.UtcNow.AddHours(7); // Update timestamp

                // Find the contact associated with the user
                var contact = await _dbContext.Contacts.FirstOrDefaultAsync(c => c.UserId == userId);
                if (contact != null)
                {
                    // Update contact details if provided
                    contact.Name = updateUserModel.Contact?.Name ?? contact.Name;
                    contact.Email = updateUserModel.Contact?.Email ?? contact.Email;
                    contact.Gender = updateUserModel.Contact?.Gender ?? contact.Gender;
                    contact.Occupation = updateUserModel.Contact?.Occupation ?? contact.Occupation;
                    contact.Phone = updateUserModel.Contact?.Phone ?? contact.Phone;
                    contact.Notes = updateUserModel.Contact?.Notes ?? contact.Notes;
                    contact.Dob = updateUserModel.Contact?.Dob ?? contact.Dob;
                }

                // Save changes to the database
                await _dbContext.SaveChangesAsync();

                return Ok(new { Message = "User information updated successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating user information.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

    }
}
