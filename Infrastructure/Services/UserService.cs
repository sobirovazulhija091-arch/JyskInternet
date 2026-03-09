using System.Net;
using Domain.DTOs;
using Infrastructure.Responses;
using Microsoft.EntityFrameworkCore;
public class UserService(ApplicationDbContext dbContext):IUserService
{
    private readonly ApplicationDbContext context = dbContext;

    public async Task<Response<string>> DeleteAsync(string userid)
    {
       var del = await context.Users.FindAsync(userid);
       if (del == null)
       {
        return new Response<string>(HttpStatusCode.NotFound , "user not found");
       }
       context.Users.Remove(del);
       await context.SaveChangesAsync();
        return new Response<string>(HttpStatusCode.OK , "ok");
    }

    public async  Task<PagedResult<User>> GetAllAsync(FilterUser filter , PagedQuery query)
    {
        IQueryable<User> usersQuery = context.Users.AsQueryable();

    if (filter.Email != null)
        {
          usersQuery = usersQuery.Where(u => u.Email==filter.Email);   
        }

    if (filter.FullName!=null)
    {
        usersQuery = usersQuery.Where(u => u.FullName==filter.FullName);
    }
     var total = await  usersQuery.CountAsync();
     var page = query.Page > 0 ? query.Page : 1;
     var pageSize = query.PageSize > 0 ? query.PageSize : 10;
     usersQuery =  usersQuery.Skip((page-1)*pageSize).Take(pageSize);
    var users = await usersQuery.ToListAsync();

    return new PagedResult<User>
    {
        Items = users,
        Page = page,
        PageSize = pageSize,
        TotalCount = total,
        TotalPages = (int)Math.Ceiling((double)total / pageSize)
    };
    }

    public async Task<Response<User>> GetByIdAsync(string userid)
    {
        var user = await context.Users.FindAsync(userid);
        if (user == null)
        {
            return new Response<User>(HttpStatusCode.NotFound,"user not found");
        }
         return new Response<User>(HttpStatusCode.OK,"ok", user);

    }

    public async Task<Response<string>> UpdateAsync(string userid, UpdateUserDto dto)
    {
        var user = await context.Users.FindAsync(userid);
        if (user == null)
        {
            return new Response<string>(HttpStatusCode.NotFound,"user not found");
        }
            user.FullName = dto.FullName;
             await context.SaveChangesAsync();
            return new Response<string>(HttpStatusCode.OK,"ok" );
    }

}
