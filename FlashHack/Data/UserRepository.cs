using FlashHack.Data.DataInterfaces;
using FlashHack.Models;
using Microsoft.EntityFrameworkCore;

namespace FlashHack.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext applicationDbContext;

        public UserRepository(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }

        //  Lägg till en ny användare
        public async Task AddAsync(User user)
        {
            await applicationDbContext.AddAsync(user);
            await applicationDbContext.SaveChangesAsync();
        }

        //  Ta bort en användare
        public async Task Delete(User user)
        {
            applicationDbContext.Remove(user);
            await applicationDbContext.SaveChangesAsync();
        }

        //  Hämta alla användare
        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await applicationDbContext.User
                .Include(u => u.Skills)  // 🟡 Inkludera Skills för listan
                .ToListAsync();
        }

        //  Hämta en användare baserat på ID, inkl. Skills
        public async Task<User?> GetByIdAsync(int id)
        {
            return await applicationDbContext.User
                .Include(u => u.Skills)  // 🟡 Viktigt! Inkludera Skills här
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        //  Uppdatera användarinformation och Skills
        public async Task Update(User user)
        {
            var existingUser = await applicationDbContext.User
                .Include(u => u.Skills)  // 🟢 Inkludera Skills för korrekt uppdatering
                .FirstOrDefaultAsync(u => u.Id == user.Id);

            if (existingUser != null)
            {
                //  Uppdatera användarens basdata
                applicationDbContext.Entry(existingUser).CurrentValues.SetValues(user);

                //  Hantera färdigheter
                existingUser.Skills = user.Skills ?? new List<Skill>();

                await applicationDbContext.SaveChangesAsync();
            }
        }

        public async Task RemoveSkillAsync(Skill skill)
        {
            applicationDbContext.Skill.Remove(skill);
            await applicationDbContext.SaveChangesAsync();
        }


    }
}
