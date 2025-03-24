using Microsoft.AspNetCore.Mvc;
using ReserveBiteee.Models;

namespace ReserveBiteee.Service
{
    public interface IRegisterService
    {
        public int Register(Register user);
    }
}
