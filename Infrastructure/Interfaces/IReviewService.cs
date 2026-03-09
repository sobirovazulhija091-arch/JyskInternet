using Domain.DTOs;
using Infrastructure.Responses;

public interface IReviewService//(Отзывы)
{
    Task<Response<string>> AddReviewAsync(ReviewDto dto);
    Task<Response<List<Review>>> GetProductReviewsAsync(int productId);
    Task<Response<double>> GetAverageRatingAsync(int productId);
     Task<PagedResult<Review>> GetAllAsync(FilterReview filter,PagedQuery query);
}