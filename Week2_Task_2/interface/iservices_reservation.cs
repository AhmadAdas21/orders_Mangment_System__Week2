using Week2_Task_2.Dto.reservation;
using Week2_Task_2.models;

namespace Week2_Task_2
{
    public interface iservices_reservation
    
    {
        Task<reservartion> Create(add_reservation dto);
        Task<reservartion?> GetById(int id);
        Task<bool> Cancel(int id);
        Task<int> ExpireReservations();
        Task<order?> ConvertToOrder(int id);
    }
}
