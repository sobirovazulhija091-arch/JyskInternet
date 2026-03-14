using Domain.DTOs;
using Infrastructure.Data;
using Infrastructure.Responses;
using Microsoft.EntityFrameworkCore;
using System.Net;
public class ReviewService(ApplicationDbContext dbContext):IReviewService
{
     private readonly ApplicationDbContext context = dbContext;

    public async Task<Response<string>> AddReviewAsync(ReviewDto dto)
    {
       var product = await context.Products.FirstOrDefaultAsync(p => p.Id == dto.ProductId);
       if (product == null)
       {
        return new Response<string>(HttpStatusCode.NotFound,"Product not found");
       }
       if (string.IsNullOrWhiteSpace(dto.UserId))
       {
        return new Response<string>(HttpStatusCode.BadRequest,"UserId is required");
       }
       if (dto.Rating < 1 || dto.Rating > 5)
       {
        return new Response<string>(HttpStatusCode.BadRequest,"Rating must be between 1 and 5");
       }

       var review =  new Review
       {
        ProductId=dto.ProductId,
        UserId = dto.UserId,
        Rating=dto.Rating,
        Comment=dto.Comment
       };
        await context.Reviews.AddAsync(review);
        await context.SaveChangesAsync();
        return new Response<string>(HttpStatusCode.OK,"Add successfully!");
    }

    public async Task<PagedResult<Review>> GetAllAsync(FilterReview filter, PagedQuery query)
    {
       IQueryable<Review> reviews = context.Reviews.AsNoTracking();
        if (filter.ProductId != null)
        {
            reviews = reviews.Where(r=>r.ProductId==filter.ProductId);
        }
         if (filter.UserId != null)
        {
            reviews = reviews.Where(r=>r.UserId==filter.UserId);
        }
        if (filter.Rating.HasValue)
         {
         reviews  = reviews.Where(r => r.Rating == filter.Rating);
         }
          var total = await reviews.CountAsync();
      var page = query.Page > 0 ? query.Page : 1;
      var pageSize = query.PageSize > 0 ? query.PageSize : 10;
      reviews = reviews.Skip((page-1)*pageSize).Take(pageSize).Include(r => r.User).Include(r => r.Product);
    var review = await reviews.ToListAsync();
    return new PagedResult<Review>
    {
        Items = review,
        Page = page,
        PageSize = pageSize,
        TotalCount = total,
        TotalPages = (int)Math.Ceiling((double)total / pageSize)
    };
    }

    public async Task<Response<double>> GetAverageRatingAsync(int productId)
    {
        var reviews = await context.Reviews.Where(o=>o.ProductId==productId).Select(r => r.Rating).ToListAsync();
        var average = reviews.Count == 0 ? 0 : reviews.Average();
        return new Response<double>(HttpStatusCode.OK,"Ok",average);

    }

    public async Task<Response<List<Review>>> GetProductReviewsAsync(int productId)
    {
    var join = await context.Reviews.Where(o=>o.ProductId==productId).Include(r=>r.User)
    .OrderByDescending(r => r.Id).ToListAsync();
         return new Response<List<Review>>(HttpStatusCode.OK,"Ok",join);
    }
}
